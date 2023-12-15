using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Processor;

public class Processor
{
    private readonly string text;
    private readonly ISyntax syntax;

    public Processor(string text, ISyntax syntax)
    {
        this.text = text;
        this.syntax = syntax;
    }

    public IList<IToken> ParseTags()
    {
        var tags = FindAllTags();
        tags = RemoveEscapedTags(tags);
        tags = RemoveNonPairTags(tags);
        return GetValidTags(tags);
    }

    private IList<IToken> FindAllTags()
    {
        var tags = new List<IToken>();
        var i = 0;
        var skipCycle = false;
        while (i < text.Length)
        {
            if (skipCycle)
            {
                skipCycle = false;
                i++;
            }
            
            if (text[i] == '#' && TryParseSharp(i, out var sharp))
            {
                tags.Add(sharp);
            } else if (text[i] == '_' && TryParseDoubleUnderline(i, out var doubleUnderline))
            {
                tags.Add(doubleUnderline);
                skipCycle = true;
            } else if (text[i] == '_' && TryParseUnderline(i, out var underline))
            {
                tags.Add(underline);
            }

            i++;
        }

        return tags;
    }

    private IList<IToken> RemoveEscapedTags(IList<IToken> tags)
    {
        return tags;
    }

    private IList<IToken> RemoveNonPairTags(IList<IToken> tags)
    {
        return tags;
    }

    private IList<IToken> GetValidTags(IList<IToken> tags)
    {
        return tags;
    }

    private bool TryParseSharp(int position, out IToken token)
    {
        if (position == 0 || text[position - 1] == '\n')
        {
            token = new MarkdownToken(position, syntax.GetTagType(text[position].ToString()), 1);
            return true;
        }

        token = null;
        return false;
    }

    private bool TryParseUnderline(int position, out IToken token)
    {
        token = new MarkdownToken(position, syntax.GetTagType(text[position].ToString()), 1);
        return true;
    }

    private bool TryParseDoubleUnderline(int position, out IToken token)
    {
        if (position < text.Length - 1 && text[position + 1] == '_')
        {
            token = new MarkdownToken(position, syntax.GetTagType("__"), 2);
            return true;
        }

        token = null;
        return false;
    }
}