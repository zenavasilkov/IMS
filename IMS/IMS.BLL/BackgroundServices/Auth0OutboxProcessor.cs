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
            try
            {
                var createAuth0User = JsonConvert.DeserializeObject<CreateAuth0User>(message.Content);

                if (createAuth0User is null) return;
                
                var auth0UserRequest = new UserCreateRequest
                {
                    Email = createAuth0User.Email,
                    Connection = _connection,
                    EmailVerified = false,
                    Password = PasswordGenerator.GenerateRandomPassword()
                };
                
                var auth0User = await auth0Client.Users.CreateAsync(auth0UserRequest);
                
                if (!Auth0Roles.Roles.TryGetValue(createAuth0User.Role, out var auth0RoleId))
                    throw new NotFoundException($"Role: {createAuth0User.Role} was not found");
                
                await auth0Client.Users.AssignRolesAsync(auth0User.UserId,
                    new AssignRolesRequest { Roles = [auth0RoleId] });

                await auth0Client.Tickets.CreatePasswordChangeTicketAsync(new PasswordChangeTicketRequest
                {
                    UserId = auth0User.UserId
                });
                
                message.ProcessedOnUtc = DateTime.UtcNow;
                
                logger.LogInformation("User with Email: {Email} was successfully registered in Auth0", createAuth0User.Email);
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
}
