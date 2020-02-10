using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.ViewModels
{
    public class ExceptionViewModel
    {
        public string Path { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override string ToString() => $"请求路径:{Path} 错误信息:{Message} 调用堆栈:{StackTrace}";
    }
}
