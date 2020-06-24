using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Otp.Api.Controllers;
using Otp.Api.Data;
using Otp.Api.Models;
using Xunit;

namespace Otp.ApiTests
{
  public class OtpTests : IClassFixture<CustomWebApplicationFactory<Otp.Api.Startup>>
  {
    private readonly HttpClient httpClient;
    private readonly ApplicationDbContext dbContext;
    private readonly CustomWebApplicationFactory<Otp.Api.Startup>  webAppFactory;

    private DateTimeOffset testDate = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));
    public OtpTests(CustomWebApplicationFactory<Otp.Api.Startup> factory)
    {
      this.httpClient = factory.CreateClient();
      this.webAppFactory = factory;
      OtpController.UtcNow = () => testDate.AddSeconds(1);
    }

    [Fact]
    public async Task Create_CorrectInput_ReturnsOtpAndExpirationDate()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest
      {
        LoginId = "mary",
        CreatedAt = testDate,
      });

      var response = await httpClient.PostAsync("/api/otp", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      var content = await response.Content.ReadAsStringAsync();
      var parsedContent = JsonSerializer.Deserialize<OtpController.CreateResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      // This test can probably be more sophisticated, but I trust the otp library works
      Assert.NotNull(parsedContent.OneTimePassword);
      // expires 30 seconds later
      Assert.Equal(testDate.AddSeconds(30), parsedContent.ExpiresAt);
    }

    [Fact]
    public async Task Create_UserNotFound_ReturnsUnauthorized()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest
      {
        LoginId = "does not exist",
        CreatedAt = testDate,
      });
      var response = await httpClient.PostAsync("/api/otp", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_NoDate_ReturnsBadRequest()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest
      {
        LoginId = "mary",
      });

      var response = await httpClient.PostAsync("/api/otp", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }    

    [Fact]
    public async Task Verify_CorrectInput_Successful()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.VerifyRequest
      {
        LoginId = "mary",
        OneTimePassword = "687028", //Correct! I found this value by running the topt library on this secret key with the date 2010-01-01 00:00
      });

      var response = await httpClient.PostAsync("/api/otp/verify", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Verify_Incorrectusername_Unauthorized()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.VerifyRequest
      {
        LoginId = "does not exist",
        OneTimePassword = "687028", // Correct! I found this value by running the topt library on this secret key with the date 2010-01-01 00:00
      });

      var response = await httpClient.PostAsync("/api/otp/verify", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Verify_IncorrectPassword_Unauthorized()
    {
      var requestInput = JsonSerializer.Serialize(new OtpController.VerifyRequest
      {
        LoginId = "mary",
        OneTimePassword = "incorrect",
      });

      var response = await httpClient.PostAsync("/api/otp/verify", new StringContent(requestInput, Encoding.UTF8, "application/json"));

      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
  }
}
