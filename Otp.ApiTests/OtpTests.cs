using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Otp.ApiTests
{
    public class OtpTests
    {
        [Fact]
        public async Task Get_ReturnsSuccess()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Otp.Api.Startup>());
            var client =  server.CreateClient();
            var response = await client.GetAsync("/api/otp");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("foo", content);
        }
    }
}
