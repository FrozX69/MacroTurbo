
namespace MacroTurbo.Services;

public interface IMacroService
{
    Task StartAutoClickerAsync(byte virtualKeyCode, int delayMs, CancellationToken cancellationToken);
    void StartClickHolder(byte virtualKeyCode);
    void StopClickHolder(byte virtualKeyCode);
    Task StartAntiAfkAsync(CancellationToken cancellationToken);
}

