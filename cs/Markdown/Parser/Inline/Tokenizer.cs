using Markdown.Parser.Inline.Rules;
using Markdown.Parser.Inline.TokenRules;
using Markdown.Parser.Inline.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Parser.Inline
{
    public static class Tokenizer
    {
        private static readonly List<ITokenRule> _rules = new()
        {
            new EscapeRule(),
            new UnderscoreRule(),
            new LinkRule()
        };

        public static List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var buffer = new StringBuilder();
            var index = 0;

            while (index < text.Length)
            {
                var rule = _rules.FirstOrDefault(r => r.CanHandle(text[index]));

                if (rule != null)
                {
                    index += rule.Handle(text, index, tokens, buffer);
                }
                else
                {
                    buffer.Append(text[index]);
                    index++;
                }
            }

            AddTextToken(tokens, buffer, text.Length);
            return tokens;
        }

        public static void AddTextToken(List<Token> tokens, StringBuilder buffer, int currentIndex)
        {
            if (buffer.Length == 0) return;
            tokens.Add(new TextToken(buffer.ToString(), currentIndex - buffer.Length));
            buffer.Clear();
        }
    }
}