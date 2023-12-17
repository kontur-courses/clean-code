using Markdown.Extensions;
using Markdown.Tags;
using System.Text;

namespace Markdown.Tokens;

public class Tokenizer
{
    private readonly MD markdownContext;

    public Tokenizer(MD markdownContext)
    {
        this.markdownContext = markdownContext;
    }

    public List<Token> CollectTokens(string text)
    {
        var tokens = new List<Token>();
        var collector = new StringBuilder();

        text = " " + text + "\\n ";

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
                i += tagToken.Tag!.Info.GlobalMark.Length - 1;

                continue;
            }

            collector.Append(text[i]);
        }

        if (collector.Length > 0)
            tokens.Add(new Token(text: collector.ToString()));

        var tagTokens = tokens
            .Where(token => token.Tag != null)
            .ToList();

        tagTokens.DetermineTagStatuses();
        tagTokens.FilterIntersections();
        tokens.FilterEmptyTags();

        return tokens;
    }

    private Token? TryGetTagTokenOnPosition(int position, string text)
    {
        Token? foundToken = null;

        var prefix = string.Concat(text[position], text[position + 1]);
        var foundTag = markdownContext.GetInstanceViaMark(prefix);

        if (foundTag == null)
            return foundToken;

        var context = new ContextInfo(position, text);

        foundTag.Context = context;
        foundToken = new Token(foundTag);

        return foundToken;
    }
}