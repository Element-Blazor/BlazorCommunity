using Blazui.Community.Model.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazui.Community.Model.Models
{
    public class BlazUICommunityAdminDbContext : IdentityDbContext
    {
        public BlazUICommunityAdminDbContext()
        {

        }
        public BlazUICommunityAdminDbContext(DbContextOptions<BlazUICommunityAdminDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySql("server=192.168.205.92;database=blazuiadmin;port=3306;uid=root;password=P@ssw0rd;character set=utf8mb4;");
            optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
        }

        /// <summary>
        /// 在模型创建时
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置实体类型映射到的表名        
            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

        }

    }
}
