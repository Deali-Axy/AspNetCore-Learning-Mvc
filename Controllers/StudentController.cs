using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Models;
using StudyManagement.ViewModels;

namespace StudyManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment _hostingEnvironment;

        // 通过构造函数注入的方式注入 IStudentRepository, HostingEnvironment     
        public StudentController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(_studentRepository.GetAll());
        }

        // 支持内容格式协商
        public ObjectResult Get(int id)
        {
            return new ObjectResult(_studentRepository.GetById(id));
        }

        [Route("test/{id?}")]
        public IActionResult Details(int id = 1)
        {
            var model = _studentRepository.GetById(id);
            if (model == null) return View("StudentNotFound");

            ViewData["Title"] = "学生视图";
            ViewData["Model"] = model;

            // 直接给动态属性赋值
            ViewBag.PageTitle = "ViewBag标题";
            ViewBag.Student = model;

            var viewModel = new StudentDetailsViewModel
            {
                Student = model,
                PageTitle = "viewmodel里的页面标题"
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    uniqueFileName = ProcessUpload(model.Photo);
                }
                var student = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName,
                };

                // 处理多文件上传
                if (model.Gallery != null && model.Gallery.Count > 0)
                {
                    foreach (var photo in model.Gallery)
                    {
                        ProcessUpload(photo);
                    }
                }

                var newStudent = _studentRepository.Add(student);
                return RedirectToAction("details", new { id = newStudent.Id });
            }

            return View();
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            var model = _studentRepository.GetById(id);
            if (model == null) return View("StudentNotFound");

            var viewModel = new StudentEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                ClassName = model.ClassName,
                Email = model.Email,
                ExistedPhotoPath = model.PhotoPath
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _studentRepository.GetById(viewModel.Id);
                if (model == null) return View("StudentNotFound");

                model.Name = viewModel.Name;
                model.Email = viewModel.Email;
                model.ClassName = viewModel.ClassName;
                // 删除旧的图片
                if (model.PhotoPath != null && viewModel.ExistedPhotoPath != null)
                {
                    var existedPhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "images", model.PhotoPath);
                    if (System.IO.File.Exists(existedPhotoPath)) System.IO.File.Delete(existedPhotoPath);
                }
                model.PhotoPath = ProcessUpload(viewModel.Photo);

                var newModel = _studentRepository.Update(model);
                return RedirectToAction("details", new { id = newModel.Id });
            }

            return View(viewModel);
        }

        private string ProcessUpload(IFormFile file)
        {
            var uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            using (var stream = new FileStream(Path.Combine(uploadDir, uniqueFileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return uniqueFileName;
        }
    }
}