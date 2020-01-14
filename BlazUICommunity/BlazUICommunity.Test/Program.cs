using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using ( BlazUICommunityContext  communityContext=new BlazUICommunityContext() )
            {
                communityContext.Database.EnsureCreated();
                //var s=   communityContext.Find<SysUser>(1);
            }
            Console.ReadLine();
        }
    }
}
