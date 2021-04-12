using System;
using System.Collections.Generic;
using AspNet_MVC_Training.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNet_MVC_Training.Areas.Identity.Data
{
  public class ApplicationUser : IdentityUser {
    public string Name { get; set; }
    public ICollection<UserTraining> UserTrainings { get; set; }
  }
}