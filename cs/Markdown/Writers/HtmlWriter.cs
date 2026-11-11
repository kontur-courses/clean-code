using System.Text;
using Markdown.Tokens;
using Markdown.Writers.TagHtmlInfos;

namespace Markdown.Writers;

public class HtmlWriter(IEnumerable<ITagHtmlInfo> tagInfos) : IWriter
{
    private readonly Dictionary<Type, ITagHtmlInfo> tagInfos = tagInfos.ToDictionary(t => t.TagType);
    
    public string Write(List<Token> tokens)
    {
        var result = new StringBuilder();
        foreach (var token in tokens)
        {
            result.Append(WriteToken(token));
        }

        return result.ToString();
    }
    
    private StringBuilder WriteToken(Token token)
    {
        if (tagInfos.TryGetValue(token.GetType(), out var tagInfo))
        {
            return token.Role switch
            {
                TokenRole.Opening => new StringBuilder(tagInfo.GetOpeningTag(token)),
                TokenRole.Closing => new StringBuilder(tagInfo.GetClosingTag(token)),
                _ => new StringBuilder(token.Value)
            };
        }

        return new StringBuilder(token.Value);
    }
}