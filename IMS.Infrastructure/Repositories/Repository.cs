using IMS.Application.IServices;
using IMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private readonly ILogger<Repository<T>> _logger;

    public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        _logger.LogInformation("Fetching all records of {Entity}", typeof(T).Name);
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include != null)
        {
            query = include(query);
        }

        var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

        if (entity == null)
        {
            _logger.LogWarning("{Entity} with ID {Id} not found.", typeof(T).Name, id);
        }
        else
        {
            _logger.LogInformation("Fetched {Entity} with ID {Id}", typeof(T).Name, id);
        }

        return entity;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Added new {Entity}", typeof(T).Name);
    }

    public async void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated {Entity}", typeof(T).Name);
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _logger.LogInformation("Deleted {Entity}", typeof(T).Name);
    }
}