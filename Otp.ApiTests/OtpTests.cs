using System;
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
        [Fact]
        public async Task Post_ReturnsOtpAndExpirationDate()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Otp.Api.Startup>());
            var client =  server.CreateClient();
            var requestInput = JsonSerializer.Serialize(new OtpController.CreateRequest {
                UserId = "userFoo",
                CreatedAt = new DateTimeOffset(2020,01,01,0,0,0, new TimeSpan(0,0,0))
            });

            var response = await client.PostAsync("/api/otp", new StringContent(requestInput,Encoding.UTF8,"application/json"));
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonSerializer.Deserialize<OtpController.CreateResponse>(content,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // This test can probably be more sophisticated, but I trust the otp library works
            Assert.NotNull(parsedContent.OneTimePassword);
            // expires 30 seconds later
            Assert.Equal(new DateTimeOffset(2020,01,01,0,0,30, new TimeSpan(0,0,0)), parsedContent.ExpiresAt);
        }
    }
}