using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using IMS.BLL.Exceptions;
using IMS.BLL.Services;
using IMS.BLL.Services.Interfaces;
using IMS.DAL;
using IMS.DAL.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Shared.Dictionaries;

namespace IMS.BLL.BackgroundServices;

public class Auth0OutboxProcessor(
    ImsDbContext dbContext,
    IAuth0ClientFactory auth0ClientFactory,
    IConfiguration configuration,
    ILogger<Auth0OutboxProcessor> logger) : IJob
{
    private const int BatchSize = 20;
    private readonly string _connection = configuration["Auth0:Connection"] ??
        throw new MissingConfigurationException("Missing 'Auth0:Connection' property in configurations");
    
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.Id)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);
        
        if (messages.Count == 0) return;
        
        var auth0Client = await auth0ClientFactory.CreateClientAsync();

        foreach (var message in messages)
        {
            var email = "";

            try
            {
                if (message.Type.Contains("UserRoleUpdate"))
                {
                    var updatePayload = JsonConvert.DeserializeObject<OutboxUserRoleUpdate>(message.Content);
                    
                    if (updatePayload is null) continue;

                    email = updatePayload.Email;

                    await ProcessUserRoleUpdateOutboxMessageAsync(updatePayload, auth0Client);
                }
                else if (message.Type.Contains("User"))
                {
                    var createAuth0User = JsonConvert.DeserializeObject<CreateAuth0User>(message.Content);

                    if (createAuth0User is null) return;
                    
                    email = createAuth0User.Email;
                    
                    await ProcessCreateAuth0UserAsync(createAuth0User, auth0Client);
                }
                
                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (ErrorApiException auth0Exception) when (auth0Exception.Message.Contains("The user already exists"))
            {
                message.ProcessedOnUtc = DateTime.UtcNow;
                message.Error = auth0Exception.Message;
                
                logger.LogWarning(auth0Exception,
                    "Outbox message {MessageId}: User with Email {Email} already existed. Marked as processed to prevent infinite loop.",
                    message.Id, email);
            }
            catch (Exception exception)
            {
                message.Error = exception.Message;

                logger.LogError(exception, "Outbox message processing failed. Type={Type}, MessageId={MessageId}",
                    message.Type, message.Id);
            }
        }
        
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task ProcessUserRoleUpdateOutboxMessageAsync(OutboxUserRoleUpdate message, ManagementApiClient auth0Client)
    {
        var auth0User = await auth0Client.Users.GetUsersByEmailAsync(message.Email);
        var userId = auth0User.FirstOrDefault()?.UserId;

        if (string.IsNullOrEmpty(userId))
            throw new NotFoundException($"Auth0 user not found for email: {message.Email}");

        if (!Auth0Roles.Roles.TryGetValue(message.NewRole, out var newAuth0RoleId))
            throw new NotFoundException( $"Role: {message.NewRole} was not found in Auth0Roles mapping.");

        await auth0Client.Users.AssignRolesAsync(userId,
            new AssignRolesRequest { Roles = [newAuth0RoleId] });

        logger.LogInformation("Role updated for Auth0 User: {Email} to {Role}", message.Email,
            message.NewRole);
    }

    private async Task ProcessCreateAuth0UserAsync(CreateAuth0User message, ManagementApiClient auth0Client)
    {
        var auth0UserRequest = new UserCreateRequest
        {
            Email = message.Email,
            Connection = _connection,
            EmailVerified = false,
            Password = PasswordGenerator.GenerateRandomPassword()
        };

        var auth0User = await auth0Client.Users.CreateAsync(auth0UserRequest);

        if (!Auth0Roles.Roles.TryGetValue(message.Role, out var auth0RoleId))
            throw new NotFoundException($"Role: {message.Role} was not found");

        await auth0Client.Users.AssignRolesAsync(auth0User.UserId, new AssignRolesRequest { Roles = [auth0RoleId] });

        await auth0Client.Tickets.CreatePasswordChangeTicketAsync(new PasswordChangeTicketRequest
        {
            UserId = auth0User.UserId
        });

        logger.LogInformation("User with Email: {Email} was successfully registered in Auth0", message.Email);
    }
}
