namespace TestProject;

using Testovoe.Task_1;
using Xunit;

public class StringCompressorTests
{
    #region Compress

    [Theory]
    [InlineData("aaabbc", "a3b2c")]       // Базовый случай
    [InlineData("abc", "abc")]            // Нет повторяющихся символов
    [InlineData("aabbbcc", "a2b3c2")]     // Несколько повторяющихся символов
    [InlineData("zzzz", "z4")]            // Все символы одинаковые
    [InlineData("", "")]                  // Пустая строка
    [InlineData("   ", " 3")]             // Пробелы (если нужно их учитывать)
    [InlineData("a", "a")]                // Один символ
    [InlineData("AaA", "AaA")]            // Разный регистр (если важно)
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

    #endregion

    #region Decompress

    [Theory]
    [InlineData("a3b2c", "aaabbc")]       // Базовый случай
    [InlineData("abc", "abc")]            // Без чисел
    [InlineData("a2b3c2", "aabbbcc")]     // Несколько чисел
    [InlineData("z4", "zzzz")]            // Одно число
    [InlineData("", "")]                  // Пустая строка
    [InlineData("a", "a")]                // Один символ
    [InlineData("AaA", "AaA")]            // Разный регистр (если важно)
    [InlineData(" 3", "   ")]             // Пробелы (если нужно их учитывать)
    public void Decompress_ValidInput_ReturnsOriginalString(string compressed, string expected)
    {
        string result = StringCompressor.Decompress(compressed);
        Assert.Equal(expected, result);
    }

    #endregion

    [Fact]
    public void CompressDecompress_RoundTrip_ReturnsOriginalString()
    {
        string original = "aaabbbccdeeffgg";
        string compressed = StringCompressor.Compress(original);
        string decompressed = StringCompressor.Decompress(compressed);
        Assert.Equal(original, decompressed);
    }
}