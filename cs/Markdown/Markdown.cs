
namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "";

            var tagsFounder = new TextParser();
            var textTokens = tagsFounder.GetTextTokens(text);

            var htmlConverter = new HTMLConverter();
            var htmlString = htmlConverter.GetHTMLString(textTokens);

        }
    }
}
