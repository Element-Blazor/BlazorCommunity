using Blazui.Community.Model.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazui.Community.Model.Models
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
            modelBuilder.Entity<BZUserModel>().ToTable("BZUser");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("BZRole");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("BZUserRole");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("BZRoleClaim");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("BZUserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("BZUserLogin");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("BZUserToken");

            //  string userId = Guid.NewGuid().ToString();
            //  string topicId = Guid.NewGuid().ToString();
            //  string versionId = Guid.NewGuid().ToString();
            //  modelBuilder.Entity<BZFollowModel>().HasData(new BZFollowModel() { CreateDate = DateTime.Now, CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, TopicId = topicId, Id = Guid.NewGuid().ToString() });
            //  modelBuilder.Entity<BZTopicModel>().HasData(new BZTopicModel() { CreateDate = DateTime.Now, CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, Id = topicId, Category = 0, Content = "测试内容", Good = 0, Hot = 0, ReplyCount = 0, Title = "测试内容标题", Top = 0, VersionId = versionId });
            //  modelBuilder.Entity<BZReplyModel>().HasData(new BZReplyModel() { CreateDate = DateTime.Now, Id = Guid.NewGuid().ToString(), CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, TopicId = topicId, Content = "测试回复内容", Top = 0, Favor = 0, Good = 0 });
            //  modelBuilder.Entity<BZPointModel>().HasData(new BZPointModel() { CreateDate = DateTime.Now, Id = Guid.NewGuid().ToString(), CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, Score = 0, UserId = userId, Description = "测试积分", Access = 0 });
            //  modelBuilder.Entity<BzBannerModel>().HasData(new BzBannerModel() { CreateDate = DateTime.Now, Id = Guid.NewGuid().ToString(), CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, BannerImg = "测试banner", Show = true, Title = "测试banner标题" });
            //  modelBuilder.Entity<BZAddressModel>().HasData(new BZAddressModel()
            //  {
            //      CreateDate = DateTime.Now,
            //      CreatorId = userId,
            //      LastModifierId = userId,
            //      LastModifyDate = DateTime.Now,
            //      Status = 0,
            //      UserId = userId,
            //      City = "常德",
            //      Country = "中国"
            //,
            //      Id = Guid.NewGuid().ToString(),
            //      District = "桃源",
            //      Province = "湖南"
            //  });
            //  modelBuilder.Entity<BZIDCardModel>().HasData(new BZIDCardModel() { CreateDate = DateTime.Now, CreatorId = userId, LastModifierId = userId, LastModifyDate = DateTime.Now, Status = 0, IdentityNo = "231313131", PhotoBehind = "behandA", PhotoFront = "PhotoFront", UserName = "UserName", UserId = userId, Id = Guid.NewGuid().ToString() });
            //  modelBuilder.Entity<BZOauthModel>().HasData(new BZOauthModel()
            //  {
            //      CreateDate = DateTime.Now,
            //      CreatorId = userId,
            //      LastModifierId = userId,
            //      LastModifyDate = DateTime.Now,
            //      Status = 0,
            //      UserId = userId,
            //      HomePage = "HomePage",
            //      NickName = "NickName",
            //      OauthName = "OauthName",
            //      OauthType = 0,
            //      Photo = "",
            //      Id = Guid.NewGuid().ToString()
            //  });
            //  modelBuilder.Entity<BZUserModel>().HasData(new BZUserModel()
            //  {
            //      CreateDate = DateTime.Now,
            //      CreatorId = userId,
            //      LastModifierId = userId,
            //      LastModifyDate = DateTime.Now,
            //      Status = 0
            //   ,
            //      Avator = "Avator",
            //      Email = "Email@qq.com",
            //      LastLoginAddr = "LastLoginAddr",
            //      LastLoginDate = DateTime.Now,
            //      LastLoginType = 0,
            //      Level = 0,
            //      NickName = "NickName",
            //      UserName = "剑之初",
            //      Signature = "在下，剑之初",
            //      Points = 0,
            //      PhoneNumber = "PhoneNumber",
            //      Sex = 0,
            //      Id = Guid.NewGuid().ToString()
            //  });
            //  modelBuilder.Entity<BzVerifyCodeModel>().HasData(new BzVerifyCodeModel()
            //  {
            //      CreateDate = DateTime.Now,
            //      CreatorId = userId,
            //      LastModifierId = userId,
            //      LastModifyDate = DateTime.Now,
            //      Status = 0,
            //      UserId = userId,
            //      ExpireTime = DateTime.Now,
            //      VerifyCode = "1234",
            //      VerifyType = 0,
            //      Id = Guid.NewGuid().ToString()
            //  });
            //  modelBuilder.Entity<BZVersionModel>().HasData(new BZVersionModel()
            //  {
            //      CreateDate = DateTime.Now,
            //      CreatorId = userId,
            //      LastModifierId = userId,
            //      LastModifyDate = DateTime.Now,
            //      Status = 0,
            //      Project = 0,
            //      VerDescription = "VerDescription",
            //      VerDownUrl = "VerDownUrl",
            //      VerName = "VerName",
            //      VerNo = "VerNo",
            //      VerNuget = "VerNuget",
            //      Id = Guid.NewGuid().ToString()
            //  });
        }
    }
}