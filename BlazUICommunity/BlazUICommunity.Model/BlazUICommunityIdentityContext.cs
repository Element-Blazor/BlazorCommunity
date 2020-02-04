using System;
using Blazui.Community.Utility.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Blazui.Community.Model.Models
{
    public partial class BlazUICommunityIdentityContext : IdentityDbContext
    {
      
        public BlazUICommunityIdentityContext()
        {
        }

        public BlazUICommunityIdentityContext(DbContextOptions<BlazUICommunityIdentityContext> options)
            : base(options)
        {
        }


        public virtual DbSet<BZUserModel> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //throw new ArgumentException("dbconnection string is not Configured");
                optionsBuilder.UseMySql("server=localhost;database=BlazuiCommunity;port=3306;uid=root;password=P@ssw0rd123;character set=utf8mb4;");
            }
            optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
