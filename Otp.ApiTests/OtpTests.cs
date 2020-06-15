using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Otp.Api.Controllers;
using Xunit;

namespace Otp.ApiTests
{
    public class OtpTests
    {
        public OtpTests()
        {
            InitializeUsers();
        }
        [Fact]
        public async Task Post_CorrectInput_ReturnsOtpAndExpirationDate()
        {
            CreateUser(new User{Id = "userFoo", SecretKey="12345678901234567890"});
            var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest {
                UserId = "userFoo",
                CreatedAt = new DateTimeOffset(2020,01,01,0,0,0, new TimeSpan(0,0,0))
            });

            var response = await CreateHttpClient().PostAsync("/api/otp", new StringContent(requestInput,Encoding.UTF8,"application/json"));
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonSerializer.Deserialize<OtpController.CreateResponse>(content,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // This test can probably be more sophisticated, but I trust the otp library works
            Assert.NotNull(parsedContent.OneTimePassword);
            // expires 30 seconds later
            Assert.Equal(new DateTimeOffset(2020,01,01,0,0,30, new TimeSpan(0,0,0)), parsedContent.ExpiresAt);
        }

        [Fact]
        public async Task Post_UserNotFound_Returns404()
        {
            CreateUser(new User{Id = "userFoo", SecretKey="12345678901234567890"});
            var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest {
                UserId = "does-not-exist",
                CreatedAt = DateTime.Now
            });

            var response = await CreateHttpClient().PostAsync("/api/otp", new StringContent(requestInput,Encoding.UTF8,"application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private HttpClient CreateHttpClient(){
            var server = new TestServer(new WebHostBuilder().UseStartup<Otp.Api.Startup>());
            return server.CreateClient();
        }

        private void InitializeUsers() {
            OtpController.Users = new List<User>();
        }
        private void CreateUser(User user) {
            OtpController.Users.Add(user);
        }
    }
}
