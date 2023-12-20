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

    private string TranslateToHtml(IEnumerable<MdToken> tokens)
    {
        var text = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Text != null)
                text.Append(token.Text);
            else
            {
                text.Append(token.Tag.HtmlTag.Open);
            }
        }

        return text.ToString();
    }
}