using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfExampleForToolkit.ViewModels;

namespace WpfExampleForToolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Services = ConfigureServices();
        }
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient(typeof(MainViewModel));
            services.AddTransient(typeof(HomeViewModel));
            services.AddTransient(typeof(CustomerViewModel));

            return services.BuildServiceProvider();
        }
    }
}
