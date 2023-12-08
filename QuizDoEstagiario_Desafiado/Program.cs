using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            // Adicione aqui qualquer inicialização que você precise antes de executar o host (favor manter organizado)
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configurei os serviços aqui, pode ser adicionado outros, seguir meu padrão para sicar organizado
        services.AddControllersWithViews();
        services.AddDistributedMemoryCache(); // Adicionei um serviço de cache distribuído em memória
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Defini o tempo limite da sessão para 30 minutos pode mudar se for necessário
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSession(); // Adicionei o middleware de sessão

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "result",
                pattern: "result",
                defaults: new { controller = "Home", action = "Result" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
