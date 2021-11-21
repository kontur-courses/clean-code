using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class ParserContext
    {
        public readonly List<Token> Result;
        public readonly string TextToParse;
        public readonly Dictionary<string, Token> Tokens;

        public ParserContext(string textToParse)
        {
            Result = new List<Token>();
            TextToParse = textToParse;
            Tokens = new Dictionary<string, Token>();
        }
    }
}