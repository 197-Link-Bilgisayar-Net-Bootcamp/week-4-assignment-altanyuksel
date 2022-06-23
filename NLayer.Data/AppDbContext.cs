using Microsoft.EntityFrameworkCore;
using NLayer.Data.Models;
using NLayer.Data.Models.JWTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data {
  public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<User> Users { get; set; }
    public override int SaveChanges() {
      SetDates();
      return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
      SetDates();
      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    private void SetDates() {
      ChangeTracker.Entries().ToList().ForEach(e => {
        if (e.Entity is Product || e.Entity is Category) {
          var p = e.Entity as BaseModel;
          if (p != null) {
            if (e.State == EntityState.Added) {
              p.CreatedDate = DateTime.Now;
              p.UpdatedDate = DateTime.Now;
            }
            else if (e.State == EntityState.Modified) {
              p.UpdatedDate = DateTime.Now;
            }
          }
        }
      });
    }
  }
}