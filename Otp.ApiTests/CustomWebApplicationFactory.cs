using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Otp.Api.Data;
using Otp.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Otp.ApiTests
{
  public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
  {
    public HttpClient HttpClient {get; private set;}

    private IServiceScope serviceScope;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    { 
      builder.ConfigureServices(services =>
      {
        // Remove the app's ApplicationDbContext registration.
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        // Add ApplicationDbContext using an in-memory database for testing.
        services.AddDbContext<ApplicationDbContext>(options =>
        {
          var connectionString = "Server=localhost;Database=otp_dev;User=sa;Password=Development_password123;";
          options.UseSqlServer(connectionString);
        });

        // Build the service provider.
        var sp = services.BuildServiceProvider();

        using (var scope = sp.CreateScope())
        {
          var scopedServices = scope.ServiceProvider;
          var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
          var logger = scopedServices
              .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

          // Ensure the database is created.
          dbContext.Database.EnsureCreated();

          try
          {
            SeedDb(dbContext);
          }
          catch (Exception ex)
          {
              logger.LogError(ex, "An error occurred seeding the " +
                  "database with test messages. Error: {Message}", ex.Message);
          }
        }
      });
    }
    public void SeedDb(ApplicationDbContext dbContext)
    {
      dbContext.Users.RemoveRange(dbContext.Users);
      var users = new List<User>()
      {
        new User() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), SecretKey="00000000000000000001" },
        new User() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), SecretKey="00000000000000000002" },
        new User() { Id = Guid.Parse("00000000-0000-0000-0000-123456789012"), SecretKey="12345678901234567890" }
      };
      dbContext.Users.AddRange(users);
      dbContext.SaveChanges();
    }
  }
}