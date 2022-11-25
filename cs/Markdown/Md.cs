using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        private IConverter _converter;

        public Md(IConverter converter)
        {
            _converter = converter;
        }
        public string Render(string markdown)
        {
            if (markdown == null)
                throw new ArgumentNullException();

            var lines = markdown.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                var tagCondition = new TagCondition();
                var tokenTyper = new TokenTyper(lines[i], tagCondition);
                var tokenBuilder = new TokenBuilder(tagCondition);
                var tokenSetter = new TokenSetter(tokenBuilder, tagCondition);

                var tokenizer = new Tokenizer<TokenType>(lines[i], tokenSetter, tokenTyper);
                var tokens = tokenizer.TokenizeLine();
                lines[i] = _converter.ConvertTokens(tokens);
            }

            return string.Join('\n', lines);
        }
    }
}
