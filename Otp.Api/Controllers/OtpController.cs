using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otp.Api.Data;
using OtpNet;

namespace Otp.Api.Controllers
{
  [ApiController]
  [Route("/api/otp")]
  public class OtpController : ControllerBase
  {
    public static Func<DateTimeOffset> UtcNow = () => DateTimeOffset.UtcNow;
    private readonly ApplicationDbContext dbContext;

    public OtpController(ApplicationDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateRequest request)
    {
      var user = await dbContext.Users.FindAsync(request.UserId);
      if(user == null) {
        return NotFound();
      }
      var otpCalc = new Totp(System.Text.Encoding.UTF8.GetBytes(user.SecretKey));
      var otp = otpCalc.ComputeTotp(request.CreatedAt.UtcDateTime);
      var result = new CreateResponse {
        OneTimePassword = otp,
        ExpiresAt = request.CreatedAt.AddSeconds(30)
      };

      return new JsonResult(result);
    }

    [HttpPost("verify")]
    public async Task<ActionResult> Verify([FromBody] VerifyRequest request) 
    {
      var user = await dbContext.Users.FindAsync(request.UserId);
      if(user == null) {
        return Unauthorized();
      }
      var otpCalc = new Totp(System.Text.Encoding.UTF8.GetBytes(user.SecretKey));
      var validPassword = otpCalc.VerifyTotp(
        UtcNow().DateTime,
        request.OneTimePassword,
        out var timeStepMatched,
        VerificationWindow.RfcSpecifiedNetworkDelay
      );
      if(validPassword) {
        return Ok();
      }else {
        return Unauthorized();
      }
    }

    public class CreateRequest {
      public Guid UserId {get;set;}
      public DateTimeOffset CreatedAt {get;set;}
    }

    public class CreateResponse {
      public string OneTimePassword {get;set;}
      public DateTimeOffset ExpiresAt {get;set;}
    }

    public class VerifyRequest
    {
      public Guid UserId {get;set;}      
      public string OneTimePassword { get; set; }
    }
  }
}