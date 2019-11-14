using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDown.TokenDeclarations;

namespace MarkDown
{
    public class LineParser
    {
        private DeclarationGetter declarations;
        public LineParser(DeclarationGetter declarationGetter)
        {
            declarations = declarationGetter;
        }

        public string GetParsedLineFrom(string line) => GetParsedLineFrom(line, new List<TokenType>());

        public string GetParsedLineFrom(string line, List<TokenType> shilders)
        {
            var tokenParsers = declarations.GetTokenDeclarations();
            var parsedLine = new StringBuilder();
            var indexNextToLastToken = 0;
            for (var i = 0; i < line.Length; i++)
            {
                var tokens = tokenParsers
                    .Where(p => !p.TokenShielded(shilders))
                    .Select(p => p.GetToken(line, i))
                    .FirstOrDefault(t => t != null);
                if (tokens != null)
                {
                    parsedLine.Append(line.Substring(indexNextToLastToken, i - indexNextToLastToken));
                    shilders.Add(tokens.Type);
                    parsedLine.Append(tokens.Value);
                    indexNextToLastToken = tokens.indexNextToToken;
                    i = indexNextToLastToken;
                }
            }
            parsedLine.Append(line.Substring(indexNextToLastToken, line.Length - indexNextToLastToken));
            return parsedLine.ToString();
        }
    }
}
