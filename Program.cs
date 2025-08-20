using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            //builder.Services.AddTransient<IProductRepository, FakeProductRepository>();
            builder.Services.AddTransient<IProductRepository, EFProductRepository>();
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MsSql")),
                ServiceLifetime.Singleton);


            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
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

            app.Run();
        }
    }
}
