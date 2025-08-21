using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SportsStore
{
    public class Program
    {
        public static async Task Main( string[] args )
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            //builder.Services.AddTransient<IProductRepository, FakeProductRepository>();
            builder.Services.AddTransient<IProductRepository, EFProductRepository>();
            builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MsSql")),
                ServiceLifetime.Singleton);
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration["SportStoreIdentity:Identity"]),
                ServiceLifetime.Singleton);
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();


            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.MapControllerRoute(
                name: null,
                pattern: "{category}/Page{productPage:int}", 
                defaults: new { controller = "Product", action = "List" }
            );
            app.MapControllerRoute(
                name: null,
                pattern: "Page{productPage:int}",
                defaults: new { controller = "Product", action = "List", productPage = 1}
            );
            app.MapControllerRoute(
                name: null,
                pattern: "{category}",
                defaults: new { controller = "Product", action = "List", productPage = 1 }
            );
            app.MapControllerRoute(
                name: null,
                pattern: "",
                defaults: new { controller = "Product", action = "List", productPage = 1 }
            );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}"
            );

            SeedData.EnsurePopulated(app);
            //await IdentitySeedData.EnsurePopulated(app);

            app.Run();
        }
    }
}
