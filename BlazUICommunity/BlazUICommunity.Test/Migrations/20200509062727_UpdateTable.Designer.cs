﻿// <auto-generated />
using System;
using Blazui.Community.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blazui.Community.Test.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20200509062727_UpdateTable")]
    partial class UpdateTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Blazui.Community.Model.Models.BZAddressModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("City")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<string>("Country")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("District")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("Province")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZAddress");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZAutho2Model", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("HomePage")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("NickName")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("OauthName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OauthType")
                        .HasColumnType("int");

                    b.Property<string>("Photo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZAutho");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZFollowModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("TopicId")
                        .HasColumnName("TopicId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZFollow");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZPointModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<int?>("Access")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZPoint");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZReplyModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<int?>("Favor")
                        .HasColumnType("int");

                    b.Property<int>("Good")
                        .HasColumnType("int");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("ReplyId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<int>("Top")
                        .HasColumnType("int");

                    b.Property<string>("TopicId")
                        .HasColumnName("TopicId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZReply");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZTopicModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<int>("Good")
                        .HasColumnType("int");

                    b.Property<int>("Hot")
                        .HasColumnType("int");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<string>("RoleId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<int>("Top")
                        .HasColumnType("int");

                    b.Property<string>("VersionId")
                        .HasColumnName("VersionId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("BZTopic");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZUserModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avator")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastLoginAddr")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnName("LastLoginDate")
                        .HasColumnType("timestamp");

                    b.Property<int>("LastLoginType")
                        .HasColumnType("int");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NickName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QQ")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("Signature")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("BZUser");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BZVersionModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<int>("Project")
                        .HasColumnType("int");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("VerDescription")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VerDocUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VerDownUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VerName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VerNo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VerNuget")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("BZVersion");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BzBannerModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("BannerImg")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("Link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Show")
                        .HasColumnType("tinyint(1)");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("BZBanner");
                });

            modelBuilder.Entity("Blazui.Community.Model.Models.BzVerifyCodeModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("CreateDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("CreatorId")
                        .HasColumnName("CreatorId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("ExpireTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastModifierId")
                        .HasColumnName("LastModifierId")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("LastModifyDate")
                        .HasColumnName("LastModifyDate")
                        .HasColumnType("timestamp");

                    b.Property<sbyte>("Status")
                        .HasColumnName("Status")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId")
                        .HasColumnName("UserId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("VerifyCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("VerifyType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BZVerify");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("BZRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("BZRoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BZUserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("BZUserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("BZUserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("BZUserToken");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Blazui.Community.Model.Models.BZUserModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Blazui.Community.Model.Models.BZUserModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blazui.Community.Model.Models.BZUserModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Blazui.Community.Model.Models.BZUserModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
