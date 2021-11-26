using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkDownRenderer
    {
        private readonly string[] mdTags = { "#", "_", "__" };
        private readonly Tokenizer tokenizer = new();
        private readonly SemanticAnalyzer analyzer = new();

        public string Render(string text)
        {
            

            var rawTokens = tokenizer.Tokenize(text, mdTags).ToArray();

            var analyzedTokens =
                rawTokens.Select(tokens => analyzer.Analise(tokens)).ToArray();

            var translatedTokens = analyzedTokens.Select(Translator.Translate);

            var html = string.Join("\n", translatedTokens);

            return html;
        }
    }
}