using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Test.Models
{
  public  class IndentityModels
    {
    }

    public class ApplicationUser : IdentityUser<int>
    {

    }
    public class ApplicationRole : IdentityRole<int>
    {

    }

}
