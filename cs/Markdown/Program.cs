using Markdown.Renderers;
using Markdown.Tokenizers;

namespace Markdown;

public static class Program
{
    public static void Main()
    {
        var input = "Подчерки внутри текста c цифрами_12_3 ";
        var md = Md.Create(new Tokenizer(), new HtmlTokenConverter()).Render(input);
        Console.WriteLine(md);
    }
}