using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public static class Md
{
    private static readonly List<ITag> Tags = [];

    static Md()
    {
        Tags.Add(new BoldTag());
        Tags.Add(new HeaderTag());
        Tags.Add(new ItalicTag());
    }

    public static string Render(string text)
    {
        var tokenizer = new MdTokenizer(text, Tags);
        return tokenizer.Tokenize().HandleTokens().TransformToText();
    }

    private static string TransformToText(this IEnumerable<IToken> tokens)
    {
        throw new NotImplementedException();
    }
}