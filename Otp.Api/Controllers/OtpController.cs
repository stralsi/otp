using System;
using Microsoft.AspNetCore.Mvc;

namespace Otp.Api.Controllers
{
  [ApiController]
  [Route("/api/otp")]
  public class OtpController : ControllerBase
  {
    [HttpPost]
    public ActionResult Create([FromBody] OtpRequest request)
    {
      var result = new {
        Password = request.UserId,
        ExpiresAt = DateTimeOffset.Now.AddMinutes(1)
      };

      return new JsonResult(result);
    }

    public class OtpRequest {
      public string UserId {get;set;}
      public DateTime DateTime {get;set;}
    }
  }
}