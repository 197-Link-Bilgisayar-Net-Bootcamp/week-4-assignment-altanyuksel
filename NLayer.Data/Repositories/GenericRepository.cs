using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data.Repositories {
  public class GenericRepository<T> where T : class {
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericRepository(AppDbContext context) {
      this._context = context;
      this._dbSet = this._context.Set<T>(); ;
    }
    public async Task<T> AddAsync(T entity) {
      _context.Set<T>().Add(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<T?> DeleteAsync(int id) {
      var entity = await _context.Set<T>().FindAsync(id);
      if (entity == null) {
        return entity;
      }
      _context.Set<T>().Remove(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task<T?> GetAsync(int id) {
      return await _context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync() {
      return await _context.Set<T>().ToListAsync();
    }
    public List<T> GetAll() {
      return _context.Set<T>().ToList<T>();
    }
    public async Task<List<T>> UpdateAysnc(List<T> listEntity) {
      foreach (var item in listEntity) {
        _context.Entry(item).State = EntityState.Modified;
      }
      await Task.CompletedTask;
      return listEntity;
    }

    public T Update2(T entity) {
      _context.Entry(entity).State = EntityState.Modified;
      return entity;
    }

    public async Task<T?> FindByNameAsync(string name) {
      return await _context.Set<T>().FindAsync(name);
    }
    public IList<T> GetItems(Expression<Func<T, bool>> predicate, params string[] navigationProperties) {
      List<T> list;
      using (var ctx = _context) {
        var query = ctx.Set<T>().AsQueryable();

        foreach (string navigationProperty in navigationProperties)
          query = query.Include(navigationProperty);//got to reaffect it.

        list = query.Where(predicate).ToList<T>();
      }
      return list;
    }

    public async ValueTask<IList<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties) {
      List<T> list;
      var query = _context.Set<T>().AsQueryable();

      foreach (string navigationProperty in navigationProperties)
        query = query.Include(navigationProperty);//got to reaffect it.

      list = query.Where(predicate).ToList<T>();
      await Task.CompletedTask;
      return list;
    }
    public async Task<List<T>> AddRangeAysnc(List<T> listEntity) {
      await _context.Set<List<T>>().AddRangeAsync(listEntity);
      return listEntity;
    }
  }
}