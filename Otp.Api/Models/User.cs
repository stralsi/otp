using System;

namespace Otp.Api.Models
{
  public class User
  {
    public Guid Id {get;set;}
    public string SecretKey {get;set;}
  }
}