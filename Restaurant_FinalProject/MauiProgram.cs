using Microsoft.Extensions.Logging;
using Restaurant_FinalProject.Database;
using Microsoft.EntityFrameworkCore;
using Restaurant_FinalProject.Services;

namespace Restaurant_FinalProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddDbContext<RestaurantDbContext>(options =>
            {
                // The OnConfiguring method in RestaurantDbContext will handle this
            });
            // Register all services
            builder.Services.AddScoped<DatabaseService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<MenuItemService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<TableService>();
            builder.Services.AddScoped<InventoryService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<TimesheetService>();

            // Register database service
            builder.Services.AddSingleton<DatabaseService>();
            
            return builder.Build();
        }
    }
}
