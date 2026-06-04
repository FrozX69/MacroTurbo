using System.Windows;
using MacroTurbo.Services;
using MacroTurbo.ViewModels;

namespace MacroTurbo;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Injection des dépendances
        var macroService = new MacroService();
        var mainViewModel = new MainViewModel(macroService);

        // Récupérer la MainWindow et assigner le ViewModel
        if (Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.DataContext = mainViewModel;
        }
    }
}