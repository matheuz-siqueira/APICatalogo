using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; 

namespace APICatalogo.Data;

public class Context : DbContext  
{
    public Context(DbContextOptions<Context> options ) : base (options) {}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }   
    public DbSet<User> Users { get; set; }
}
