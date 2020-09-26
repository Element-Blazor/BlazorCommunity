using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Test
{
   public class TestDbContext:Blazui.Community.AppDbContext.BlazUICommunityContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=blazuicommunity;port=3306;uid=root;password=P@ssw0rd123;character set=utf8mb4;");
        }
    }
}
