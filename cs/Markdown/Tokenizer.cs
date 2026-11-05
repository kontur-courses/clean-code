using Markdown.Tokens;

namespace Markdown;

/// <summary>
/// Разбивает текст на Token, создаёт Token и LinkToken,
/// использует LinkParser для распознавания ссылок,
/// рекурсивно вызывает себя для текста внутри ссылки
/// </summary>
internal static class Tokenizer
{
    public static List<Token> Tokenize(string text)
    {
        var tokens = new List<Token>();
        var position = 0;
        while (position < text.Length)
        {
            position = text[position] switch
            {
                '\\' => ReadBackslash(text, position, tokens),
                '_' => ReadUnderscores(text, position, tokens),
                '[' => ReadLink(text, position, tokens),
                _ => ReadText(text, position, tokens)
            };
        }
        return tokens;
    }

    private static int ReadBackslash(string text, int start, List<Token> tokens)
    {
        throw  new NotImplementedException();
    }
    

    private static int ReadUnderscores(string text, int start, List<Token> tokens)
    {
        throw  new NotImplementedException();
    }

    private static int ReadLink(string text, int start, List<Token> tokens)
    {
        if (LinkParser.TryParse(text, start, out var token))
        {
            //...
        }
        throw  new NotImplementedException();
    }

    private static int ReadText(string text, int start, List<Token> tokens)
    {
        throw new NotImplementedException();
    }
}