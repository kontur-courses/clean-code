using System.Text;
using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown.HtmlTools;

internal static class HtmlTokenHandler
{
    public static string Handle(string inputText, IEnumerable<Token> tokens)
    {
        var processedText = new StringBuilder(inputText);
        var shiftOffset = 0;

        foreach (var token in TokensAccordanceWithRules.Handle(tokens))
        {
            processedText.Remove(token.Position + shiftOffset, token.TagLength);
            processedText.Insert(token.Position + shiftOffset, GetHtmlTag(inputText, token));

            shiftOffset = processedText.Length - inputText.Length;
        }

        return processedText.ToString();
    }

    private static string GetHtmlTag(string inputText, Token token)
    {
        return token.Tag == Tag.Link ? GetLinkTag(inputText, token) : GetHtmlTag(token);
    }

    private static string GetHtmlTag(Token token)
    {
        var htmlTag = token.Tag.GetHtmlOpenTag();

        if (token.TagType == TagType.Close)
        {
            htmlTag = htmlTag.Insert(1, "/");
        }

        return htmlTag;
    }

    private static string GetLinkTag(string text, Token token)
    {
        return HtmlLinkTagBuilder.Build(text.Substring(token.Position, token.TagLength));
    }
}