namespace DentalSaaS.Blazor.Utils;

public class ActionDebouncer : IDisposable
{
    private readonly TimeSpan _delay;
    private CancellationTokenSource? _cancellationTokenSource;

    public ActionDebouncer(int milliseconds = 800)
    {
        _delay = TimeSpan.FromMilliseconds(milliseconds);
    }

    public void Invoke(Func<Task> action)
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(_delay, token);
                if (!token.IsCancellationRequested)
                {
                    await action();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorar, el usuario hizo clic de nuevo antes del delay
            }
        }, token);
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        GC.SuppressFinalize(this);
    }
}
