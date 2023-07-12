using Propeus.Modulo.Hosting;

namespace Propeus.Modulo.Hosting.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();
            
            builder.Host.ConfigureGerenciador(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Modulo.Core.ModuleManagerCoreExtensions.CreateModuleManagerDefault()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}