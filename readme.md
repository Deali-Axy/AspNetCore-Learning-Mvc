# Asp.Net Core 学习之MVC

相关项目：[Asp.Net Core学习 WebApi版 图书馆管理系统](https://github.com/Deali-Axy/AspNetCore-Learning-WebApi)

新项目推荐：[使用 AspNetCore 开发博客](https://github.com/Deali-Axy/StarBlog)

公众号 | 公众号 |
------- | ------ | 
![](https://gitee.com/deali/CodeZone/raw/master/images/coding_lab_logo.jpg) | ![](https://gitee.com/deali/CodeZone/raw/master/images/coding_lab_qr_code.jpg)   |

Asp.Net Core 学习笔记系列博客：

- [Asp.Net Core学习笔记：入门篇](https://zhuanlan.zhihu.com/p/105443116)
- [Asp.Net Core学习笔记：（二）视图、模型、持久化、文件、错误处理、日志](https://zhuanlan.zhihu.com/p/105953794)
- [Asp.Net Core学习笔记：（三）使用SignalR实时通信框架开发聊天室](https://zhuanlan.zhihu.com/p/106321863)
- [Asp.Net Core学习笔记：（四）Blazor WebAssembly入门](https://zhuanlan.zhihu.com/p/107262924)
- [Asp.Net Core学习笔记：（五）构建和部署](https://zhuanlan.zhihu.com/p/203298625)
- [Asp-Net-Core开发笔记：接口返回json对象出现套娃递归问题](https://www.cnblogs.com/deali/p/15847475.html)
- [Asp-Net-Core学习笔记：身份认证入门](https://www.cnblogs.com/deali/p/15851620.html)
- [Asp-Net-Core开发笔记：使用NPM和gulp管理前端静态文件](https://www.cnblogs.com/deali/p/15905760.html)

C#语言学习系列博客：

- [C#学习（一）委托的概念和使用](https://zhuanlan.zhihu.com/p/101040936)
- [C#学习（二）匿名方法和委托的多种使用方式](https://zhuanlan.zhihu.com/p/101116276)
- [C#学习（三）深入理解委托、匿名方法和 Lambda 表达式](https://zhuanlan.zhihu.com/p/101178999)

相关博文推荐：

- [跨平台框架AspNetCore开发实践杂谈](https://www.cnblogs.com/deali/p/13929132.html)
- [花一周时间整理的六千字长文！深入思考技术本质，跨平台开发框架AspNetCore的简单实践杂谈](https://zhuanlan.zhihu.com/p/267938409)
- [在.NET Core(C#)中使用EPPlus.Core导出Excel文档](https://zhuanlan.zhihu.com/p/261750807)
- [NetCore爬虫：CatSpider# 开发笔记](https://zhuanlan.zhihu.com/p/106015789)


## 常识

像Django那样自动检查代码更新，自动重载服务器（太方便了）

```
dotnet watch run
```

## 托管设置

设置项目文件的`AspNetCoreHostingModel`属性。

```xml
<PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
</PropertyGroup>
```

- InProcess：使用IIS服务器托管
- OutOfProcess：使用自带Kestrel服务器托管

## 中间件入门

- 可同时被访问和请求
- 可以处理请求后，将请求传递给下一个中间件
- 可以处理请求后，使管道短路
- 可以传出响应
- 中间件是按照添加顺序执行的

通过在`Configure`中添加参数`ILogger<Startup> logger`引入Asp.Net Core自带的日志组件。

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();

    app.Use(async (context, next) =>
    {
        context.Response.ContentType = "text/plain;charset=utf-8";

        //await context.Response.WriteAsync("Hello!");

        logger.LogDebug("M1: 传入请求");
        await next();
        logger.LogDebug("M1: 传出响应");
    });


    app.Use(async (context, next) =>
    {
        context.Response.ContentType = "text/plain;charset=utf-8";

        logger.LogDebug("M2: 传入请求");
        await next();
        logger.LogDebug("M2: 传出响应");
    });

    app.Run(async (context) =>
    {
        //await context.Response.WriteAsync("你好！");
        await context.Response.WriteAsync("M3: 处理请求，生成响应");
        logger.LogDebug("M3: 处理请求，生成响应");
    });
}
```

输出日志：(可以看到三个中间件的执行过程)

```
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request starting HTTP/2.0 GET https://localhost:44383/  
StudyManagement.Startup:Debug: M1: 传入请求
StudyManagement.Startup:Debug: M2: 传入请求
StudyManagement.Startup:Debug: M3: 处理请求，生成响应
StudyManagement.Startup:Debug: M2: 传出响应
StudyManagement.Startup:Debug: M1: 传出响应
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request finished in 52.8954ms 200 text/plain;charset=utf-8
StudyManagement.Startup:Debug: M1: 传入请求
StudyManagement.Startup:Debug: M2: 传入请求
StudyManagement.Startup:Debug: M3: 处理请求，生成响应
StudyManagement.Startup:Debug: M2: 传出响应
StudyManagement.Startup:Debug: M1: 传出响应
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request finished in 34.3387ms 200 text/plain;charset=utf-8
```

## 静态文件支持

所有静态文件都在目录`wwwroot`下

### 首先

```c#
// 设置默认文件
// 不设置的话，默认就是index.html/default.html这几个
var defaultFileOpinions = new DefaultFilesOptions();
defaultFileOpinions.DefaultFileNames.Clear();
defaultFileOpinions.DefaultFileNames.Add("test.html");

// 添加默认文件中间件，必须在UseStaticFiles之前注册
app.UseDefaultFiles(defaultFileOpinions);

// 添加静态文件中间件
app.UseStaticFiles();
```

### DirectoryBrowser 中间件

可以在浏览器浏览 `wwwroot` 下的内容。不推荐在生产环境中使用。

```c#
app.UseDirectoryBrowser();
```

### FileServer 中间件

集成了`UseDefaultFiles`, `UseStaticFiles`, `UseDirectoryBrowser`三个中间件的功能。同样不推荐在生产环境中使用。

```c#
var fileServerOpinions = new FileServerOptions();
fileServerOpinions.DefaultFilesOptions.DefaultFileNames.Clear();
fileServerOpinions.DefaultFilesOptions.DefaultFileNames.Add("test.html");

app.UseFileServer(fileServerOpinions);
```

## 开发者异常页面

```c#
if (env.IsDevelopment())
{
    var developerExceptionPageOptions = new DeveloperExceptionPageOptions();
    // 显示代码行数
    developerExceptionPageOptions.SourceCodeLineCount = 10;
    app.UseDeveloperExceptionPage();
}

app.Run(async (context) =>
{
    throw new Exception("自己抛出的异常");
});
```

## 开发环境变量

- Development：开发环境
- Staging：演示（模拟、临时）环境
- Production：正式（生产）环境

Ops:

- 使用`ASPNETCORE_ENVIRONMENT`环境变量设置开发环境。
- 在开发机上，在`launchSettings.json`文件中设置环境变量。
- 在Staging和Production环境时，尽量在操作系统设置环境变量。
- 使用`IHostEnvironment`服务访问运行时环境
- 除了标准环境之外还支持自定义环境（UAT、QA等）

## 引入MVC框架

### 首先添加MVC服务。

```c#
public void ConfigureServices(IServiceCollection services)
{
    // 单纯引入核心MVC服务，只有核心功能
    services.AddMvcCore();
    // 一般用这个，功能多
    services.AddMvc();
}
```

### 添加中间件

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
{
    if (env.IsDevelopment())
    {
        var developerExceptionPageOptions = new DeveloperExceptionPageOptions();
        // 显示代码行数
        developerExceptionPageOptions.SourceCodeLineCount = 10;
        app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();
    app.UseMvcWithDefaultRoute();
}
```

MVC路由规则：`/控制器名称/方法名称`，（不区分大小写）

例如下面例子的路由是：`/home/index`

### HomeController代码：

```c#
public class HomeController : Controller
{
    public string Index()
    {
        return "home controller";
    }
}
```

## 初步了解模型和依赖注入

### 定义模型

```c#
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ClassName { get; set; }
    public string Email { get; set; }
}
```

### 定义接口

```c#
public interface IStudentRepository
{
    Student GetById(int id);
    void Save(Student student);
}
```

### 实现接口

目前还没接入数据库，定义一个假数据的类

```c#
public class MockStudentRepository : IStudentRepository
{
    private List<Student> _students;

    public MockStudentRepository()
    {
        _students = new List<Student>
        {
            new Student { Id=1, Name="小米", ClassName="红米", Email="hello1@deali.cn" },
            new Student { Id=2, Name="华为", ClassName="荣耀", Email="hello2@deali.cn" },
            new Student { Id=3, Name="oppo", ClassName="vivo", Email="hello3@deali.cn" },
        };
    }

    public Student GetById(int id)
    {
        return _students.FirstOrDefault(a => a.Id == id);
    }

    public void Save(Student student) => throw new NotImplementedException();
}
```

### 注册依赖注入

Asp.Net Core依赖注入容器注册服务有三种

- AddSingleton：全局单例
- AddTransient：每次使用都创建新对象
- AddScoped：每个http请求中创建和使用同一个对象

依赖注入的优点

- 低耦合
- 高测试性，更加方便进行单元测试

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    // 注册依赖注入，将实现类与接口绑定
    services.AddSingleton<IStudentRepository, MockStudentRepository>();
}
```

### 在模型中使用依赖注入

```c#
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;

    // 通过构造函数注入的方式注入 IStudentRepository
    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;

    }

    public JsonResult Index(int id)
    {
        return Json(_studentRepository.GetById(id));
    }
}
```
## 控制器入门

### 内容格式协商

在控制器方法中使用 `ObjectResult` 返回类型，支持内容协商，根据请求头参数返回数据，

```c#
// 支持内容格式协商
public ObjectResult Details(int id)
{
    return new ObjectResult(_studentRepository.GetById(id));
}
```

如：

```
Accept: application/xml
```

将返回xml格式。注：还要添加xml序列化器。

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        // 注册XML序列化器
        .AddXmlSerializerFormatters();
}
```

## 视图入门

### 将数据从控制器传递到视图的方法

前两种都是弱类型的

- ViewData
- ViewBag
- 强类型视图

### ViewData

- 弱类型字典对象
- 使用string类型的键值，存储和chaxun
- 运行时动态解析
- 没有智能感知，编译时也没有类型检查

使用方法：

```c#
ViewData["Title"] = "学生视图";
ViewData["Model"] = model;
```

cshtml代码：

```scss
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>@ViewData["Title"]</h1>
    @{
        var student = ViewData["model"] as StudyManagement.Models.Student;
    }
    <div>姓名：@student.Name</div>
    <div>班级：@student.ClassName</div>
</body>
</html>
```

### ViewBag

```c#
// 直接给动态属性赋值
ViewBag.PageTitle = "ViewBag标题";
ViewBag.Student = model;
```

cshtml使用：

```html
<h1>@ViewBag.PageTitle</h1>
<div>姓名：@ViewBag.Student.Name</div>
<div>班级：@ViewBag.Student.ClassName</div>
```

### 强类型视图

在控制器中传给View()模型

```c#
public IActionResult GetView()
{
    var model = _studentRepository.GetById(1);
    return View(model);
}
```

在cshtml中指定模型类型

```scss
@model StudyManagement.Models.Student
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>强类型模型</h1>
    <ul>
        <li>@Model.Id</li>
        <li>@Model.Name</li>
        <li>@Model.ClassName</li>
        <li>@Model.Email</li>
    </ul>

</body>
</html>
```

## ViewModel 视图模型

类似于DTO（数据传输对象）

### 定义ViewModel

```c#
public class StudentDetailsViewModel
{
    public Student Student { get; set; }
    public string PageTitle { get; set; }
}
```

### 修改控制器

```c#
public IActionResult Details()
{
    var model = _studentRepository.GetById(1);
    var viewModel = new StudentDetailsViewModel
    {
        Student = model,
        PageTitle = "viewmodel里的页面标题"
    };
    return View(viewModel);
}
```

### 在View中使用

```scss
<!-- 这里注册的模型改成了ViewModel了 -->
@model StudyManagement.ViewModels.StudentDetailsViewModel
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>强类型模型</h1>
    <h2>@Model.PageTitle</h2>
    <ul>
        <li>@Model.Student.Id</li>
        <li>@Model.Student.Name</li>
        <li>@Model.Student.ClassName</li>
        <li>@Model.Student.Email</li>
    </ul>
</body>
</html>
```

### View中使用循环

```scss
@model IEnumerable<StudyManagement.Models.Student>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <table border="1">
        <tr>
            <td>Id</td>
            <td>姓名</td>
            <td>班级</td>
            <td>邮箱</td>
        </tr>
        @foreach (var student in Model)
        {
            <tr>
                <td>@student.Id</td>
                <td>@student.Name</td>
                <td>@student.ClassName</td>
                <td>@student.Email</td>
            </tr>
        }
    </table>
</body>
</html>
```

## 布局视图 LayoutView

### 创建布局视图

```scss
<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
</head>
<body>
    <div>
        @RenderBody()
    </div>

    @RenderSection("Scripts", required: false)
</body>
</html>
```

### 渲染视图

```scss
@model IEnumerable<StudyManagement.Models.Student>
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
    ViewBag.Title = "首页 学生列表";
}
<div></div>
```

### 视图节点 Section

在布局视图里渲染节点

```scss
@RenderSection("Scripts", required: false)
```

在普通视图里定义节点

```scss
@section Scripts{ 
    <script>
        document.write("hello");
    </script>
}
```

### 视图开始 ViewStart

我的理解就是`_ViewStart.cshtml`文件所在目录下的每个视图文件开始渲染先执行这个文件的内容。一般直接放在`Views`目录下，全局生效，可以放在各个子文件夹下，这样可以覆盖全局的`_ViewStart.cshtml`。

```scss
@{
    Layout = "_Layout";
}
```

### 视图导入 ViewImports

用来导入命名空间、注册模型等等n多种操作。

生效机制和ViewStart差不多。

## 路由

- 常规路由（传统路由）
- 属性路由

### 常规路由

在`MapRoute`方法中传入就好了。

```c#
// 自定义路由
app.UseMvc(route =>route.MapRoute("default",
	"{controller=Home}/{action=Index}/{id?}"));
```

### 属性路由

比传统路由更加灵活，可以搭配传统路由使用。

即在控制器方法上添加路由注解，一个方法可以同时映射多个路由。

```c#
[Route("Home/Index")]
public IActionResult Index()
{
    return View(_studentRepository.GetAll());
}
```

路由中也可以指定参数

```c#
[Route("test/{id?}")]
public IActionResult Details(int id = 1)
{
    var model = _studentRepository.GetById(id);
    var viewModel = new StudentDetailsViewModel
    {
        Student = model,
        PageTitle = "viewmodel里的页面标题"
    };
    return View(viewModel);
}
```

可以直接在控制器类上加注解，`[controller]/[action]`。

## TagHelper

### 入门

优点：根据参数自动生成，不需要手写超链接，类似Django模板里面的url命令。

在ViewImport中添加TagHelper

```scss
@addTagHelper *,Microsoft.AspNetCore.Mvc.TagHelpers
```

比如，链接TagHelper使用

```html
<a class="btn btn-outline-primary" 
   asp-controller="student" asp-action="get" 
   asp-route-id="@student.Id">
    查看
</a>
```

缓存破坏的TagHelper

```html
<img src="~/images/banner.jpg" alt="Alternate Text" asp-append-version="true" />
```

### 环境 TagHelper

在开发环境中使用本地css文件，在非开发环境下使用的是CDN的css文件。

注：`integrity`是用来做完整性检查的，保证CDN提供文件的完整和安全。

```html
<environment include="Development">
    <link href="~/lib/twitter-bootstrap/css/bootstrap.css" rel="stylesheet" />
</environment>

<environment exclude="Development">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
</environment>
```

为了防止CDN加载失败页面无法显示，可以加上fallback相关属性，第一个是失败时加载的文件，第二个是不检查这个文件的完整性

```scss
asp-fallback-href="~/lib/twitter-bootstrap/css/bootstrap.css"
asp-suppress-fallback-integrity="true"
```

### 表单 Tag Helper

直接贴上一个布局的代码，把class样式都去掉了，保留最基本代码。

确实是很方便的，和Django、jinja2之类的模板比完全不输。

```scss
@model Student

<form asp-controller="student" asp-action="create">
    <label asp-for="Name"></label>
    <input asp-for="Name" />

    <label asp-for="Email"></label>
    <input asp-for="Email" />

    <label asp-for="ClassName"></label>
    <select asp-for="ClassName" asp-items="Html.GetEnumSelectList<ClassNameEnum>()"></select>

    <button type="submit">提交</button>
</form>
```



## 模型绑定

将Http请求中的数据绑定到控制器方法上对应参数的顺序：

- Form Values （Post表单数据）
- Route Values （路由中的值）
- Query String （Get的查询字符串）

### 模型验证

#### 1.设置模型

首先在Model中加入验证属性，如：

```c#
public class Student
{
    public int Id { get; set; }

    [Display(Name = "姓名"), MaxLength(4, ErrorMessage = "名字长度不能超过四个字")]
    [Required(ErrorMessage = "请输入名字！")]
    public string Name { get; set; }

    [Display(Name = "班级")]
    public ClassNameEnum ClassName { get; set; }

    [Required(ErrorMessage = "请输入邮箱！")]
    [Display(Name = "邮箱")]
    [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; }
}
```

##### 常用的模型验证方法

- Required
- Range：指定允许的最小值和最大值
- MinLength
- MaxLength
- Compare：比较两个属性，比如密码和确认密码
- RegularExpression：正则匹配

#### 2.在控制器中加入验证代码

使用`ModelState.IsValid`来验证模型属性是否正确

```c#
[HttpPost]
public IActionResult Create(Student student)
{
    if (ModelState.IsValid)
    {
        var newStudent = _studentRepository.Add(student);
        return RedirectToAction("details", new { id = newStudent.Id });
    }

    return View();
}
```

#### 3.使用TagHelper在网页上显示错误信息

例子如下：

```scss
<div class="text-danger" asp-validation-summary="All"></div>

<div class="form-group row">
<label asp-for="Name" class="col-sm-2 col-form-label"></label>
<div class="col-sm-10">
<input asp-for="Name" class="form-control" />
<span class="text-danger" asp-validation-for="Name"></span>
</div>
</div>
```



## EF Core入门

### 首先实现DbContext

```c#
public class AppDbContext:DbContext
{
    // 将应用程序的配置传递给DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // 对要使用到的每个实体都添加 DbSet<TEntity> 属性
    // 通过DbSet属性来进行增删改查操作
    // 对DbSet采用Linq查询的时候，EFCore自动将其转换为SQL语句
    public DbSet<Student> Students { get; set; }
}
```

### 注册DbContext连接池

```c#
services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));
```

其中，本地SqlServer数据库的配置，在`appserttings.json`中：

```json
"ConnectionStrings": {
    "StudentDBConnection": "server=(localdb)\\MSSQLLocalDB;database=StudentDB;Trusted_Connection=true"
}
```

### 实现仓储

```c#
public class SqlStudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public SqlStudentRepository(AppDbContext context)
    {
        _context = context;
    }
    public Student Add(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();
        return student;
    }
    public Student Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
        }
        return student;
    }
    public IEnumerable<Student> GetAll() => _context.Students;
    public Student GetById(int id) => _context.Students.Find(id);
    public Student Update(Student updatedStudent)
    {
        var student = _context.Students.Attach(updatedStudent);
        student.State = EntityState.Modified;
        _context.SaveChanges();
        return updatedStudent;
    }
}
```

### EF Core 常用命令

在nuget控制台中，不区分大小写

- Get-Help about_enti：显示帮助，`about_enti`全名很长可以只写前面的
- Add-Migration：添加迁移记录
- Update-Database：更新数据库

使用dotnet cli的话请先安装：dotnet tool install --global dotnet-ef   

- dotnet ef migrations add initial: 添加迁移记录
- dotnet ef database update: 更新数据库

### 添加种子数据

重写`DbContext`的`OnModelCreating`方法

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder) { 
    modelBuilder.Entity<Student>().HasData(
        new Student { Id = 1, Name = "小米" },
    );
}
```

为了避免`DbContext`代码太乱，也可以使用扩展方法的方式：

```c#
public static class ModelBuilderExtensions
{
    public static void InsertSeedData(this ModelBuilder mBuilder)
    {
        mBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "小米" },
        );
    }
}
```

### 领域模型与数据库架构

- 使用迁移功能同步领域模型和数据库架构
- 使用 `add-migration` 添加迁移记录
- 使用 `remove-migration` 删除最近一条记录
- 使用 `update-database 迁移记录名称` 可以回滚至任意一次迁移

## 文件上传

### 定义ViewModel

要上传的字段采用 `IFormFile` 类型

```c#
public class StudentCreateViewModel
{
    public int Id { get; set; }
	// 省略无关代码...
    [Display(Name = "图片")]
    public IFormFile Photo { get; set; }
}
```

### 编写视图

修改`cshtml`视图文件，修改模型绑定：

```scss
@model StudentCreateViewModel
```

加入上传文件的表单项

```scss
<div class="form-group row">
    <label asp-for="Photo" class="col-sm-2 col-form-label"></label>
    <div class="col-sm-10">
        <div class="custom-file">
        <input asp-for="Photo" class="form-control custom-file-input" />
        <label class="custom-file-label">请选择图片</label>
        </div>
    </div>
</div>
```

为了选择文件后能显示出文件名还要编写js：

```js
$(document).ready(function () {
    $('.custom-file-input').on('change', function () {
        var fileName = $(this).val().split('\\').pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});
```

### 编写控制器

通过构造函数注入 `HostingEnvironment`

```csharp
public StudentController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment)
{
    _studentRepository = studentRepository;
    _hostingEnvironment = hostingEnvironment;
}
```

处理文件上传和保存的逻辑

```c#
[HttpPost]
public IActionResult Create(StudentCreateViewModel model)
{
    if (ModelState.IsValid)
    {
        var uniqueFileName = "";
        if (model.Photo != null)
        {
            var uploadDir = Path.Combine(
                _hostingEnvironment.WebRootPath, 
                "uploads", "images");
            uniqueFileName = Guid.NewGuid().ToString() + 
                "_" + model.Photo.FileName;
            model.Photo.CopyTo(new FileStream(
                Path.Combine(uploadDir, uniqueFileName), 
                FileMode.Create));
        }
        var student = new Student
        {
            Name = model.Name,
            Email = model.Email,
            ClassName = model.ClassName,
            PhotoPath = uniqueFileName,
        };

        var newStudent = _studentRepository.Add(student);
        return RedirectToAction("details", 
                                new { id = newStudent.Id });
    }

    return View();
}
```

## 多文件上传

和单文件差不多

### ViewModel

增加 `List<IFormFile>` 类型字段

```c#
[Display(Name = "图库")]
public List<IFormFile> Gallery { get; set; }
```

### 修改视图

```scss
<div class="form-group row">
    <label asp-for="Gallery" class="col-sm-2 col-form-label"></label>
    <div class="col-sm-10">
        <div class="custom-file">
            <input asp-for="Gallery" multiple id="gallery-input" class="form-control custom-file-input" />
            <label id="gallery-label" class="custom-file-label">请选择图片 可以一次选择多张</label>
        </div>
    </div>
</div>
```

js代码：

```js
$('#gallery-input').on('change', function () {
    var label = $(this).next('#gallery-label');
    var files = $(this)[0].files;
    if (files.length > 1) {
        label.html(`你已经选择了${files.length}个文件`);
    } else if (files.length == 1) {
        label.html(files[0].name);
    }
});
```

### 修改控制器代码

```c#
// 处理多文件上传
if (model.Gallery != null && model.Gallery.Count > 0)
{
    foreach(var photo in model.Gallery)
    {
        uniqueFileName = Guid.NewGuid().ToString() + "_" 
            + photo.FileName;
        photo.CopyTo(new FileStream(Path.Combine(uploadDir, 
        uniqueFileName),FileMode.Create));
    }
}
```

## 错误处理

### 添加错误处理页面

在 `Startup.cs` 中设置中间件：

```c#
app.UseStatusCodePagesWithReExecute("/error/{0}");
```

推荐用 `UseStatusCodePagesWithReExecute` 而不是 `UseStatusCodePagesWithRedirects`，前者在管道内执行执行错误跳转url，后者会重定向到该url，导致http错误状态码变成新页面的正常执行的200码了。

接着编写错误控制器：

```c#
public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode)
    {
        var statusCodeResult = HttpContext.Features.
            Get<IStatusCodeReExecuteFeature>();
        var viewModel = new ErrorViewModel
        {
            Path = statusCodeResult.OriginalPath,
            QueryString = statusCodeResult.
                OriginalQueryString,
        };
        switch (statusCode)
        {
            case 404:
                viewModel.Message = "页面未找到";
                break;
        }
        return View("Error", viewModel);
    }
}
```

对了，我还定义了ViewModel：

```c#
public class ErrorViewModel
{
    public int Code { get; set; }
    public string Message { get; set; }
    public string Path { get; set; }
    public string QueryString { get; set; }
}
```

视图代码就不贴了，无非就是显示ViewModel里的这些错误信息~

### 设置全局异常跳转

添加中间件

```c#
app.UseExceptionHandler("/exception");
```

编写处理用的控制器，这里需要添加`AllowAnonymous`注解，允许用户在未登录的时候访问到这个异常页面，保证无论如何可以显示出异常页面。

```c#
[AllowAnonymous]
[Route("exception")]
public IActionResult ExceptionHandler()
{
    var exception = HttpContext.Features.
        Get<IExceptionHandlerPathFeature>();
    var viewModel = new ExceptionViewModel
    {
        Path = exception.Path,
        Message = exception.Error.Message,
        StackTrace = exception.Error.StackTrace,
    };
    return View("Exception", viewModel);
}
```

另外，ViewModel定义如下：

```c#
public class ExceptionViewModel
{
    public string Path { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
}
```

## 日志记录

AspNetCore里面自带了一套日志系统，默认已经注册到了服务容器里了，只要在控制器的构造函数里注入就可以使用了，比如：

```c#
public class ErrorController : Controller
{
    private ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        this._logger = logger;
    }
}
```

默认的日志只会记录到控制台或者调试输出，不过我们为了实现更多功能，比如记录到文件或者推送到日志服务器，我们需要使用第三方的日志组件。这里我用的是NLog。

首先要安装`NLog.Web.AspNetCore`这个包。

之后在`Program.cs`里引入nlog服务：

```c#
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
    .ConfigureLogging((hostingContext, logging) =>
     {
         // 保留官方的代码中的默认日志程序
 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
         logging.AddConsole();
         logging.AddDebug();
         logging.AddEventSourceLogger();
         // 引入 nlog
         logging.AddNLog();
      }).UseStartup<Startup>();
```

保留官方默认日志程序那里，要看AspNetCore的源代码，本文用的是2.2版本，在github看，地址如下：

 https://github.com/dotnet/aspnetcore/blob/v2.2.8/src/DefaultBuilder/src/WebHost.cs 

然后，为了使用nlog，需要创建一个配置文件，在项目根目录创建 `NLog.config`：

关于配置文件的说明可以参考：https://github.com/NLog/NLog/wiki

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      throwConfigExceptions="true">
  <targets>
    <target name="f1" xsi:type="File" fileName="Logs\nlog-all-${shortdate}.log"/>
    <target name="n1" xsi:type="Network" address="tcp://localhost:4001"/>
    <target name="c1" xsi:type="Console" encoding="utf-8"
            error="true"
            detectConsoleAvailable="true" />
    <target name="c2" xsi:type="ColoredConsole" encoding="utf-8"
          useDefaultRowHighlightingRules="true"
          errorStream="true"
          enableAnsiOutput="true"
          detectConsoleAvailable="true"
          DetectOutputRedirected="true">
    </target>
  </targets>
  <rules>
    <logger name="*" maxLevel="Debug" writeTo="c2" />
    <logger name="*" minLevel="Info" writeTo="f1" />
  </rules>
</nlog>
```

之后在程序中就可以正常使用日志功能了。比如：

```c#
var viewModel = new StatusCodeViewModel
{
    Code = statusCode,
    Path = statusCodeResult.OriginalPath,
    QueryString = statusCodeResult.OriginalQueryString,
};
_logger.LogWarning(viewModel.ToString());
```

还有可以在`appsettings.json`里面配置日志等级和命名空间过滤，跟在`NLog.conf`里面配置效果是一样的。例如：

```json
"Logging": {
    "LogLevel": {
        "Default": "Warning",
        "StudyManagement.Controllers.ErrorController": 
        "Warning"
    }
},
```

## 构建与部署

发布：

>参考：https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-publish

```bash
dotnet publish
```

构建不依赖DotNet Sdk的可执行项目：

```bash
dotnet publish -r <RID>
```

>其中，`RID`参考：https://docs.microsoft.com/zh-cn/dotnet/core/rid-catalog

附上几个常用的吧：
- Windows
    - win-x86
    - win-x64
    - win-arm
    - win-arm64
- Linux
    - linux-x64
    - linux-arm
    - linux-arm64

更多选项
```bash
dotnet publish -p:PublishSingleFile=true -p:PublishTrimmed=true 
```

