using Testovoe.Task_3.Core.Models;

namespace Testovoe.Task_3.Core.Interfaces;
public interface ILogWriter
{
    Task WriteStandardizedAsync(LogEntry entry, string outputPath, CancellationToken cancellationToken = default);
    Task WriteProblemAsync(string problemLine, string problemsFilePath, CancellationToken cancellationToken = default);
}
