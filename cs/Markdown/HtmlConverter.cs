using System.Text;
using Markdown.Tag;

namespace Markdown;

public class HtmlConverter
{
    private static readonly Dictionary<TagType, string> TagType = TagConfig.HtmlTags;

    public string ConvertMarkdownToHtml(string markdownText, List<Token> tokens)
    {
        var resultHtml = new StringBuilder(markdownText);
        var htmlTags = ConvertToHtml(tokens);
        var shift = 0;

        foreach (var tag in htmlTags)
        {
            var mdTagLength = 1;
            if (tag.Type == Tag.TagType.Bold)
            {
                mdTagLength = 2;
                shift--;
            }

            if (tag.IsClosing)
            {
                if (tag.Type == Tag.TagType.Header)
                {
                    mdTagLength = 0;
                    shift++;
                }
                else if (tag.Type == Tag.TagType.EscapedSymbol)
                {
                    mdTagLength = 0;
                    shift++;
                }
            }

            resultHtml.Remove(tag.Index + shift, mdTagLength);
            resultHtml.Insert(tag.Index + shift, tag.GetTag());
            shift = resultHtml.Length - markdownText.Length;
        }

        return resultHtml.ToString();
    }

    private static List<HtmlTag> ConvertToHtml(List<Token> tokens)
    {
        var htmlTags = new List<HtmlTag>();
        foreach (var token in tokens)
        {
            htmlTags.Add(new HtmlTag(token.TagType, token.StartIndex, false, TagType[token.TagType]));
            htmlTags.Add(new HtmlTag(token.TagType, token.EndIndex, true, TagType[token.TagType]));
        }

        return htmlTags.OrderBy(tag => tag.Index).ToList();
    }
}