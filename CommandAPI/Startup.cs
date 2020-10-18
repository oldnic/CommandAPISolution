using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Newtonsoft.Json.Serialization;
using Microsoft.Data.SqlClient;
using AutoMapper;

namespace CommandAPI {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            builder.UserID = Configuration["UserID"];
            builder.Password = Configuration["Password"];

            //services.AddScoped<ICommandAPIRepo, MockCommandAPIRepo>();
            //services.AddDbContext<CommandContext>(opt => opt.UseSqlServer (Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<CommandContext>(opt => opt.UseSqlServer(builder.ConnectionString));

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommandContext context) {

            context.Database.Migrate();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                //endpoints.MapGet("/", async context => {
                //await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllers();
            });
        }
    }
}
