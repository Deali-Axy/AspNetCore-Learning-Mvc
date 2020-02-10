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

namespace StudyManagement
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configurationProvider)
        {
            _configuration = configurationProvider;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));
            // 注册XML序列化器
            services.AddMvc().AddXmlSerializerFormatters();
            // 注册依赖注入，将实现类与接口绑定
            services.AddScoped<IStudentRepository, SqlStudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                var developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                // 显示代码行数
                developerExceptionPageOptions.SourceCodeLineCount = 10;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/status-code/{0}");
                app.UseExceptionHandler("/exception");
            }

            app.UseStaticFiles();
            // 使用默认路由
            //app.UseMvcWithDefaultRoute();

            // 自定义路由
            app.UseMvc(route => route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
