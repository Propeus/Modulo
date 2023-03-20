using System.Runtime.CompilerServices;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        var core = Propeus.Modulo.Core.Gerenciador.Atual;
        var dinamico = Propeus.Modulo.Dinamico.Gerenciador.Atual(core);

        builder.Host.ConfigureGerenciador(dinamico);
        
        // Add services to the container.
        builder.Services.AddControllersWithViews();

        WebApplication? app = builder.Build();


        // ConfigureGerenciador the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    
}