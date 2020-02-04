using System;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Blazui.Community.Test
{
    public partial class BlazUICommunityContext1 : IdentityDbContext
    {
       // public static readonly ILoggerFactory MyLoggerFactory
       //= LoggerFactory.Create(builder =>
       //{
       //    builder
       //     .AddFilter((category , level) =>
       //         category == DbLoggerCategory.Database.Command.Name
       //         && level == LogLevel.Information)
       //     .AddConsole();
       //});
        public BlazUICommunityContext1()
        {
        }

        public BlazUICommunityContext1(DbContextOptions<BlazUICommunityContext1> options)
            : base(options)
        {
        }

        public virtual DbSet<BZFollowModel> Follow { get; set; }
        public virtual DbSet<BZPointModel> Point { get; set; }
        public virtual DbSet<BZReplyModel> Reply { get; set; }
        public virtual DbSet<BZThirdAccountModel> Thirdaccount { get; set; }
        public virtual DbSet<BZTopicModel> Topic { get; set; }
        public virtual DbSet<BZUserModel> User { get; set; }
        public virtual DbSet<BZAddressModel> Useraddress { get; set; }
        public virtual DbSet<SysLogModel> SysLog { get; set; }
        public virtual DbSet<SysUserModel> SysUser { get; set; }
        public virtual DbSet<SysMenuModel> SysMenu { get; set; }
        public virtual DbSet<SysRoleModel> SysRole { get; set; }
        public virtual DbSet<SysRoleMenuMappingModel> SysRoleMenuMapping { get; set; }
        public virtual DbSet<SysUserMenuMappingModel> SysUserMenuMapping { get; set; }
        public virtual DbSet<SysUserRoleMappingModel> SysUserRoleMapping { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //throw new ArgumentException("dbconnection string is not Configured");
                optionsBuilder.UseMySql("server=localhost;database=BlazuiCommunity;port=3306;uid=root;password=P@ssw0rd123;character set=utf8mb4;");
            }
            //optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            OnModelCreatingPartial(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
