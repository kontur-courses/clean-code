using Markdown.Parser.Inline.Rules;
using Markdown.Parser.Inline.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.TokenRules
{
    public class LinkRule : ITokenRule
    {
        public bool CanHandle(char c) => c == '[';

        public int Handle(string text, int position, List<Token> tokens, StringBuilder buffer)
        {
            var closeBracket = Escaper.GetNotEscapedClosingCharIndex(text, position + 1, ']');
            if (closeBracket == -1 || closeBracket + 1 >= text.Length || text[closeBracket + 1] != '(')
            {
                buffer.Append('[');
                return 1;
            }

            var closeParen = Escaper.GetNotEscapedClosingCharIndex(text, closeBracket + 2, ')');
            if (closeParen == -1)
            {
                buffer.Append('[');
                return 1;
            }

            Tokenizer.AddTextToken(tokens, buffer, position);

            var linkText = text.Substring(position + 1, closeBracket - position - 1);
            var rawUrl = text.Substring(closeBracket + 2, closeParen - closeBracket - 2);
            var cleanUrl = Escaper.HandleEscapeForWord(rawUrl);

            tokens.Add(new LinkToken(linkText, cleanUrl, position));

            return closeParen - position + 1;
        }
    }
}
