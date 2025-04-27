namespace Testovoe.Task_2;

public static class Server
{
    private static int _count;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public static async ValueTask<int> GetCountAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            ct.ThrowIfCancellationRequested();
            return _count;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public static async ValueTask AddAsync(int value, CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            ct.ThrowIfCancellationRequested();
            _count += value;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public static async ValueTask ResetAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            ct.ThrowIfCancellationRequested();
            _count = 0;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
