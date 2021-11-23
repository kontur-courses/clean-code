using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IMdParser
    {
        public IReadOnlyDictionary<string, Token> Tokens { get; }
        public IReadOnlyList<Token> Result { get; }
        public string TextToParse { get; }
        public IEnumerable<Token> ParseTokens(string textToParse);
        public void AddScreening(ScreeningToken token);
    }
}