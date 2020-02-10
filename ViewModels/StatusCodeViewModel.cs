using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.ViewModels
{
    public class StatusCodeViewModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }

        public override string ToString() => $"请求路径:{Path} Http响应码:{Code} 错误信息:{Message} 请求字符串:{QueryString}";
    }
}
