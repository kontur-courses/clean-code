using System.Net.Mime;
using System.Text;
using Markdown.TagsType;

namespace Markdown.Parsers;

public class LinkParser : IParser
{
    private LinkTag linkTag;
    private StringBuilder nameOfLink;
    private StringBuilder urlOfLink;
    public bool IsThisLink;

    public LinkParser()
    {
        linkTag = new LinkTag();
        nameOfLink = new StringBuilder();
        urlOfLink = new StringBuilder();
    }

    public Token LinkParse(string text)
    {
        IsThisLink = true;
        for (int i = 0; i < text.Length; i++)
        {
            switch (text[i])
            {
                case '[':
                    break;
                case ']':
                    linkTag.LinkName = nameOfLink.ToString();
                    linkTag.LinkUrl = text.Substring(i + 2, text.Length - i - 3);
                    return GenerateLinkToken();
                default:
                    nameOfLink.Append(text[i]);
                    break;
            }
        }

        nameOfLink.Append(' ');
        return null!;
    }

    private Token GenerateLinkToken()
    {
        var context = linkTag.GetHtmlOpenTag + linkTag.LinkName + linkTag.GetHtmlCloseTag;
        urlOfLink.Clear();
        nameOfLink.Clear();
        return new Token(context, linkTag);
    }

    public bool TryParse(char symbol,  string text, int i)
    {
        return symbol == '[';
    }

    public Token Parse(string text, ref int i)
    {
        var token = LinkParse(text);
        i = text.Length-1;
        return token;
    }
}