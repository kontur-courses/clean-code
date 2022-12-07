namespace Markdown
{
    public class Md
    {
        public string Render(string text)
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
