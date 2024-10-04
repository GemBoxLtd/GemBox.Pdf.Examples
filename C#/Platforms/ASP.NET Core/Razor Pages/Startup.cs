using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PdfCorePages;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        // Add compatibility switch.
        AppContext.SetSwitch("Switch.System.Windows.Media.ShouldRenderEvenWhenNoDisplayDevicesAreAvailable", true);
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) => services.AddRazorPages();

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        _ = env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseExceptionHandler("/Error");

        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.UseAuthorization();

        _ = app.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }
}
