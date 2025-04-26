using System.Text;

namespace Testovoe.Task_1;
public static class StringCompressor
{
    /// <summary>
    /// Принимает строку для компрессии
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>входная строка, подлежая компрессии</remarks>
    /// <returns>сжатая строка</returns>
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input ?? string.Empty; // но сюда по-хорошему обработчик ошибок добавить чтобы не был null
        }

        IList<(char Symbol, int Count)> charGroups = new List<(char, int)>();

        int currentCount = 0;
        char prevChar = input[0];

        foreach(char currentChar in input)
        {
            if(currentChar == prevChar)
            {
                // Продолжение последовательности
                currentCount++;
                continue;
            }
            
            // Новая последовательность
            charGroups.Add((prevChar, currentCount));
            prevChar = currentChar;
            currentCount = 1;
        }

        // Добавляем последнюю последовательность
        charGroups.Add((prevChar, currentCount));

        // Собираем сжатую строку
        var compressed = new StringBuilder();
        foreach(var group in charGroups)
        {
            compressed.Append(group.Symbol);
            if(group.Count > 1)
                compressed.Append(group.Count);
        }

        return compressed.ToString();
    }

    /// <summary>
    /// Принимает строку для декомпрессии
    /// </summary>
    /// <param name="compressed"></param>
    /// <remarks>входная строка, подлежая компрессии</remarks>
    /// <returns>строка после декомпрессии</returns>
    public static string Decompress(string compressed)
    {
        if(string.IsNullOrEmpty(compressed))
            return compressed;

        var charGroups = new List<(char Symbol, int Count)>();
        char? currentChar = null;
        var numberBuilder = new StringBuilder();

        foreach(char c in compressed)
        {
            if(char.IsWhiteSpace(c) || char.IsLetterOrDigit(c))
            {
                if(currentChar.HasValue && (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    if(char.IsDigit(c))
                    {
                        numberBuilder.Append(c);
                    }
                    else
                    {
                        AddCurrentGroup(charGroups, currentChar.Value, numberBuilder);
                        currentChar = c;
                    }
                }
                else
                {
                    AddCurrentGroup(charGroups, currentChar, numberBuilder);
                    currentChar = c;
                }
            }
        }

        AddCurrentGroup(charGroups, currentChar, numberBuilder);

        var result = new StringBuilder();
        foreach(var group in charGroups)
        {
            result.Append(group.Symbol, group.Count);
        }

        return result.ToString();
    }

    private static void AddCurrentGroup(List<(char, int)> charGroups, char? currentChar, StringBuilder numberBuilder)
    {
        if(currentChar.HasValue)
        {
            int count = numberBuilder.Length > 0 ? int.Parse(numberBuilder.ToString()) : 1;
            charGroups.Add((currentChar.Value, count));
            numberBuilder.Clear();
        }
    }
}
