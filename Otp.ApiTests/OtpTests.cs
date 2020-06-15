using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Otp.ApiTests
{
    public class OtpTests
    {
        [Fact]
        public async Task Post_ReturnsSuccess()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Otp.Api.Startup>());
            var client =  server.CreateClient();
            var requestInput = JsonSerializer.Serialize(new {
                UserId = "userFoo",
                DateTime = DateTime.Now
            });
            
            var response = await client.PostAsync("/api/otp", new StringContent(requestInput,Encoding.UTF8,"application/json"));
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("userFoo", content);
        }
    }
}
