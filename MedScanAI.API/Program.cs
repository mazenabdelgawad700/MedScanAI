using MedScanAI.Core;
using MedScanAI.Infrastructure;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services
                    .AddInfrastructureDependencies()
                    .AddServiceDependencies()
                    .AddCoreDependencies()
                    .AddServiceRegisteration(builder.Configuration);


            #region AllowCors
            string CORS = "_cors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS, policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            #endregion


            builder.Services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("SlidingWindowPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(1);
                    opt.PermitLimit = 1000;
                    opt.QueueLimit = 1000;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    opt.SegmentsPerWindow = 50;
                }).RejectionStatusCode = 429;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRateLimiter();

            app.UseHttpsRedirection();

            app.UseCors(CORS);

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.Roles.Any())
                {
                    string[] roleNames = ["Admin", "Patient", "Doctor"];
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }
                }
                scope.Dispose();
            }

            app.Run();
        }
    }
}