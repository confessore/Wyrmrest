using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using Wyrmrest.Web.Data;
using Wyrmrest.Web.Services;
using Wyrmrest.Web.Statics;

namespace Wyrmrest.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            IntializeEnvironmentVariables();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Strings.DotNetConnectionString));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        void IntializeEnvironmentVariables()
        {
            EnvironmentVariables.DatabaseHost = Environment.GetEnvironmentVariable("DatabaseHost");
            EnvironmentVariables.DatabasePort = Environment.GetEnvironmentVariable("DatabasePort");
            EnvironmentVariables.DatabaseUser = Environment.GetEnvironmentVariable("DatabaseUser");
            EnvironmentVariables.DatabasePass = Environment.GetEnvironmentVariable("DatabasePass");
            EnvironmentVariables.SmtpHost = Environment.GetEnvironmentVariable("SmtpHost");
            EnvironmentVariables.SmtpPort = Environment.GetEnvironmentVariable("SmtpPort");
            EnvironmentVariables.SmtpName = Environment.GetEnvironmentVariable("SmtpName");
            EnvironmentVariables.SmtpEmail = Environment.GetEnvironmentVariable("SmtpEmail");
            EnvironmentVariables.SmtpUser = Environment.GetEnvironmentVariable("SmtpUser");
            EnvironmentVariables.SmtpPass = Environment.GetEnvironmentVariable("SmtpPass");
            Strings.DotNetConnectionString = BuildDotNetConnectionString();
            Strings.AuthConnectionString = BuildAuthConnectionString();
        }

        string BuildDotNetConnectionString()
        {
            var builder = new StringBuilder();
            builder.Append($"server={EnvironmentVariables.DatabaseHost};");
            builder.Append($"port={EnvironmentVariables.DatabasePort};");
            builder.Append($"uid={EnvironmentVariables.DatabaseUser};");
            builder.Append($"pwd={EnvironmentVariables.DatabasePass};");
            builder.Append($"database=dotnet;");
            return builder.ToString();
        }

        string BuildAuthConnectionString()
        {
            var builder = new StringBuilder();
            builder.Append($"server={EnvironmentVariables.DatabaseHost};");
            builder.Append($"port={EnvironmentVariables.DatabasePort};");
            builder.Append($"uid={EnvironmentVariables.DatabaseUser};");
            builder.Append($"pwd={EnvironmentVariables.DatabasePass};");
            builder.Append($"database=auth;");
            return builder.ToString();
        }
    }
}
