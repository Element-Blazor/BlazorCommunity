//using System;
//using Blazui.Community.Model.Models;
//using Blazui.Community.Utility.Logger;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.Extensions.Logging;

//namespace Blazui.Community.Test
//{
//    public partial class BlazUICommunityContext : IdentityDbContext<BZUserModel, ApplicationRole, int>
//    {
//        // public static readonly ILoggerFactory MyLoggerFactory
//        //= LoggerFactory.Create(builder =>
//        //{
//        //    builder
//        //     .AddFilter((category , level) =>
//        //         category == DbLoggerCategory.Database.Command.Name
//        //         && level == LogLevel.Information)
//        //     .AddConsole();
//        //});
//        public BlazUICommunityContext()
//        {
//        }

//        public BlazUICommunityContext(DbContextOptions<BlazUICommunityContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<BZFollowModel> Follow { get; set; }
//        public virtual DbSet<BZPointModel> Point { get; set; }
//        public virtual DbSet<BZReplyModel> Reply { get; set; }
//        public virtual DbSet<BZThirdAccountModel> Thirdaccount { get; set; }
//        public virtual DbSet<BZTopicModel> Topic { get; set; }
//        public virtual DbSet<BZUserModel> User { get; set; }
//        public virtual DbSet<BZAddressModel> Useraddress { get; set; }
//        public virtual DbSet<SysLogModel> SysLog { get; set; }
//        public virtual DbSet<SysUserModel> SysUser { get; set; }
//        public virtual DbSet<SysMenuModel> SysMenu { get; set; }
//        public virtual DbSet<SysRoleModel> SysRole { get; set; }
//        public virtual DbSet<BzVerifyCodeModel> BzVerifyCode { get; set; }

