using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Test
{
    public class DemoDbContext:  IdentityDbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {
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
        }
    }


}
