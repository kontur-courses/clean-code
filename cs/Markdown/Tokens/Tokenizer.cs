using Markdown.Extensions;
using Markdown.Helpers;
using Markdown.Tags;
using System.Text;

namespace Markdown.Tokens;

public static class Tokenizer
{
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
                i += tagToken.Tag!.Info.GlobalMark.Length - 1;

                continue;
            }

            collector.Append(text[i]);
        }

        if (collector.Length > 0)
            tokens.Add(new Token(text: collector.ToString()));
        
        // 1-st filter
        tokens.AcceptBrokenFilter();
        
        // 2-nd filter
        tokens.SetTokenTypes();
        
        // 3-rd filter
        tokens.MarkOpenCloseTags();

        return tokens;
    }

    private static Token? TryGetTagTokenOnPosition(int position, string text)
    {
        Token? foundToken = null;
        
        var prefix = string.Concat(text[position], text[position + 1]);
        var foundTag = TagHelper.GetInstanceViaMark(prefix);

        if (foundTag == null) 
            return foundToken;
        
        var context = new ContextInfo(position, text);

        foundTag.Context = context;
        foundToken = new Token(foundTag);

        return foundToken;
    }
}