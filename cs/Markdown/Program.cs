using Markdown.Html;
using Markdown.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Markdown
{
    class Program
    {
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ITokenParser, HtmlTokenParser>();
            serviceCollection.AddSingleton<IBuilder, HtmlBuilder>();
            serviceCollection.AddSingleton<Renderer>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var markdownText = "";
            var renderer = serviceProvider.GetService<Renderer>();
            renderer?.Render(markdownText);
        }
    }
}