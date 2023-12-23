using System.Diagnostics;
using System.Text;
using Markdown.Extensions;

namespace Markdown;

public class Md
{
    private static readonly List<ITag> Tags = new();

    static Md()
    {
        Tags.Add(new BoldTag());
        Tags.Add(new HeaderTag());
        Tags.Add(new ItalicTag());
    }
    
    public string Render(string source)
    {
        var tokenizer = new MdTokenizer(source, Tags);
        return TranslateToHtml(tokenizer.Tokenize().HandleTokens());
    }

    private string TranslateToHtml(IEnumerable<IToken> tokens)
    {
        return string.Concat(tokens.Select(x => x.Value));
    }
}