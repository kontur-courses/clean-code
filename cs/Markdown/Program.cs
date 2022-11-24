using System;
using System.IO;
using Markdown.Html;
using Markdown.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Markdown
{
    class Program
    {
        private static readonly string ProjectDirectory 
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ITokenParser, HtmlTokenParser>();
            serviceCollection.AddSingleton<IBuilder, HtmlBuilder>();
            serviceCollection.AddSingleton<Renderer>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var markdownText = File.ReadAllText(string.Concat(ProjectDirectory, @"\test.md"));
            var renderer = serviceProvider.GetService<Renderer>();
            var htmlText = renderer?.Render(markdownText);
            
            Console.WriteLine(htmlText);
        }
    }
}