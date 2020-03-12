using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiObrasBibliograficas.Data;
using ApiObrasBibliograficas.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiObrasBibliograficas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
      
            // Add framework services.
            services.AddCors(opções =>
            {
                opções.AddPolicy("AllowMyOrigin",
                p => p.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<ApiContext>(context => { context.UseInMemoryDatabase("DBBibliografia"); }) ;
            services.AddTransient<ApiContext>();
            services.AddTransient<AuthorServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApiContext>();

                AdicionarDadosTeste(context);
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowMyOrigin");
            app.UseMvc();


        }

        private static void AdicionarDadosTeste(ApiContext context)
        {
            var testeUsuario1 = new Models.Author
            {
                Id = 1,
                Name = "Eduardo Ribeiro Fernandes",
            };
            context.Authors.Add(testeUsuario1);
     
            context.SaveChanges();
        }
    }
}
