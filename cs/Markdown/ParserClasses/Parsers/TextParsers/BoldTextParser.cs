using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses.Parsers.TextParsers
{
    class BoldTextParser
    {
        public Node Parse(TokenList tokens)
        {
            var boldPattern = new [] {"UNDERSCORE", "UNDERSCORE", "TEXT", "UNDERSCORE", "UNDERSCORE"};
            return !tokens.Peek(boldPattern) ? null : new Node("BOLD", tokens.GetThird().Value, 5);
        }
    }
}
