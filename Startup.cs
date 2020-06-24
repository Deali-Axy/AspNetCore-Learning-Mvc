using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StudyManagement.Models;

namespace StudyManagement {
    public class Startup {
        private IConfiguration _configuration;

        public Startup(IConfiguration configurationProvider) {
            _configuration = configurationProvider;
        }

        public void ConfigureServices(IServiceCollection services) {
            // 添加数据库连接池
            services.AddDbContextPool<AppDbContext>(builder =>
                builder.UseSqlite(_configuration.GetConnectionString("SQLite")));
            // services.AddDbContextPool<AppDbContext>(builder =>
            //     builder.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));
            // 注册XML序列化器
            services.AddMvc().AddXmlSerializerFormatters().AddMvcOptions(
                // 关闭EndpointRouting功能，因为我要用传统的MVC+Routing
                options => options.EnableEndpointRouting = false
                );
            // 注册依赖注入，将实现类与接口绑定
            services.AddScoped<IStudentRepository, SqlStudentRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger) {
            if (env.IsDevelopment()) {
                var developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                // 显示代码行数
                developerExceptionPageOptions.SourceCodeLineCount = 10;
                app.UseDeveloperExceptionPage();
            } else {
                app.UseStatusCodePagesWithReExecute("/status-code/{0}");
                app.UseExceptionHandler("/exception");
            }

            app.UseStaticFiles();
            // 使用默认路由
            app.UseMvcWithDefaultRoute();

            // 自定义路由
            // app.UseMvc(route => route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}