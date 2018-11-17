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
            this.tokenPosition = tokenPosition;
            LocationType = locationType;
        }

        public TokenType TokenType { get; }
        private readonly TokenPosition tokenPosition;
        public LocationType LocationType { get; }

        public int GetPosition()
        {
            return LocationType == LocationType.Opening ? tokenPosition.Start :
                LocationType == LocationType.Closing ? tokenPosition.End :
                throw new InvalidOperationException();
        }
    }
}
