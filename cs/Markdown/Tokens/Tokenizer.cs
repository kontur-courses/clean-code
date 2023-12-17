using Markdown.Contracts;
using Markdown.Extensions;
using Markdown.Tags;
using System.Text;

namespace Markdown.Tokens;

public static class Tokenizer
{
    private static readonly Dictionary<string, Func<ITag>> tagTemplates = new()
    {
        { "__", () => new BoldTag() },
        { "_", () => new ItalicTag() },
        { "# ", () => new HeaderTag() },
        { "\\n", () => new NewlineTag() }
    };

    public static List<Token> CollectTokens(string text)
    {
        var tokens = new List<Token>();
        var collector = new StringBuilder();

        text = " " + text + " ";

        for (var i = 1; i < text.Length - 1; i++)
        {
            if (text[i + 1].IsEscapedBy(text[i]))
            {
                collector.Append(text[i + 1]);
                i += 1;

                continue;
            }

            var tagToken = TryGetTagTokenOnPosition(i, text);

            if (tagToken != null)
            {
                if (collector.Length > 0)
                {
                    tokens.Add(new Token(text: collector.ToString()));
                    collector.Clear();
                }

                tokens.Add(tagToken);

                if (tokens.Count != 1)
                {
                    var previous = tokens[^2];

                    if (previous.Tag != null && previous.Tag.Type == tagToken.Tag!.Type)
                        // Filters cases: ____ or __ 
                        if (previous.Tag.Type != TagType.Newline)
                        {
                            previous.Tag.Status = TagStatus.Broken;
                            tagToken.Tag.Status = TagStatus.Broken;
                        }
                }

                i += tagToken.Tag!.Info.GlobalMark.Length - 1;

                continue;
            }

            collector.Append(text[i]);
        }

        if (collector.Length > 0)
            tokens.Add(new Token(text: collector.ToString()));

        // Bottom sentinel for headers.
        tokens.Add(new Token(new NewlineTag()));

        var tagTokens = tokens
            .Where(token => token.Tag != null)
            .ToList();

        tagTokens.DetermineTagStatuses();
        tagTokens.FilterIntersections();

        return tokens;
    }

    private static Token? TryGetTagTokenOnPosition(int position, string text)
    {
        Token? foundToken = null;

        var prefix = string.Concat(text[position], text[position + 1]);
        var foundTag = GetInstanceViaMark(prefix);

        if (foundTag == null)
            return foundToken;

        var context = new ContextInfo(position, text);

        foundTag.Context = context;
        foundToken = new Token(foundTag);

        return foundToken;
    }

    private static ITag? GetInstanceViaMark(string mark)
    {
        foreach (var tagMark in tagTemplates.Keys)
            if (mark.StartsWith(tagMark))
                return tagTemplates[tagMark]();

        return null;
    }
}