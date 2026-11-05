using System.Text;
using Markdown.Tokens;

namespace Markdown;

/// <summary>
/// Превращает токены с расставленными ролями в html, читает все типы токенов
/// </summary>
internal static class HtmlWriter
{
    public static string Write(IEnumerable<Token> tokens)
    {
        var result = new StringBuilder();
        Append(result, tokens);
        return result.ToString();

    }
    
    private static void Append(StringBuilder result, IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            if (token is LinkToken link)
                AppendLink(result, link);
            else
                AppendMark(result, token);
        }
    }
    
    private static void AppendMark(StringBuilder result, Token token)
    {
        throw new NotImplementedException();
    }
    
    private static void AppendLink(StringBuilder result, LinkToken link)
    {
        throw new NotImplementedException();
    }
}