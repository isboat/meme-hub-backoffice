using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Azure.SignalR.Management;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Partners.Management.Web.Services;
using Meme.Domain.Models;

namespace Partners.Management.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            builder.Services.Configure<AuthSettings>(
                builder.Configuration.GetSection("AuthSettings"));

            builder.Services.Configure<S3Settings>(
                builder.Configuration.GetSection("S3Settings"));


            ConfigureServices(builder);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            RegisterAuth(builder, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRepository<MemePageModel>, MemePageRepository>();
            builder.Services.AddSingleton<IRepository<UserModel>, UserRepository>();

            builder.Services.AddSingleton<IMemePageService, MemePageService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddSingleton<IUploadService, S3UploadService>();

            builder.Services.AddScoped<ISignalrService>(provider =>
            {
                var serviceBusConnectionString = builder.Configuration.GetValue<string>(SignalRConstants.AzureSignalRConnectionStringName);
                return new SignalrService(serviceBusConnectionString!, ServiceTransportType.Transient);
            });

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
        }

        private static void RegisterAuth(WebApplicationBuilder builder, ConfigurationManager configuration)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(TenantAuthorization.RequiredPolicy, policy =>
                    policy.RequireAuthenticatedUser().RequireClaim("scope", TenantAuthorization.RequiredScope));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.Cookie.Name = "MemeToken.Hub.AspNetCore.Cookies";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;

                });
        }
    }
}