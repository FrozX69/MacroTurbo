using System.Runtime.InteropServices;
using MacroTurbo.Utils;

namespace MacroTurbo.Services;

public class MacroService : IMacroService
{
    public async Task StartAutoClickerAsync(byte virtualKeyCode, int delayMs, CancellationToken cancellationToken)
    {
        int safeDelay = Math.Max(11, delayMs); 
        
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(safeDelay));
        
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            NativeMethods.keybd_event(virtualKeyCode, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            NativeMethods.keybd_event(virtualKeyCode, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }

    public void StartClickHolder(byte virtualKeyCode)
    {
        NativeMethods.keybd_event(virtualKeyCode, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
    }

    public void StopClickHolder(byte virtualKeyCode)
    {
        NativeMethods.keybd_event(virtualKeyCode, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    public async Task StartAntiAfkAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(50));
        
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            NativeMethods.keybd_event(NativeMethods.VK_SCROLL, 0, NativeMethods.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            NativeMethods.keybd_event(NativeMethods.VK_SCROLL, 0, NativeMethods.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }
}

