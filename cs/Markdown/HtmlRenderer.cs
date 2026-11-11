using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Blocks;
using Markdown.Inline;

namespace Markdown;

public class HtmlRenderer
{
    private const string HeadingFormat = "<h1>{0}</h1>";
    private const string ParagraphFormat = "<p>{0}</p>";
    private const string ItalicFormat = "<em>{0}</em>";
    private const string StrongFormat = "<strong>{0}</strong>";
    private const string LinkFormat = "<a href=\"{1}\">{0}</a>";

    public string RenderDocument(IEnumerable<IBlock> blocks)
    {
        var sb = new StringBuilder();
        foreach (var block in blocks)
        {
            sb.Append(RenderBlock(block));
        }
        return sb.ToString();
    }

    private string RenderBlock(IBlock block)
    {
        return block switch
        {
            HeadingBlock heading => string.Format(HeadingFormat, RenderInlineTags(heading.InlineTags)),
            ParagraphBlock paragraph => string.Format(ParagraphFormat, RenderInlineTags(paragraph.InlineTags)),
            _ => throw new NotSupportedException($"Unknown block type: {block.GetType()}")
        };
    }

    private string RenderInlineTags(IEnumerable<IInlineTag> tags)
    {
        var sb = new StringBuilder();
        foreach (var tag in tags)
        {
            sb.Append(tag switch
            {
                TextTag text => EscapeHtml(text.Text),
                ItalicTag italic => string.Format(ItalicFormat, RenderInlineTags(italic.Content)),
                StrongTag strong => string.Format(StrongFormat, RenderInlineTags(strong.Content)),
                LinkTag link => string.Format(LinkFormat, RenderInlineTags(link.Content), EscapeHtml(link.Url)),
                _ => throw new NotSupportedException($"Unknown tag type: {tag.GetType()}")
            });
        }
        return sb.ToString();
    }

    private string EscapeHtml(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;");
    }
}