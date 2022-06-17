using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data {
  public class UnitOfWork {
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context) {
      _context = context;
    }
    public async Task CommitAsyn() {
      await Task.CompletedTask;
      await _context.SaveChangesAsync();
    }
    public async Task Commit() {
      await Task.CompletedTask;
      _context.SaveChanges();
    }
    //public IDbContextTransaction BeginTransaction() {
    //  return _context.Database.BeginTransaction();
    //}
  }
}