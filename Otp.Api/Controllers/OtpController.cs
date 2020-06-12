using System;
using Microsoft.AspNetCore.Mvc;

namespace Otp.Api.Controllers
{
  [ApiController]
  [Route("/api/otp")]
  public class OtpController : ControllerBase
  {
    public ActionResult Get()
    {
      var result = new {
        Password = "foo",
        ExpiresAt = DateTimeOffset.Now.AddMinutes(1)
      };

      return new JsonResult(result);
    }
  }
}