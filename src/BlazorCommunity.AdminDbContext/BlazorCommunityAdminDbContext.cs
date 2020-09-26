using Element.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorCommunity.AdminDbContext
{
    public class BlazorCommunityAdminDbContext : IdentityDbContext
    {
        public BlazorCommunityAdminDbContext()
        {
        }

        public BlazorCommunityAdminDbContext(DbContextOptions<BlazorCommunityAdminDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
        }

        public DbSet<RoleResource> RoleResources { get; set; }

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

            modelBuilder.Entity<RoleResource>(e =>
            {
                e.HasKey(x => new
                {
                    x.ResourceId,
                    x.RoleId
                });
            });
        }
    }
}