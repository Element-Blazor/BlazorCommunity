using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.Test
{
   public class TestDbContext:BlazorCommunity.AppDbContext.BlazorCommunityContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=BlazorCommunity;port=3306;uid=root;password=P@ssw0rd123;character set=utf8mb4;");
        }
    }
}
