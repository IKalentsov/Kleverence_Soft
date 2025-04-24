namespace TestProject;

using Testovoe.Task_1;
using Xunit;

public class StringCompressorTests
{
    [Theory]
    [InlineData("aaabbc", "a3b2c")]       // ������� ������
    [InlineData("abc", "abc")]            // ��� ������������� ��������
    [InlineData("aabbbcc", "a2b3c2")]     // ��������� ������������� ��������
    [InlineData("zzzz", "z4")]            // ��� ������� ����������
    [InlineData("", "")]                  // ������ ������
    [InlineData("   ", " 3")]             // ������� (���� ����� �� ���������)
    [InlineData("a", "a")]                // ���� ������
    [InlineData("AaA", "A2a")]            // ������ ������� (���� �����)
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