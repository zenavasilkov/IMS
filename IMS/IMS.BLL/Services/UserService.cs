using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Repositories.Interfaces;
using Shared.Enums;
using Shared.Pagination;
using IMS.NotificationsCore.Services;
using IMS.BLL.Mapping;
using User = IMS.DAL.Entities.User;

namespace IMS.BLL.Services;

public class UserService(IUserRepository repository, IMapper mapper, IMessageService messageService) 
    : Service<UserModel, User>(repository, mapper), IUserService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<UserModel> CreateAsync(
        UserModel model,
        CancellationToken cancellationToken = default)
    {
        var existingUsers = await repository.GetPagedAsync(u =>
            u.Email == model.Email,
            new PaginationParameters(1, 1),
            false,
            cancellationToken);
        
        if (existingUsers is not null && existingUsers.TotalCount > 0) 
            throw new EmailIsNotUniqueException("User with this given email already exists");
        
        var createdUser = await base.CreateAsync(model, cancellationToken);
        
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
}
