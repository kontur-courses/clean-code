using Markdown.Parser.Inline.Rules;
using Markdown.Parser.Inline.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.TokenRules
{
    public class EscapeRule : ITokenRule
    {
        public bool CanHandle(char c) => c == '\\';

        public int Handle(string text, int position, List<Token> tokens, StringBuilder buffer)
        {
            return Escaper.HandleEscape(text, position, buffer);
        }
    }
}
