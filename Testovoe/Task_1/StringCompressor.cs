using System.Text;

namespace Testovoe.Task_1;
public static class StringCompressor
{
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return ""; // но сюда по-хорошему обработчик ошибок добавить чтобы не был null
        }

        IDictionary<char, int> letterCountDict = new Dictionary<char, int>(input.Length);

        foreach (var itemChar in input)
        {
            if (letterCountDict.TryGetValue(itemChar, out int value))
            {
                letterCountDict[itemChar] = ++value;
                continue;
            }

            letterCountDict.Add(itemChar, 1);
        }

        return GetResult(letterCountDict);
    }

    private static string GetResult(IDictionary<char, int> letterCountDict)
    {
        var result = new StringBuilder(letterCountDict.Count);

        foreach (var item in letterCountDict)
        {
            if (item.Value == 1)
            {
                result.Append(item.Key);
            }
            else
            {
                result.Append(item.Key);
                result.Append(item.Value);
            }
        }

        return result.ToString();
    }
}
