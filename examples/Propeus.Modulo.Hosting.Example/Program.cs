namespace Propeus.Modulo.Hosting.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Host.ConfigureGerenciador(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Modulo.Core.ModuleManagerCoreExtensions.CreateModuleManagerDefault()));

            WebApplication app = builder.Build();

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