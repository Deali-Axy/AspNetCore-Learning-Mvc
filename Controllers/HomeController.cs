using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Models;

namespace StudyManagement.Controllers
{
    public class HomeController : Controller
    {

        public string Index()
        {
            return "home controller";
        }

        public JsonResult GetJson()
        {
            return Json(new { id = 1, name = "你好" });
        }
    }
}