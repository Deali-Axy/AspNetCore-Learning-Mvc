using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyManagement.Models;

namespace StudyManagement.ViewModels
{
    public class StudentCreateViewModel
    {
        [Display(Name = "姓名"), MaxLength(4, ErrorMessage = "名字长度不能超过四个字")]
        [Required(ErrorMessage = "请输入名字！")]
        public string Name { get; set; }

        [Display(Name = "班级")]
        [Required(ErrorMessage = "请选择班级")]
        public ClassNameEnum? ClassName { get; set; }

        [Required(ErrorMessage = "请输入邮箱！")]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "图片")]
        public IFormFile Photo { get; set; }

        [Display(Name = "图库")]
        public List<IFormFile> Gallery { get; set; }
    }
}
