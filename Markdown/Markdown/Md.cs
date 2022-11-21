using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly List<TokenBase> tokensToCheck;

    public Md()
    {
        tokensToCheck = new List<TokenBase>
        {
            // Order matters
            new HeaderTokenBase(),
            new BoldTokenBase(),
            new ItalicTokenBase()
        };
    }

    public string Render(string text)
    {
        var result = new StringBuilder();

        var i = 0;
        var lines = MarkdownParser.Parse(text);
        foreach (var line in lines)
        {
            RenderLine(line, result);
            if (i++ != lines.Count - 1)
                result.Append('\n');
        }

        return result.ToString();
    }

    public StringBuilder RenderLine(List<TokenBase> tokens, StringBuilder? buffer = null)
    {
        return buffer;
    }
}