//        public virtual DbSet<SysRoleMenuMappingModel> SysRoleMenuMapping { get; set; }
//        public virtual DbSet<SysUserMenuMappingModel> SysUserMenuMapping { get; set; }
//        public virtual DbSet<SysUserRoleMappingModel> SysUserRoleMapping { get; set; }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                //throw new ArgumentException("dbconnection string is not Configured");
//                optionsBuilder.UseMySql("server=localhost;database=BlazuiCommunity;port=3306;uid=root;password=P@ssw0rd123;character set=utf8mb4;");
//            }
//            optionsBuilder.UseLoggerFactory(new CustomEFCoreLoggerFactory());
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {

//            //modelBuilder.Ignore<IdentityUserLogin<string>>();
//            //modelBuilder.Ignore<IdentityUserRole<string>>();
//            //modelBuilder.Ignore<IdentityUserClaim<string>>();
//            //modelBuilder.Ignore<IdentityUserToken<string>>();
//            //modelBuilder.Ignore<IdentityUser<string>>();
//            //modelBuilder.Entity<BZFollowModel>(entity =>
//            //{
//            //    entity.ToTable("follow");

//            //    entity.HasComment("关注帖Id");

//            //    entity.HasIndex(e => e.TopicId)
//            //        .HasName("FK_Reference_7");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_6");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.FollowTime)
//            //        .HasColumnType("datetime")
//            //        .HasComment("关注时间");
//            //    entity.Property(e => e.Status)
//            //        .HasColumnType("int(11)")
//            //        .HasDefaultValueSql("'0'")
//            //        .HasComment("0：正常，-1：已取消关注");

//            //    entity.Property(e => e.TopicId)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("主题帖Id");

//            //    entity.Property(e => e.UserId)
//            //        .HasColumnType("varchar(255)")
//            //        .HasComment("用户Id");

//            //    entity.HasOne(d => d.Topic)
//            //        .WithMany(p => p.Follow)
//            //        .HasForeignKey(d => d.TopicId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("follow_ibfk_2");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Follow)
//            //        .HasForeignKey(d => d.UserId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("follow_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZPointModel>(entity =>
//            //{
//            //    entity.ToTable("point");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_5");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.Access)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("0：签到，1：发帖，2：回帖，3:精华帖，4：其他");

//            //    entity.Property(e => e.Description)
//            //        .HasColumnType("varchar(100)")
//            //        .HasComment("说明")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Score)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("分数");

//            //    entity.Property(e => e.UserId)
//            //      .HasColumnType("varchar(255)")
//            //      .HasComment("用户Id");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Point)
//            //        .HasForeignKey(d => d.UserId)
//            //        .HasConstraintName("point_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZReplyModel>(entity =>
//            //{
//            //    entity.ToTable("reply");

//            //    entity.HasIndex(e => e.TopicId)
//            //        .HasName("FK_Reference_4");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.Content)
//            //        .IsRequired()
//            //        .HasColumnType("text")
//            //        .HasComment("内容")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Favor)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("赞数量");

//            //    entity.Property(e => e.ModifyTime)
//            //        .HasColumnType("datetime")
//            //        .HasComment("修改时间");

//            //    entity.Property(e => e.PublishTime)
//            //        .HasColumnType("datetime")
//            //        .HasComment("发表时间");

//            //    entity.Property(e => e.Status)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("0：正常，-1，已删除");

//            //    entity.Property(e => e.TopicId)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("主题帖Id");

//            //    entity.Property(e => e.UserId)
//            //        .HasColumnType("varchar(255)")
//            //        .HasComment("发帖人Id");

//            //    entity.HasOne(d => d.Topic)
//            //        .WithMany(p => p.Reply)
//            //        .HasForeignKey(d => d.TopicId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("reply_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZThirdAccountModel>(entity =>
//            //{
//            //    entity.ToTable("thirdaccount");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_2");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.HomePage)
//            //        .HasColumnType("varchar(200)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.NickName)
//            //        .HasColumnType("varchar(50)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.OauthId)
//            //        .IsRequired()
//            //        .HasColumnName("OAuthId")
//            //        .HasColumnType("varchar(50)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.OauthType)
//            //        .HasColumnName("OAuthLogin")
//            //        .HasColumnType("int(11)");

//            //    entity.Property(e => e.OauthName)
//            //        .HasColumnName("OAuthName")
//            //        .HasColumnType("varchar(50)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Photo)
//            //        .HasColumnType("varchar(200)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.UserId)
//            //.HasColumnType("varchar(255)")
//            //.HasComment("用户Id");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Thirdaccount)
//            //        .HasForeignKey(d => d.UserId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("thirdaccount_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZTopicModel>(entity =>
//            //{
//            //    entity.ToTable("topic");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_8");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.Content)
//            //        .IsRequired()
//            //        .HasColumnType("text")
//            //        .HasComment("内容")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Good)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("是否精华");

//            //    entity.Property(e => e.Hot)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("人气");

//            //    entity.Property(e => e.ModifyTime)
//            //        .HasColumnType("datetime")
//            //        .HasComment("修改时间");

//            //    entity.Property(e => e.PublishTime)
//            //        .HasColumnType("datetime")
//            //        .HasComment("发表时间");

//            //    entity.Property(e => e.ReplyCount)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("回复帖数量");

//            //    entity.Property(e => e.Status)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("0：正常，-1：已结帖");

//            //    entity.Property(e => e.Title)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(200)")
//            //        .HasComment("标题")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Top)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("是否置顶");

//            //    entity.Property(e => e.TopicType)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("0：提问，1：分享，2：讨论，3：建议，4：公告");

//            //    entity.Property(e => e.UserId)
//            //.HasColumnType("varchar(255)")
//            //.HasComment("用户Id");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Topic)
//            //        .HasForeignKey(d => d.UserId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("topic_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZUserModel>(entity =>
//            //{
//            //    entity.ToTable("user");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.Account)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(20)")
//            //        .HasComment("账号")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Avatar)
//            //        .HasColumnType("varchar(200)")
//            //        .HasComment("头像")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    //entity.Property(e => e.IdentityUser)
//            //    //    .IsRequired()
//            //    //    .HasColumnType("varchar(20)")
//            //    //    .HasComment("identity用户ID")
//            //    //    .HasCharSet("utf8")
//            //    //    .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Email)
//            //        .HasColumnType("varchar(50)")
//            //        .HasComment("邮箱")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.LastLoginAddr)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(100)")
//            //        .HasComment("最后登录地区")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.LastLoginDate)
//            //        .HasColumnType("datetime")
//            //        .HasComment("最后登录时间");

//            //    entity.Property(e => e.LastLoginType)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("最后登录方式");

//            //    entity.Property(e => e.Level)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("等级");

//            //    entity.Property(e => e.Mobile)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(15)")
//            //        .HasComment("手机号")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.NickName)
//            //        .HasColumnType("varchar(30)")
//            //        .HasComment("昵称")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Points)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("积分");

//            //    entity.Property(e => e.RegisterDate)
//            //        .HasColumnType("datetime")
//            //        .HasComment("注册日期");

//            //    entity.Property(e => e.Sex)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("性别");

//            //    entity.Property(e => e.Signature)
//            //        .HasColumnType("varchar(200)")
//            //        .HasComment("签名")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Status)
//            //        .HasColumnType("int(11)")
//            //        .HasComment("0：正常，-1：已删除/冻结");
//            //});

//            //modelBuilder.Entity<BZAddressModel>(entity =>
//            //{
//            //    entity.ToTable("useraddress");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_1");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.City)
//            //        .HasColumnType("varchar(100)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Country)
//            //        .HasColumnType("varchar(50)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.District)
//            //        .HasColumnType("char(10)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.Province)
//            //        .HasColumnType("varchar(100)")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.UserId)
//            //     .HasColumnType("varchar(255)")
//            //     .HasComment("用户Id");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Useraddress)
//            //        .HasForeignKey(d => d.UserId)
//            //        .HasConstraintName("useraddress_ibfk_1");
//            //});

//            //modelBuilder.Entity<BZUserRealVerificationModel>(entity =>
//            //{
//            //    entity.ToTable("userrealverification");

//            //    entity.HasComment("用户实名认证");

//            //    entity.HasIndex(e => e.UserId)
//            //        .HasName("FK_Reference_3");

//            //    entity.Property(e => e.Id).HasColumnType("int(11)");

//            //    entity.Property(e => e.IdentityNo)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(20)")
//            //        .HasComment("身份证")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.PhotoBehind)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(200)")
//            //        .HasComment("身份证背面")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.PhotoFront)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(200)")
//            //        .HasComment("身份证正面")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.Property(e => e.UserId)
//            //.HasColumnType("varchar(255)")
//            //.HasComment("用户Id");

//            //    entity.Property(e => e.UserName)
//            //        .IsRequired()
//            //        .HasColumnType("varchar(50)")
//            //        .HasComment("姓名")
//            //        .HasCharSet("utf8")
//            //        .HasCollation("utf8_general_ci");

//            //    entity.HasOne(d => d.User)
//            //        .WithMany(p => p.Userrealverification)
//            //        .HasForeignKey(d => d.UserId)
//            //        .OnDelete(DeleteBehavior.ClientSetNull)
//            //        .HasConstraintName("userrealverification_ibfk_1");
//            //});
//            base.OnModelCreating(modelBuilder);
//            //OnModelCreatingPartial(modelBuilder);
//        }

//        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
