using Markdown.Parser.Inline.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Rules
{
    public interface ITokenRule
    {
        bool CanHandle(char c);
        int Handle(string text, int position, List<Token> tokens, StringBuilder buffer);
    }
}