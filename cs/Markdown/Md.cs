using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public static class Md
{
    private static readonly List<ITag> tags;

    static Md()
    {
        tags.Add(new BoldTag());
        tags.Add(new HeaderTag());
        tags.Add(new ItalicTag());
    }

    public static string Render(string text)
    {
        var tokenizer = new MdTokenizer(text, tags);
        return tokenizer.Tokenize().HandleTokens().TransformToText();
    }

    private static string TransformToText(this IEnumerable<IToken> tokens)
    {
        throw new NotImplementedException();
    }
}