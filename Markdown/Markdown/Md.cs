using System.Diagnostics;
using System.Text;

namespace Markdown;

public class Md
{
    public string Render(string source)
    {
        var tokenizer = new MdTokenizer(source);
        return TranslateToHtml(tokenizer.Tokenize());
    }

    private string TranslateToHtml(IEnumerable<IToken> tokens)
    {
        return string.Concat(tokens.Select(x => x.GetValue));
    }
}