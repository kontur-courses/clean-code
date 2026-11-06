using Markdown.Extensions;

namespace Markdown;

class Program
{
    static void Main(string[] args)
    {
        var markdownText = """
                           _курсив_
                           __полужирный__
                           #заголовок
                           """;
        
       var htmlText = Md.Render(markdownText);
       
       Console.WriteLine(htmlText);


       var htmlTextByExtensions = markdownText
           .ParseMarkdown()
           .RenderHtml();
       
       Console.WriteLine(htmlTextByExtensions);
    }
}