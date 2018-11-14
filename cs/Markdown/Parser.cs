using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace Markdown
{
    public class Parser
    {
        private readonly TokenReader reader;
        private readonly HashSet<char> fieldStartMarkers;
        private readonly ITokenType[] possibleTokenTypes;
        private ITokenType currentTokenType; //потом я, наверное, сделаю это стеком для работы с вложенными тегами
        public readonly string MdText;

        public Parser(string mdText, ITokenType[] tokenTypes)
        {
            reader = new TokenReader(mdText);
            MdText = mdText;
            fieldStartMarkers = new HashSet<char>(tokenTypes.Select(tokenType => tokenType.GetMarker().First()));
            possibleTokenTypes = tokenTypes.ToArray();
        }

        public Token[] GetTokens()
        {
            var tokens = new List<Token>();
            while (reader.Position < MdText.Length - 1)
            {
                var previousType = currentTokenType;
                var token = currentTokenType == null
                    ? reader.ReadUntil(CheckIfOpenSymbol)
                    : reader.ReadUntil(CheckIfClosingSymbol);
                reader.Skip(1);
                token.Type = previousType;
                tokens.Add(token);
            }

            return tokens.ToArray();
        }

        private bool CheckIfOpenSymbol(char ch)
        {
            if (!fieldStartMarkers.Contains(ch))
                return false;
            foreach (var tokenType in possibleTokenTypes)
            {
                var left = reader.Position == 0 ? ' ' : MdText[reader.Position - 1];
                var right = reader.Position == MdText.Length - 1 ? ' ' : MdText[reader.Position + 1];
                if (!tokenType.CheckIfOpen(ch, left, right))
                    continue;
                currentTokenType = tokenType;
                return true;
            }

            return false;
        }

        private bool CheckIfClosingSymbol(char ch)
        {
            var left = reader.Position == 0 ? ' ' : MdText[reader.Position - 1];
            var right = reader.Position == MdText.Length - 1 ? ' ' : MdText[reader.Position + 1];
            var returnValue = currentTokenType.CheckIfClosing(ch, left, right);
            if (returnValue)
                currentTokenType = null;
            return returnValue;
        }
    }
}