using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace Otp.Api.Controllers
{
  [ApiController]
  [Route("/api/otp")]
  public class OtpController : ControllerBase
  {
    // let's pretend this is a database
    public static List<User> Users = new List<User> {
      new User {
        Id = "someUser",
        SecretKey = "12345678901234567890"
      }
    };
    public static Func<DateTimeOffset> UtcNow = () => DateTimeOffset.UtcNow;

    [HttpPost]
    public ActionResult Create([FromBody] CreateRequest request)
    {
      var user = Users.Find(u => u.Id == request.UserId);
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
    public ActionResult Verify([FromBody] VerifyRequest request) 
    {
      var user = Users.Find(u => u.Id == request.UserId);
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
      public string UserId {get;set;}
      public DateTimeOffset CreatedAt {get;set;}
    }

    public class CreateResponse {
      public string OneTimePassword {get;set;}
      public DateTimeOffset ExpiresAt {get;set;}
    }

    public class VerifyRequest
    {
      public string UserId {get;set;}      
      public string OneTimePassword { get; set; }
    }
  }
  public class User {
      public string Id {get;set;}
      public string SecretKey {get;set;}
  }

}