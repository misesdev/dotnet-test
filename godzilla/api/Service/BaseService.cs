using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api.Service;

public abstract class BaseService<Entity> where Entity : BaseModel 
{
    private readonly AppDbContex _dbContext;
    private readonly DbSet<Entity> _dbSet;

    public BaseService(AppDbContex context) {
        this._dbContext = context;
        this._dbSet = context.Set<Entity>();
    }

    protected virtual async Task<List<Entity>> GetAllAsync(int itemsPerPage = 10, int page = 1, bool paginate = false)
    {
        if(!paginate) {
            return await _dbSet
                .Where(entity => entity.DeletedAt == null)
                .ToListAsync();
        }

        return await _dbSet
            .Where(entity => entity.DeletedAt == null)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync();
    }

    protected virtual async Task<Entity> AddAsync(Entity entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.Now;

        _dbSet.Add(entity);

        await _dbContext.SaveChangesAsync();

        return entity;
    }

    protected virtual async Task<Entity?> FindAsync(Expression<Func<Entity, bool>> find)  
    {
        return await _dbSet.Where(find).FirstOrDefaultAsync();
    }

    protected virtual async Task<Entity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    protected virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.DeletedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
    }

    protected virtual async Task<List<Entity>> FilterAsync(Expression<Func<Entity, bool>> filter) 
    {
        return await _dbSet.Where(filter).ToListAsync();
    }

    protected virtual async Task<bool> Exists(Expression<Func<Entity, bool>> find) 
    {
        return await _dbSet.AnyAsync(find);
    }
}
