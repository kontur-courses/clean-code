using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class TokenConverter
{
    public string TokensToString(IEnumerable<Token> tokens)
    {
        var htmlString = new StringBuilder();
        
        foreach (var token in tokens)
            htmlString.Append(token.MdString().ReplaceShieldSequences());

        return htmlString.ToString();
    }
}