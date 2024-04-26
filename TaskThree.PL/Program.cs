using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskThree.DA.Data;
using TaskThree.PL.Extensions;
using TaskThree.PL.Helpers;
using TaskThree.PL.Models;
using TaskThree.PL.Services.EmailSender;

namespace TaskThree.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);
			#region Configure services
			webApplicationBuilder.Services.AddControllersWithViews();
			webApplicationBuilder.Services.AddDbContext<ApplicationDBContext>
				(
				options =>
				{
					options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
				}
				, ServiceLifetime.Scoped);
			webApplicationBuilder.Services.AppApplicationServices();
			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 5;
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.User.RequireUniqueEmail = true;

			}).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

			webApplicationBuilder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";
			});

			//webApplicationBuilder.Services.AddAuthentication("Hamda");
			webApplicationBuilder.Services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = "Identity.Application";
			}).AddCookie("Hamda", options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";
			});
			webApplicationBuilder.Services.AddTransient<Services.EmailSender.IEmailSender, EmailSender>();
			#endregion
			var app = webApplicationBuilder.Build();
			#region Configure Kestrel Middlewares
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
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
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			#endregion
			app.Run();
		}
    }
}
