namespace TestProject;

using Testovoe.Task_1;
using Xunit;

public class StringCompressorTests
{
    [Theory]
    [InlineData("aaabbc", "a3b2c")]       // Базовый случай
    [InlineData("abc", "abc")]            // Нет повторяющихся символов
    [InlineData("aabbbcc", "a2b3c2")]     // Несколько повторяющихся символов
    [InlineData("zzzz", "z4")]            // Все символы одинаковые
    [InlineData("", "")]                  // Пустая строка
    [InlineData("   ", " 3")]             // Пробелы (если нужно их учитывать)
    [InlineData("a", "a")]                // Один символ
    [InlineData("AaA", "A2a")]            // Разный регистр (если важно)
    public void StringCompressor_CompressesCorrectly(string input, string expected)
    {
        // Act
        var result = StringCompressor.Compress(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StringCompressor_HandlesNullInput()
    {
        // Arrange
        string input = null;

        // Act
        var result = StringCompressor.Compress(input);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void StringCompressor_HandlesMixedCharacters()
    {
        // Arrange
        string input = "aAAbBccC";

        // Act
        var result = StringCompressor.Compress(input);

        // Assert
        Assert.Equal("aA2bBc2C", result);
    }
}