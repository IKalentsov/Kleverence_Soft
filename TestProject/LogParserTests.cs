using Testovoe.Task_3.Application;
using Testovoe.Task_3.Core.Interfaces;

namespace TestProject;
public class LogParserTests : IAsyncDisposable
{
    private readonly ILogParser _parser;
    private const string InputDir = "Files/Input";
    private const string OutputDir = "Files/Output";

    public LogParserTests()
    {
        _parser = new LogParser();

        // Создаем папки если их нет
        Directory.CreateDirectory(InputDir);
        Directory.CreateDirectory(OutputDir);

        // Очищаем выходную папку перед тестами
        if(Directory.Exists(OutputDir))
        {
            Directory.Delete(OutputDir, true);
            Directory.CreateDirectory(OutputDir);
        }
    }

    public async ValueTask DisposeAsync()
    {
        // Очищаем выходную папку после тестов
        if(Directory.Exists(OutputDir))
        {
            Directory.Delete(OutputDir, true);
        }
    }

    [Fact]
    public async Task ParseAsync_ShouldCorrectlyParseFormat1()
    {
        // Arrange
        var filePath = Path.Combine(InputDir, "Test_InputFormat_Type_INFO.txt");
        var content = await File.ReadAllTextAsync(filePath);

        // Act
        var result = await _parser.ParseAsync(content.Trim());

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(new DateTime(2025, 3, 10), result.Date);
        Assert.Equal("15:14:49.523", result.Time);
        Assert.Equal("INFO", result.Level);
        Assert.Equal("DEFAULT", result.CallingMethod);
        Assert.Equal("Версия программы: '3.4.0.48729'", result.Message);
    }

    [Fact]
    public async Task ParseAsync_ShouldCorrectlyParseFormat2()
    {
        // Arrange
        var filePath = Path.Combine(InputDir, "Test_InputFormat_Type_INFORMATION");
        var content = await File.ReadAllTextAsync(filePath);

        // Act
        var result = await _parser.ParseAsync(content.Trim());

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(new DateTime(2025, 3, 10), result.Date);
        Assert.Equal("15:14:51.5882", result.Time);
        Assert.Equal("INFO", result.Level);
        Assert.Equal("MobileComputer.GetDeviceId", result.CallingMethod);
        Assert.Equal("Код устройства: '@MINDEO-M40-D-410244015546'", result.Message);
    }

    [Fact]
    public async Task ParseAsync_ShouldMarkInvalidLinesAsInvalid()
    {
        // Arrange
        var filePath = Path.Combine(InputDir, "invalid_format.txt");
        var content = await File.ReadAllTextAsync(filePath);

        // Act
        var result = await _parser.ParseAsync(content.Trim());

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Invalid log line format", result.Message);
    }

    [Fact]
    public async Task ParseAsync_ShouldNormalizeLevelsCorrectly()
    {
        // Arrange
        var filePath = Path.Combine(InputDir, "levels_normalization.txt");
        var lines = await File.ReadAllLinesAsync(filePath);

        // Act & Assert
        var result1 = await _parser.ParseAsync(lines[ 0 ].Trim());
        Assert.Equal("INFO", result1.Level);

        var result2 = await _parser.ParseAsync(lines[ 1 ].Trim());
        Assert.Equal("INFO", result2.Level);

        var result3 = await _parser.ParseAsync(lines[ 2 ].Trim());
        Assert.Equal("WARN", result3.Level);

        var result4 = await _parser.ParseAsync(lines[ 3 ].Trim());
        Assert.Equal("WARN", result4.Level);
    }

    [Fact]
    public async Task ParseAsync_ShouldHandleEmptyCallingMethod()
    {
        // Arrange
        var filePath = Path.Combine(InputDir, "empty_calling_method.txt");
        var content = await File.ReadAllTextAsync(filePath);

        // Act
        var result = await _parser.ParseAsync(content.Trim());

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("DEFAULT", result.CallingMethod);
    }
}
