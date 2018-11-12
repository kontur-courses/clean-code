using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Parser
    {
        private TokenReader reader;
        private char[] FieldStartMarkers;
        public readonly string MdText;

        public Parser(string mdText, IEnumerable<ITokenType> tokenTypes)
        {
            reader = new TokenReader(mdText);
            MdText = mdText;
            FieldStartMarkers = tokenTypes.Select(tokenType => tokenType.GetMarker().First()).ToArray();
        }

        public Token[] GetTokens()
        {
            throw new NotImplementedException();
        }
    }
}