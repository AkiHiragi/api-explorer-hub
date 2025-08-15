using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DataContext;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options) 
    : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    
    
}