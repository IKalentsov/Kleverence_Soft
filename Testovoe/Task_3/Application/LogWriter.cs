using Testovoe.Task_3.Core.Interfaces;
using Testovoe.Task_3.Core.Models;

namespace Testovoe.Task_3.Application;
public sealed class LogWriter : ILogWriter
{
    public async Task WriteStandardizedAsync(LogEntry entry, string outputPath, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(outputPath, true);
        var line = $"{entry.Date:dd-MM-yyyy}\t{entry.Time}\t{entry.Level}\t{entry.CallingMethod}\t{entry.Message}";
        await writer.WriteLineAsync(line.AsMemory(), cancellationToken);
    }

    public async Task WriteProblemAsync(string problemLine, string problemsFilePath, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(problemsFilePath, true);
        await writer.WriteLineAsync(problemLine.AsMemory(), cancellationToken);
    }
}
