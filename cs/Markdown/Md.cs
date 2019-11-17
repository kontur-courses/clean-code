using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        private const char EscapeCharacter = '\\';

        public readonly List<TokenDescription> MdTokenDescriptions = new List<TokenDescription>
        {
            new TokenDescription((text, position) => TokenReader.ReadEscapedSymbol(text, position, '\\')),
            new TokenDescription((text, position) => TokenReader.ReadSubstringToken(text, position, "_", TokenType.Underscore)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsWhiteSpace, TokenType.Whitespaces)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsLetter, TokenType.Letters)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsDigit, TokenType.Number))
        };

        public string Render(string markdownText)
        {
            var reader = new TokenReader(MdTokenDescriptions);
            var tokens = reader.SplitToTokens(markdownText);
            throw new NotImplementedException();
        }
    }
}
