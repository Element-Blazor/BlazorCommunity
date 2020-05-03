using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazui.Community.AppDbContext
{
    public partial class BlazUICommunityContext : IdentityDbContext<BZUserModel, IdentityRole<string>, string>
    {
        public BlazUICommunityContext()
        {
        }

        public BlazUICommunityContext(DbContextOptions<BlazUICommunityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BZFollowModel> BZFollow { get; set; }
        public virtual DbSet<BZPointModel> BZPoint { get; set; }
        public virtual DbSet<BZReplyModel> BZReply { get; set; }
        public virtual DbSet<BZAutho2Model> BZOauth { get; set; }
        public virtual DbSet<BZTopicModel> BZTopic { get; set; }
        public virtual DbSet<BZUserModel> BZUser { get; set; }
        public virtual DbSet<BZAddressModel> BZAddress { get; set; }
        public virtual DbSet<BzVerifyCodeModel> BzVerifyCode { get; set; }
        public virtual DbSet<BZVersionModel> BZVersion { get; set; }
        public virtual DbSet<BzBannerModel> BzBanner { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BZUserModel>().ToTable("BZUser");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("BZRole");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("BZUserRole");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("BZRoleClaim");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("BZUserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("BZUserLogin");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("BZUserToken");
        }
    }
}