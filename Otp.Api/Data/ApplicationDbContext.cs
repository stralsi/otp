using Microsoft.EntityFrameworkCore;
using Otp.Api.Models;

namespace Otp.Api.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<User> Users { get; set; }
  }
}