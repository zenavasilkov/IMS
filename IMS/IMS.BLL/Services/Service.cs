using System.Linq.Expressions; 
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using Shared.Pagination;
using IMS.DAL.Repositories.Interfaces;
using AutoMapper;
using IMS.BLL.Exceptions;

namespace IMS.BLL.Services;

public class Service<TModel, TEntity>(IRepository<TEntity> repository, IMapper mapper) 
    : IService<TModel, TEntity> 
    where TModel : ModelBase
    where TEntity : EntityBase
{  
    public virtual async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<TEntity>(model);
        var createdEntity = await repository.CreateAsync(entity, cancellationToken);
        var createdModel = mapper.Map<TModel>(createdEntity);

        return createdModel;
    }

    public virtual async Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<TEntity>(model);
        await repository.DeleteAsync(entity, cancellationToken); 
    }

    public virtual async Task<List<TModel>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, 
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(predicate, trackChanges, cancellationToken);
        var models = mapper.Map<List<TModel>>(entities);

        return models;
    }

    public virtual async Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken: cancellationToken) 
            ?? throw new NotFoundException($"No record has been found by given ID {id}");

        var model = mapper.Map<TModel>(entity);

        return model;
    }

    public virtual async Task<TModel> UpdateAsync(Guid id, TModel model, CancellationToken cancellationToken = default)
    {
        _ = await repository.GetByIdAsync(id, cancellationToken: cancellationToken) 
            ?? throw new NotFoundException($"There is no data have been found with ID {id}");

        model.Id = id; 

        var entity = mapper.Map<TEntity>(model);

        var updatedEntity = await repository.UpdateAsync(entity, cancellationToken);

        var updatedModel = mapper.Map<TModel>(updatedEntity);

        return updatedModel;
    }
}
