namespace TestProject;

using Testovoe.Task_1;
using Xunit;

public class StringCompressorTests
{
    #region Compress

    [Theory]
    [InlineData("aaabbc", "a3b2c")]       // ������� ������
    [InlineData("abc", "abc")]            // ��� ������������� ��������
    [InlineData("aabbbcc", "a2b3c2")]     // ��������� ������������� ��������
    [InlineData("zzzz", "z4")]            // ��� ������� ����������
    [InlineData("", "")]                  // ������ ������
    [InlineData("   ", " 3")]             // ������� (���� ����� �� ���������)
    [InlineData("a", "a")]                // ���� ������
    [InlineData("AaA", "AaA")]            // ������ ������� (���� �����)
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
    [InlineData("a3b2c", "aaabbc")]       // ������� ������
    [InlineData("abc", "abc")]            // ��� �����
    [InlineData("a2b3c2", "aabbbcc")]     // ��������� �����
    [InlineData("z4", "zzzz")]            // ���� �����
    [InlineData("", "")]                  // ������ ������
    [InlineData("a", "a")]                // ���� ������
    [InlineData("AaA", "AaA")]            // ������ ������� (���� �����)
    [InlineData(" 3", "   ")]             // ������� (���� ����� �� ���������)
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