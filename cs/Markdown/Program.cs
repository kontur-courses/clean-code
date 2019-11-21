using Markdown.ConsoleUtils;

namespace Markdown
{
    public class Program
    {
        static void Main()
        {
            var markdown = new Markdown();
            var inputText = ConsoleAssistant.ReadTextFromFile(".md");

            var converter = ConsoleAssistant.SetupConverter();

            var renderedText = markdown.Render(inputText, converter);
            ConsoleAssistant.WriteTextToFile(renderedText, converter.FileTypeWithDot);
        }
    }
}