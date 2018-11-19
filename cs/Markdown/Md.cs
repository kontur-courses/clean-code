using Markdown.TextProcessing;

namespace Markdown
{
    public class Md
    {
        public string Render(string content)
        {
            var splitter = new TextSplitter(content);
            var tokens = splitter.SplitToTokens();
            var builder = new TextBuilder();
            var htmlCode = builder.BuildText(tokens);
            return htmlCode;
        }
    }
}
