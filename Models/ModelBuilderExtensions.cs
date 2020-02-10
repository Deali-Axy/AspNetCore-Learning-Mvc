using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void InsertSeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "小米", ClassName = ClassNameEnum.FirstGrade, Email = "hello1@deali.cn" },
                new Student { Id = 2, Name = "华为", ClassName = ClassNameEnum.SecondGrade, Email = "hello2@deali.cn" },
                new Student { Id = 3, Name = "oppo", ClassName = ClassNameEnum.ThirdGrade, Email = "hello3@deali.cn" }
            );
        }
    }
}
