﻿namespace AspNetPolicies.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    IQueryable<TEntity> GetQueryable();
    Task<TEntity?> GetByIdAsync(object? id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(object id);
}