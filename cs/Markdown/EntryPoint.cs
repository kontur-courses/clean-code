using Markdown.Languages;
using Markdown.Tokenizing;
using Markdown.Translating;

namespace Markdown
{
    public class EntryPoint
    {
        public static string TranslateMarkdownToHtml(string markdownSource)
        {
            var tokens = MarkdownTokenizer.Tokenize(markdownSource);
            return new Translator(new HtmlLanguage()).Translate(tokens);
        }

        public static void Main()
        {

        }
    }
}
