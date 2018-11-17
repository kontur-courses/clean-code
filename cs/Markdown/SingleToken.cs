using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class SingleToken
    {
        public SingleToken(TokenType tokenType, TokenPosition tokenPosition, LocationType locationType)
        {
            TokenType = tokenType;
            TokenPosition = tokenPosition;
            LocationType = locationType;
        }

        public TokenType TokenType { get; }
        public TokenPosition TokenPosition { get; }
        public LocationType LocationType { get; }
    }
}
