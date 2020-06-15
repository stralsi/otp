using System;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace Otp.Api.Controllers
{
  [ApiController]
  [Route("/api/otp")]
  public class OtpController : ControllerBase
  {
    [HttpPost]
    public ActionResult Create([FromBody] CreateRequest request)
    {
      var secretKey = "12345678901234567890"; // todo: not so secret
      var otpCalc = new Totp(System.Text.Encoding.UTF8.GetBytes(secretKey));
      var otp = otpCalc.ComputeTotp(request.CreatedAt.UtcDateTime);
      var result = new CreateResponse {
        OneTimePassword = otp,
        ExpiresAt = request.CreatedAt.AddSeconds(30)
      };

      return new JsonResult(result);
    }

    public class CreateRequest {
      public string UserId {get;set;}
      public DateTimeOffset CreatedAt {get;set;}
    }

    public class CreateResponse {
      public string OneTimePassword {get;set;}
      public DateTimeOffset ExpiresAt {get;set;}
    }
  }
}