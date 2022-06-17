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
    //protected override void OnModelCreating(ModelBuilder modelBuilder) {
    //  modelBuilder.Entity<User>().HasKey("Id");
    //  modelBuilder.Entity<List<User>>().HasData(new User {
    //    Name = "user1",
    //    Password = "password1"
    //  },
    //  new User {
    //    Name = "user2",
    //    Password = "password2"
    //  },
    //  new User {
    //    Name = "user3",
    //    Password = "password3"
    //  });
    //}
  }
}