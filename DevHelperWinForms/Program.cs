using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevHelperWinForms;

internal static class Program
{
   /// <summary>
   ///  The main entry point for the application.
   /// </summary>
   [STAThread]
   static void Main()
   {
      // Build configuration
      var configuration = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .Build();

      var services = new ServiceCollection();
      services.Configure<AppSettings>(configuration.GetSection("App"));
      services.AddSingleton<Form1>();

      // Build the service provider
      var serviceProvider = services.BuildServiceProvider();

      ApplicationConfiguration.Initialize();
      Application.Run(serviceProvider.GetRequiredService<Form1>());
   }
}