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
using Microsoft.Extensions.Configuration;

namespace Otp.ApiTests
{
  public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
  {
    public HttpClient HttpClient {get; private set;}

    private IServiceScope serviceScope;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    { 
      var testConfig = new ConfigurationBuilder()
        .AddJsonFile("appSettings.Test.json")
        .Build();
      builder.UseConfiguration(testConfig);
      builder.ConfigureServices(services =>
      {
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
        new User() { LoginId = "betty", SecretKey="00000000000000000001" },
        new User() { LoginId = "john", SecretKey="00000000000000000002" },
        new User() { LoginId = "mary", SecretKey="12345678901234567890" }
      };
      dbContext.Users.AddRange(users);
      dbContext.SaveChanges();
    }
  }
}