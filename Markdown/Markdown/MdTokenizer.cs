using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class MdTokenizer
{
    private readonly SortedDictionary<string, Func<IToken>> registeredTokens = new();
    private readonly string text;

    public MdTokenizer(string text, IEnumerable<ITag> tags)
    {
        this.text = text;
        registeredTokens.Add("\\", () => new MdEscapeToken());
        registeredTokens.Add("\n", () => new MdNewlineToken());
        foreach (var tag in tags)
            registeredTokens.Add(tag.MdTag, () => new MdTagToken(tag));
    }

    public LinkedList<IToken> Tokenize()
    {
        var tokens = new LinkedList<IToken>();
        var lastTokenPosition = 0;
        for (var i = 0; i < text.Length;)
        {
            if (TryTokenizeFrom(i, out var token))
            {
                if (i - lastTokenPosition > 0)
                    tokens.AddLast(new MdTextToken(text[lastTokenPosition..i]));

                if (token is MdTagToken tagToken)
                    tagToken.SetContext(text.ElementAtOrDefault(i - 1), text.ElementAtOrDefault(i + token.Length));
                
                tokens.AddLast(token);
                lastTokenPosition = i += token.Length;
            }
            else i++;
        }

        if (text.Length - 1 - lastTokenPosition > 0)
            tokens.AddLast(new MdTextToken(text[lastTokenPosition..]));
        tokens.AddLast(new MdEndOfTextToken());

        return tokens;
    }

    private bool TryTokenizeFrom(int position, out IToken token)
    {
        token = null;
        var tokenTemplates = registeredTokens
            .Where(pairs => text[position..].StartsWith(pairs.Key)).ToArray();

        if (!tokenTemplates.Any()) return false;
        
        var newToken = tokenTemplates.Last().Value.Invoke();
        token = newToken;
        return true;

    }
}