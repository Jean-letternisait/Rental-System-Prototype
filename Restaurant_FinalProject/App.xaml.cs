using Restaurant_FinalProject.Services;
using Microsoft.Extensions.Logging;

namespace Restaurant_FinalProject
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<App> _logger;
        public App(DatabaseService databaseService, ILogger<App> logger)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _logger = logger;

            // Set the main page
            MainPage = new MainPage();

            // Initialize database on app start
            Task.Run(async () => await InitializeDatabase());

        }
        private async Task InitializeDatabase()
        {
            try
            {
                await _databaseService.InitializeAsync();
                _logger.LogInformation("Database initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize database");
                // You might want to show an error message to the user here
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Restaurant_FinalProject" };
        }
    }
}
