using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyManagement.Models
{
    public class AppDbContext : DbContext
    {
        // 将应用程序的配置传递给DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 对要使用到的每个实体都添加 DbSet<TEntity> 属性
        // 通过DbSet属性来进行增删改查操作
        // 对DbSet采用Linq查询的时候，EFCore自动将其转换为SQL语句
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.InsertSeedData();
        }
    }
}
