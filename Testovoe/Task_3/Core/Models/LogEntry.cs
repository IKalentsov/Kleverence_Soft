namespace Testovoe.Task_3.Core.Models;
public sealed record LogEntry(
    DateTime Date,
    string Time,
    string Level,
    string CallingMethod,
    string Message)
{
    public static LogEntry InvalidEntry(string rawLine) => new(
        DateTime.MinValue,
        string.Empty,
        string.Empty,
        string.Empty,
        rawLine);

    public bool IsValid => Date != DateTime.MinValue;
}
