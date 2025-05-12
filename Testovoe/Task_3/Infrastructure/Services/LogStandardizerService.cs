using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testovoe.Task_3.Core.Interfaces;

namespace Testovoe.Task_3.Infrastructure.Services;
public sealed class LogStandardizerService : BackgroundService
{
    private readonly ILogParser _logParser;
    private readonly ILogWriter _logWriter;
    private readonly ILogger<LogStandardizerService> _logger;

    public LogStandardizerService(
        ILogParser logParser,
        ILogWriter logWriter,
        ILogger<LogStandardizerService> logger)
    {
        _logParser = logParser;
        _logWriter = logWriter;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting log standardization process");

            var inputDir = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Input");
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Output");

            Directory.CreateDirectory(inputDir);
            Directory.CreateDirectory(outputDir);

            var outputPath = Path.Combine(outputDir, "standardized_logs.txt");
            var problemsPath = Path.Combine(outputDir, "problems.txt");

            // Очищаем предыдущие результаты
            File.Delete(outputPath);
            File.Delete(problemsPath);

            var inputFiles = Directory.GetFiles(inputDir, "*.txt");
            var tasks = inputFiles.Select(file => ProcessFileAsync(file, outputPath, problemsPath, stoppingToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Log standardization completed successfully");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error during log standardization");
            throw;
        }
    }

    private async Task ProcessFileAsync(string inputFilePath, string outputPath, string problemsPath, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing file: {FilePath}", inputFilePath);

        using var reader = new StreamReader(inputFilePath);

        while(await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            var entry = await _logParser.ParseAsync(line, cancellationToken);

            if(entry.IsValid)
            {
                await _logWriter.WriteStandardizedAsync(entry, outputPath, cancellationToken);
            }
            else
            {
                await _logWriter.WriteProblemAsync(line, problemsPath, cancellationToken);
            }
        }
    }
}
