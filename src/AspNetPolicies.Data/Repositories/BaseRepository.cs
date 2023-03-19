using AspNetPolicies.Data.Context;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly DocumentsContext _context;
    
    public BaseRepository(DocumentsContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public IQueryable<T> GetQueryable() => _context.Set<T>().AsQueryable();

    public async Task<T?> GetByIdAsync(object? id) => await _context.Set<T>().FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(object id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        _context.Set<T>().Remove(entity ?? throw new InvalidOperationException("Entity not found"));
        await _context.SaveChangesAsync();
        return entity;
    }
}