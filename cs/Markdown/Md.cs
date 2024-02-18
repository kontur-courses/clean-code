
namespace Markdown;
public class Md
{
    public string Render(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException();

        var tokens = new Tokenizer.Tokenizer(str).Tokenize();
        var tokentree = new TokenTreeRenderer.TokenTreeRenderer().ConvertTokensToHtml(tokens);

        return string.Join("", tokentree);
    }
}