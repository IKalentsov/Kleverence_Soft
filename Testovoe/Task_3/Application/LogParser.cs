using Testovoe.Task_3.Core.Interfaces;
using Testovoe.Task_3.Core.Models;

namespace Testovoe.Task_3.Application;
public sealed class LogParser : ILogParser
{
    private const string DefaultCallingMethod = "DEFAULT";
    private static readonly Dictionary<string, string> LevelMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        [ "INFORMATION" ] = "INFO",
        [ "INFO" ] = "INFO",
        [ "WARNING" ] = "WARN",
        [ "WARN" ] = "WARN",
        [ "ERROR" ] = "ERROR",
        [ "DEBUG" ] = "DEBUG"
    };

    public async Task<LogEntry> ParseAsync(string line, CancellationToken cancellationToken = default)
    {
        await Task.Yield(); // Для асинхронности

        if(string.IsNullOrWhiteSpace(line))
            return LogEntry.InvalidEntry(line);

        // Пытаемся определить формат и распарсить
        if(TryParseFormat1(line, out var entry1))
            return entry1;

        if(TryParseFormat2(line, out var entry2))
            return entry2;

        return LogEntry.InvalidEntry(line);
    }

    private static bool TryParseFormat1(string line, out LogEntry entry)
    {
        entry = LogEntry.InvalidEntry(line);

        // Формат 1: 10.03.2025 15:14:49.523 INFORMATION  Версия программы: '3.4.0.48729'
        var parts = line.Split(' ', 4, StringSplitOptions.RemoveEmptyEntries);
        if(parts.Length < 4) return false;

        if(!DateTime.TryParse(parts[ 0 ], out var date)) return false;
        if(!TimeSpan.TryParse(parts[ 1 ], out var time)) return false;

        var level = NormalizeLevel(parts[ 2 ]);
        var message = parts[ 3 ];

        entry = new LogEntry(
            date,
            parts[ 1 ], // сохраняем оригинальный формат времени
            level,
            DefaultCallingMethod,
            message);

        return true;
    }

    private static bool TryParseFormat2(string line, out LogEntry entry)
    {
        entry = LogEntry.InvalidEntry(line);

        // Формат 2: 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'
        var parts = line.Split('|', StringSplitOptions.TrimEntries);
        if(parts.Length < 5) return false;

        if(!DateTime.TryParse(parts[ 0 ], out var date)) return false;
        if(!TimeSpan.TryParse(parts[ 1 ], out var time)) return false;

        var level = NormalizeLevel(parts[ 2 ]);
        var callingMethod = parts[ 3 ];
        var message = parts[ 4 ];

        entry = new LogEntry(
            date,
            parts[ 1 ], // сохраняем оригинальный формат времени
            level,
            string.IsNullOrWhiteSpace(callingMethod) ? DefaultCallingMethod : callingMethod,
            message);

        return true;
    }

    private static string NormalizeLevel(string level)
    {
        return LevelMappings.TryGetValue(level, out var normalized)
            ? normalized
            : level;
    }
}
