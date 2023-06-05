using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options ) : base (options) {}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }   
}
