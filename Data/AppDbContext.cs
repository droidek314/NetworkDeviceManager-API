using Microsoft.EntityFrameworkCore;
using CSharpApi.Models;

namespace CSharpApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<NetworkDevice> Devices { get; set; }
}