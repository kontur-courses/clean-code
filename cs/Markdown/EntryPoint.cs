using Markdown.Languages;
using Markdown.Tokenizing;

namespace Markdown
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var htmlSource = "hello <strong>people I'm <em>so</em> glad</strong> to see <strong>you</strong>";
            var markSource = "hello __people I'm _so_ glad__ to see __you__";

            var htmlRes = new Tokenizer(new HtmlLanguage()).Tokenize(htmlSource);
            var markRes = new Tokenizer(new MarkdownLanguage()).Tokenize(markSource);
        }
    }
}
