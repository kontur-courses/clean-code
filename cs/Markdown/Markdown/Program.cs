using Microsoft.Extensions.DependencyInjection;

namespace Markdown;

public static class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IMarkdownConverter, MarkdownToHtmlConverter>();
        services.AddSingleton<IMarkdownParser, MarkdownParser>();
        services.AddSingleton<TokenParser, BoldTokenParser>();
        services.AddSingleton<TokenParser, HeaderTokenParser>();
        services.AddSingleton<TokenParser, ItalicTokenParser>();
        services.AddSingleton<TokenParser, LinkTokenParser>();
        services.AddSingleton<HtmlTokenConverter, BoldTokenHtmlConverter>();
        services.AddSingleton<HtmlTokenConverter, HeaderTokenHtmlConverter>();
        services.AddSingleton<HtmlTokenConverter, ItalicTokenHtmlConverter>();
        services.AddSingleton<HtmlTokenConverter, TextTokenHtmlConverter>();
        services.AddSingleton<HtmlTokenConverter, LinkTokenHtmlConverter>();
        services.AddSingleton<Md>();

        var sp = services.BuildServiceProvider();
        var md = sp.GetService<Md>();
    }
}
