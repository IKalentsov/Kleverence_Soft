using Testovoe.Task_2;

namespace TestProject;
public class ServerTests : IAsyncLifetime
{
    private readonly CancellationTokenSource _cts = new();

    public async Task InitializeAsync() => await Server.ResetAsync(_cts.Token);
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetCountAsync_Initially_ReturnsZero()
    {
        var result = await Server.GetCountAsync(_cts.Token);
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task AddAsync_IncrementsValueCorrectly()
    {
        await Server.AddAsync(5, _cts.Token);
        var result = await Server.GetCountAsync(_cts.Token);
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task MultipleReaders_CanReadSimultaneously()
    {
        await Server.AddAsync(10, _cts.Token);
        const int readerThreads = 10;
        var readCount = 0;

        async Task Reader()
        {
            await Server.GetCountAsync(_cts.Token);
            Interlocked.Increment(ref readCount);
        }

        var tasks = new Task[ readerThreads ];
        for(int i = 0; i < readerThreads; i++)
        {
            tasks[ i ] = Reader();
        }

        await Task.WhenAll(tasks);
        Assert.Equal(readerThreads, readCount);
    }

    [Fact]
    public async Task Writers_AreExclusive()
    {
        const int writerThreads = 5;

        async Task Writer()
        {
            await Server.AddAsync(1, _cts.Token);
        }

        var tasks = new Task[ writerThreads ];
        for(int i = 0; i < writerThreads; i++)
        {
            tasks[ i ] = Writer();
        }

        await Task.WhenAll(tasks);
        var result = await Server.GetCountAsync(_cts.Token);
        Assert.Equal(writerThreads, result);
    }

    [Fact]
    public async Task Readers_WaitForWriters()
    {
        var writerFinished = false;
        var readerSawOldValue = false;

        async Task Writer()
        {
            await Server.AddAsync(100, _cts.Token);
            writerFinished = true;
        }

        async Task Reader()
        {
            var value = await Server.GetCountAsync(_cts.Token);
            readerSawOldValue = (value == 0);
        }

        var writerTask = Writer();
        var readerTask = Reader();

        await Task.WhenAll(writerTask, readerTask);

        Assert.True(writerFinished);
        Assert.False(readerSawOldValue);
    }

    [Fact]
    public async Task Operations_CanBeCancelled()
    {
        var cancelledTokenSource = new CancellationTokenSource();
        cancelledTokenSource.Cancel();

        // Проверяем, что выбрасывается либо TaskCanceledException, либо OperationCanceledException
        var ex1 = await Record.ExceptionAsync(
            () => Server.GetCountAsync(cancelledTokenSource.Token).AsTask());

        Assert.True(ex1 is TaskCanceledException || ex1 is OperationCanceledException);

        var ex2 = await Record.ExceptionAsync(
            () => Server.AddAsync(1, cancelledTokenSource.Token).AsTask());

        Assert.True(ex2 is TaskCanceledException || ex2 is OperationCanceledException);
    }
}
