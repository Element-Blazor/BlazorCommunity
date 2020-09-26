using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorCommunity.AppDbContext.Migrations
{
    public partial class initDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BZAddress",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Country = table.Column<string>(maxLength: 20, nullable: true),
                    Province = table.Column<string>(maxLength: 20, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    District = table.Column<string>(maxLength: 20, nullable: true),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZAutho",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    OauthType = table.Column<int>(nullable: false),
                    OauthName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(maxLength: 100, nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: true),
                    HomePage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZAutho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZBanner",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Title = table.Column<string>(nullable: true),
                    BannerImg = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Show = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZBanner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZFollow",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    TopicId = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZFollow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZPoint",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Access = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZPoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZReply",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Content = table.Column<string>(nullable: true),
                    TopicId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Favor = table.Column<int>(nullable: true),
                    Top = table.Column<int>(nullable: false),
                    Good = table.Column<int>(nullable: false),
                    ReplyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZReply", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZTopic",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Hot = table.Column<int>(nullable: false),
                    Top = table.Column<int>(nullable: false),
                    Good = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    ReplyCount = table.Column<int>(nullable: false),
                    VersionId = table.Column<string>(type: "varchar(36)", nullable: true),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZTopic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    NickName = table.Column<string>(maxLength: 50, nullable: true),
                    Avator = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Signature = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastLoginType = table.Column<int>(nullable: false),
                    LastLoginAddr = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    QQ = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZVerify",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    VerifyCode = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: true),
                    VerifyType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZVerify", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZVersion",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    LastModifyDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Status = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LastModifierId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Project = table.Column<int>(nullable: false),
                    VerNo = table.Column<string>(nullable: true),
                    VerName = table.Column<string>(nullable: true),
                    VerDescription = table.Column<string>(nullable: true),
                    VerNuget = table.Column<string>(nullable: true),
                    VerDownUrl = table.Column<string>(nullable: true),
                    VerDocUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BZRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BZRoleClaim_BZRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BZRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BZUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BZUserClaim_BZUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BZUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BZUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_BZUserLogin_BZUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BZUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BZUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_BZUserRole_BZRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BZRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BZUserRole_BZUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BZUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BZUserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BZUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_BZUserToken_BZUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BZUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "BZRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BZRoleClaim_RoleId",
                table: "BZRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "BZUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "BZUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BZUserClaim_UserId",
                table: "BZUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BZUserLogin_UserId",
                table: "BZUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BZUserRole_RoleId",
                table: "BZUserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BZAddress");

            migrationBuilder.DropTable(
                name: "BZAutho");

            migrationBuilder.DropTable(
                name: "BZBanner");

            migrationBuilder.DropTable(
                name: "BZFollow");

            migrationBuilder.DropTable(
                name: "BZPoint");

            migrationBuilder.DropTable(
                name: "BZReply");

            migrationBuilder.DropTable(
                name: "BZRoleClaim");

            migrationBuilder.DropTable(
                name: "BZTopic");

            migrationBuilder.DropTable(
                name: "BZUserClaim");

            migrationBuilder.DropTable(
                name: "BZUserLogin");

            migrationBuilder.DropTable(
                name: "BZUserRole");

            migrationBuilder.DropTable(
                name: "BZUserToken");

            migrationBuilder.DropTable(
                name: "BZVerify");

            migrationBuilder.DropTable(
                name: "BZVersion");

            migrationBuilder.DropTable(
                name: "BZRole");

            migrationBuilder.DropTable(
                name: "BZUser");
        }
    }
}
