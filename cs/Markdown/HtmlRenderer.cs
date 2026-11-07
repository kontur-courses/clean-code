using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Blocks;
using Markdown.Inline;

namespace Markdown;

public class HtmlRenderer
{

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
            HeadingBlock h => $"<h1>{RenderInlineTags(h.InlineTags)}</h1>",
            ParagraphBlock p => $"<p>{RenderInlineTags(p.InlineTags)}</p>",
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
                TextTag t => t.Text,
                ItalicTag i => $"<em>{RenderInlineTags(i.Content)}</em>",
                StrongTag s => $"<strong>{RenderInlineTags(s.Content)}</strong>",
                LinkTag l => $"<a href=\"{l.Url}\">{RenderInlineTags(l.Content)}</a>",
                _ => throw new NotSupportedException($"Unknown tag type: {tag.GetType()}")
            });
        }
        return sb.ToString();
    }
}