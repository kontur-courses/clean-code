using Markdown.Tags;
using Markdown.Tokens;
using System.Text;

namespace Markdown.Generators;

public class HtmlGenerator : IMarkingGenerator
{
    public string Generate(IEnumerable<IToken> tokens, string text)
    {
        var listToken = tokens.ToList();
        var result = new StringBuilder();
        for (var i = 0; i < listToken.Count; i++)
        {
            var token = listToken[i];
            SupportedTags.Tags.TryGetValue(token.Content, out var tag);
            if (token.Type == TokenType.Text)
            {
                result.Append(token.Content);
            }
            else if (tag?.TagType == TagType.LinkDescription && tag.IsOpen)
            {
                result.Append(GetLinkInHtml(ref i, listToken, text));
            }
            else
            {
                result.Append(tag?.CreateHtmlTag(SupportedTags.IsOpenTag(token, text)));
            }
        }

        return result.ToString();
    }

    private string GetLinkInHtml(ref int index, List<IToken> listToken, string text)
    {
        var description = CreateStringWhileNotCloseTagSomeType(ref index, listToken, text, TagType.LinkDescription);
        var link = CreateStringWhileNotCloseTagSomeType(ref index, listToken, text, TagType.Link);

        return $"<a href=\"{link}\">{description}</a>";
    }

    private string CreateStringWhileNotCloseTagSomeType(ref int index, List<IToken> listToken, 
        string text, TagType type)
    {
        var result = new StringBuilder();
        index++;
        var token = listToken[index];
        SupportedTags.Tags.TryGetValue(token.Content, out var currentTag);
        while (index < listToken.Count 
               && (currentTag?.TagType != type || currentTag?.IsOpen == true || token.Type == TokenType.Text))
        {
            if (currentTag?.TagType != type || token.Type == TokenType.Text)
            {
                if (token.Type == TokenType.Text)
                    result.Append(token.Content);
                else
                    result.Append(currentTag?.CreateHtmlTag(SupportedTags.IsOpenTag(token, text)));
            }
            index++;
            token = listToken[index];
            SupportedTags.Tags.TryGetValue(token.Content, out currentTag);
        }
        return result.ToString();
    }
}
