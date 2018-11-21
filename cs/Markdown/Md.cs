using Markdown.Languages;
using Markdown.Tokenizing;
using Markdown.Translating;

namespace Markdown
{
    public class Md
    {
        public static string Render(string markdownSource)
        {
            var tokens = MarkdownTokenizer.Tokenize(markdownSource);
            return new Translator(new HtmlLanguage()).Translate(tokens);
        }
    }
}
