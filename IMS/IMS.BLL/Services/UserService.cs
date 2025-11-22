using AutoMapper;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using IMS.BLL.Exceptions;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Repositories.Interfaces;
using Shared.Enums;
using Shared.Pagination;
using IMS.NotificationsCore.Services;
using IMS.BLL.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shared.Dictionaries;
using User = IMS.DAL.Entities.User;

namespace IMS.BLL.Services;

public class UserService(
    IUserRepository repository,
    IMapper mapper,
    IMessageService messageService,
    IAuth0ClientFactory auth0ClientFactory,
    IWebHostEnvironment environment) 
    : Service<UserModel, User>(repository, mapper), IUserService
{
    private readonly IMapper _mapper = mapper;

    public async Task<UserModel> CreateAsync(
        UserModel model,
        string connection,
        CancellationToken cancellationToken = default)
    {
        var existingUsers = await repository.GetPagedAsync(u =>
            u.Email == model.Email,
            new PaginationParameters(1, 1),
            false,
            cancellationToken);
        
        if (existingUsers.TotalCount > 0) 
            throw new EmailIsNotUniqueException("User with this given email already exists");
        
        var createdUser = await base.CreateAsync(model, cancellationToken);

        if (!environment.IsEnvironment("Tests"))
        {
            var client = await auth0ClientFactory.CreateClientAsync();
            
            await CreateAuth0User(client, model, connection);
        }
        
        var message = EventMapper.ConvertToUserCreatedEvent(createdUser);
        await messageService.NotifyUserCreated(message, cancellationToken);
        
        return createdUser;
    }

    public override async Task<UserModel> UpdateAsync(Guid id, 
        UserModel model, CancellationToken cancellationToken = default)
    {
        var existingUser = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User with ID {id} was not found");

        existingUser.Email = model.Email;
        existingUser.Firstname = model.Firstname;
        existingUser.Lastname = model.Lastname;
        existingUser.Patronymic = model.Patronymic;
        existingUser.PhoneNumber = model.PhoneNumber;
        existingUser.Role = model.Role;

        var updatedUser = await repository.UpdateAsync(existingUser, cancellationToken: cancellationToken);

        var updatedUserModel = _mapper.Map<UserModel>(updatedUser);

        return updatedUserModel;
    }

    public async Task<PagedList<UserModel>> GetUsersAsync(
        PaginationParameters paginationParameters, 
        UserFilteringParameters filter, 
        UserSortingParameter sorter, 
        CancellationToken cancellationToken = default)
    {
        var users = await repository.GetAllAsync(paginationParameters, 
            filter, sorter, cancellationToken: cancellationToken);

        var userModels = _mapper.Map<PagedList<UserModel>>(users);

        return userModels;
    }

    private static async Task CreateAuth0User(
        ManagementApiClient auth0Client,
        UserModel model,
        string connection)
    {
        var auth0UserRequest = new UserCreateRequest
        {
            Email = model.Email,
            Connection = connection,
            EmailVerified = false,
            Password = model.Email
        };
        
        var auth0User = await auth0Client.Users.CreateAsync(auth0UserRequest);
        
        if (!Auth0Roles.Roles.TryGetValue(model.Role.ToString(), out var auth0RoleId))
            throw new NotFoundException($"Role: {model.Role} was not found");
        
        await auth0Client.Users.AssignRolesAsync(auth0User.UserId,
            new AssignRolesRequest { Roles = [auth0RoleId] });

        await auth0Client.Tickets.CreatePasswordChangeTicketAsync(new PasswordChangeTicketRequest
        {
            UserId = auth0User.UserId,
            //ResultUrl = "https://my-app.com/login" TODO: do not forget to redirect to change the password
        });
    }
}
