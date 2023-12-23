using System.Text;

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

    public List<IToken> Tokenize()
    {
        var textBuilder = new StringBuilder();
        var tokens = new List<IToken>();
        for (var i = 0; i < text.Length;)
        {
            if (TryTokenizeFrom(i, out var token))
            {
                AddAndClear(textBuilder, tokens);

                if (token is MdTagToken tagToken)
                    tagToken.Context = (text.ElementAtOrDefault(i - 1), text.ElementAtOrDefault(i + token.Length));
                
                tokens.Add(token);
                i += token.Length;
            }
            else textBuilder.Append(text[i++]);
        }

        AddAndClear(textBuilder, tokens);
        tokens.Add(new MdEndOfTextToken());

        return tokens;
    }

    private void AddAndClear(StringBuilder textBuilder, List<IToken> tokens)
    {
        if (textBuilder.Length <= 0) return;
        tokens.Add(new MdTextToken(textBuilder.ToString()));
        textBuilder.Clear();
    }

    private bool TryTokenizeFrom(int position, out IToken token)
    {
        token = null;
        var tokenTemplate = registeredTokens
            .Where(pairs => text[position..].StartsWith(pairs.Key));
        
        if (tokenTemplate.Any())
        {
            var newToken = tokenTemplate.Last().Value.Invoke();
            token = newToken;
            return true;
        }
        
        return false;
    }
}