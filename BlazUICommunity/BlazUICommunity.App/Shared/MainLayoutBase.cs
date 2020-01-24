using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazUICommunity.App.Shared
{
    //[Authorize]
    public class MainLayoutBase: LayoutComponentBase
    {
        public MainLayoutBase()
        {
            Console.WriteLine(11321);
        }

        public string showsearch()
        {
            Console.WriteLine(11111);
            return "321";
        }
    }
}
