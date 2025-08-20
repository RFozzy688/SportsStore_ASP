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
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MsSql")),
                ServiceLifetime.Singleton);

            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "pagination",
                pattern: "Products/Page{productPage}",
                defaults: new { Controller = "Product", action = "List" }
            );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=List}/{id?}"
            );

            SeedData.EnsurePopulated(app);

            app.Run();
        }
    }
}
