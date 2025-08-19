using cpsy200FinalProject.Services;
using cpsy200FinalProject.Interfaces;
using Microsoft.Extensions.Logging;
using cpsy200FinalProject.ViewModel;

namespace cpsy200FinalProject
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
            builder.Services.AddSingleton<IEquipmentService, EquipmentService>();
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<IRentalService, RentalService>();
            builder.Services.AddTransient<EquipmentViewModel>();
            builder.Services.AddTransient<CustomerViewModel>();
            builder.Services.AddTransient<RentalViewModel>();
            return builder.Build();
        }
    }
}
