namespace Markdown;

using Tokenizing;
using Parsing;
using Rendering;

public class Md
{
    public static string Render(string textToRender)
    {
        var tokens = Tokenizer.Tokenize(textToRender);
        var document = Parser.Parse(tokens);
        return HtmlRenderer.Render(document);
    }
}