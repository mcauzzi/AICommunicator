using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Frontend.Services;
using Frontend.ViewModels;
using Frontend.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using ServiceImplementations;

namespace Frontend;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddScoped<IRoutingManager, RoutingManager>();
        var logger=new LoggerConfiguration()
            .WriteTo.File("./logs/frontend.log",LogEventLevel.Information)
               .CreateLogger();
        collection.AddLogging(builder =>
                                  builder.AddSerilog(logger));
        collection.AddAiCommunicationServices();
        var services = collection.BuildServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
                                 {
                                     DataContext =new MainWindowViewModel(services.GetRequiredService<IRoutingManager>()),
                                 };
        }

        base.OnFrameworkInitializationCompleted();
    }
}