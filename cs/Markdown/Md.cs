using Markdown.BuilderNamespace;
using Markdown.HandlerNamespace;
using Markdown.TokenizerNamespace;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var tokenizer = new Tokenizer(text);
            var handler = new Handler();
            var builder = new Builder();

            var rawTokens = tokenizer.Tokenize();
            var handledTokens = handler.Handle(rawTokens);
            var build = builder.Build(handledTokens);

            return build;
        }
    }
}
