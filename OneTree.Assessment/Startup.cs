
#region Usings

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneTree.Assessment.API.Filters;
using OneTree.Assessment.Core.IService;
using OneTree.Assessment.Core.Service;
using OneTree.Assessment.Domain;
using OneTree.Assessment.Domain.IRepositories;
using OneTree.Assessment.Domain.Repositories;
using System;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace OneTree.Assessment.API
{
    public class Startup
    {

        /// <summary>
        /// configuration's instace
        /// </summary>
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.AddControllers(mvcOpts =>
            {
                mvcOpts.Filters.Add(typeof(AppExceptionFilterAttribute));
            });

            services.AddCors();

            services.AddAutoMapper(Assembly.Load("OneTree.Assessment.Core"));

            services.AddOpenApiDocument(document =>
            {
                document.Title = "Technical assessment";
                document.Description = "One tree - Esteban Beckett";
            });


            //dbo
            services.AddDbContext<OneTreeAssessmentContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("StringConnection"),
                    x => x.MigrationsHistoryTable("__MicroMigrationHistory", Configuration.GetConnectionString("SchemaName")));
            }, ServiceLifetime.Transient);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            //Register DataBaseContext
            builder.RegisterType<OneTreeAssessmentContext>().As<IQueryableUnitOfWork>();

            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<BlobStorageRepository>().As<IBlobStorageRepository>();
            builder.RegisterType<ProductService>().As<IProductService>();


            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Clear();
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "master-only");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("Cache-Control", "no-cache,no-store,must-revalidate");
                context.Response.Headers.Add("Pragma", "no-cache");
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true));

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
