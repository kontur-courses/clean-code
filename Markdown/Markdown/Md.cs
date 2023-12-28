using Markdown.Tags;
using Markdown.Tags.MdTags;
using Markdown.Tokens;

namespace Markdown;

public static class Md
{
    private static readonly List<ITag> Tags = new();

    static Md()
    {
        Tags.Add(new BoldTag());
        Tags.Add(new HeaderTag());
        Tags.Add(new ItalicTag());
        Tags.Add(new UnorderedListTag());
    }
    
    public static string Render(string source)
    {
        var tokenizer = new MdTokenizer(source, Tags);
        return tokenizer.Tokenize().HandleTokens().TranslateToText();
    }

    private static string TranslateToText(this IEnumerable<IToken> tokens)
    {
        return string.Concat(tokens.Select(x => x.Value));
    }
}