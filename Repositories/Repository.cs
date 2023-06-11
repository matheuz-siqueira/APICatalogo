using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using APICatalogo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APICatalogo.Repositories;

public class Repository<T> : IRepository<T> where T : class 
{
    private readonly Context _context;
    public Repository([FromServices] Context context)
    {
        _context = context; 
    }

    public void Add(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> Get()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public async Task<T> GetById(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
