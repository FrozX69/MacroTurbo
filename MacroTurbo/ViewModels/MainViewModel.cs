using System.Windows.Input;
using MacroTurbo.Services;
using MacroTurbo.Utils;

namespace MacroTurbo.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IMacroService _macroService;
    private CancellationTokenSource? _macroCancellationTokenSource;
    private CancellationTokenSource? _antiAfkCancellationTokenSource;
    
    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    private bool _isAntiAfkRunning;

    public bool IsAntiAfkRunning
    {
        get => _isAntiAfkRunning;
        set => SetProperty(ref _isAntiAfkRunning, value);
    }

    private int _intervalMs = 500;

    public int IntervalMs
    {
        get => _intervalMs;
        set => SetProperty(ref _intervalMs, value);
    }
    
    public ICommand ToggleMacroCommand { get; }
    public ICommand ToggleAntiAfkCommand { get; }

    public MainViewModel() : this(new MacroService())
    {
    }

    public MainViewModel(IMacroService macroService)
    {
        _macroService = macroService;
        ToggleMacroCommand = new RelayCommand(_ => ExecuteToggleMacro());
        ToggleAntiAfkCommand = new RelayCommand(_ => ExecuteToggleAntiAfk());
    }

    public void ExecuteToggleMacro()
    {
        if (IsRunning)
        {
            StopMacro();
        }
        else
        {
            StartMacro();
        }
    }

    private async void StartMacro()
    {
        IsRunning = true;
        _macroCancellationTokenSource = new CancellationTokenSource();
        
        byte keyE = 0x45; // Virtual-Key code for 'E'

        try
        {
            await _macroService.StartAutoClickerAsync(keyE, IntervalMs, _macroCancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // Arrêt volontaire
        }
        finally
        {
            IsRunning = false;
        }
    }

    private void StopMacro()
    {
        _macroCancellationTokenSource?.Cancel();
    }
    
    public void ExecuteToggleAntiAfk()
    {
        if (IsAntiAfkRunning)
        {
            StopAntiAfk();
        }
        else
        {
            StartAntiAfk();
        }
    }

    private async void StartAntiAfk()
    {
        IsAntiAfkRunning = true;
        _antiAfkCancellationTokenSource = new CancellationTokenSource();

        try
        {
            await _macroService.StartAntiAfkAsync(_antiAfkCancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // Arrêt volontaire
        }
        finally
        {
            IsAntiAfkRunning = false;
        }
    }

    private void StopAntiAfk()
    {
        _antiAfkCancellationTokenSource?.Cancel();
    }
}
