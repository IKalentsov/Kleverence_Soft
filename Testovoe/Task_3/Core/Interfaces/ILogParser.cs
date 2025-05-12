using Testovoe.Task_3.Core.Models;

namespace Testovoe.Task_3.Core.Interfaces;
public interface ILogParser
{
    Task<LogEntry> ParseAsync(string line, CancellationToken cancellationToken = default);
}
