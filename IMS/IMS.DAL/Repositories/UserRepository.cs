using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Pagination;
using System.Linq.Expressions;
using IMS.DAL.Builders;
using Shared.Filters;

namespace IMS.DAL.Repositories;

public class UserRepository(ImsDbContext context, IRepository<User> repository) : IUserRepository
{
    private readonly DbSet<User> _users = context.Set<User>();

    public async Task<PagedList<User>> GetAllAsync(
        PaginationParameters paginationParameters,
        UserFilteringParameters filter,
        UserSortingParameter sorter,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = _users.AsQueryable();

        query = trackChanges ? query : query.AsNoTracking();
        query = ApplyFilters(query, filter);
        query = ApplySortingOption(query, sorter);

        var users = await query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedList = new PagedList<User>(users, paginationParameters.PageNumber, 
            paginationParameters.PageSize, totalCount);

        return pagedList;
    }

    private static IQueryable<User> ApplyFilters(IQueryable<User> query, UserFilteringParameters filter)
    {
        query = new UserFilterBuilder()
            .WithRole(filter.Role)
            .WithFirstName(filter.FirstName)
            .WithLastName(filter.LastName)
            .Build(query);

        return query;
    }

    private static IQueryable<User> ApplySortingOption(IQueryable<User> query, UserSortingParameter sortingParameter)
    {
        query = sortingParameter switch
        {
            UserSortingParameter.AscendingId => query.OrderBy(u => u.Id),
            UserSortingParameter.AscendingFirstName => query.OrderBy(u => u.Firstname),
            UserSortingParameter.AscendingLastName => query.OrderBy(u => u.Lastname),
            UserSortingParameter.AscendingRole => query.OrderBy(u => u.Role),
            UserSortingParameter.DescendingId => query.OrderByDescending(u => u.Id),
            UserSortingParameter.DescendingFirstName => query.OrderByDescending(u => u.Firstname),
            UserSortingParameter.DescendingLastName => query.OrderByDescending(u => u.Lastname),
            UserSortingParameter.DescendingRole => query.OrderByDescending(u => u.Role),
            _ => query
        };

        return query;
    }

    public Task<List<User>> GetAllAsync(Expression<Func<User, bool>>? predicate = null,
        bool trackChanges = false, CancellationToken cancellationTokent = default) =>
        repository.GetAllAsync(predicate, trackChanges, cancellationTokent);

    public Task<PagedList<User>> GetPagedAsync(Expression<Func<User, bool>>? predicate,
        PaginationParameters paginationParameters, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        repository.GetPagedAsync(predicate, paginationParameters, trackChanges, cancellationToken);

    public Task<User?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) => 
        repository.GetByIdAsync(id, trackChanges, cancellationToken);

    public Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default) =>
        repository.CreateAsync(entity, cancellationToken);

    public Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default) => 
        repository.UpdateAsync(entity, cancellationToken);

    public Task DeleteAsync(User entity, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(entity, cancellationToken);
}
