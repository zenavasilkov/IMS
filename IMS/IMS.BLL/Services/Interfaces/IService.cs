using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Pagination;
using System.Linq.Expressions;

namespace IMS.BLL.Services.Interfaces;

public interface IService<TModel, TEntity> 
    where TModel : ModelBase 
    where TEntity : EntityBase
{
    Task<List<TModel>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, 
        bool trackChanges = false, CancellationToken cancellationToken = default);

    Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default);

    Task<TModel> UpdateAsync(Guid id, TModel model, CancellationToken cancellationToken = default);

    Task DeleteAsync(TModel model, CancellationToken cancellationToken = default);
}
