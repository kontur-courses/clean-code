using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenDescription
    {
        public readonly TokenType tokenType;
        public readonly string tag;
        private readonly Func<string, int, bool> isOpening;
        private readonly Func<string, int, bool> isClosing;


        /* isOpening и isClosing принимают на вход строку и позицию в этой строке,
         * выдают необходимость открытия/закрытия нового токена соответственно */
        public TokenDescription(TokenType tokenType, string tokenTag, 
            Func<string, int, bool> isOpening, Func<string, int, bool> isClosing)
        {
            this.tokenType = tokenType;
            tag = tokenTag;
            this.isOpening = isOpening;
            this.isClosing = isClosing;
        }

        public bool IsOpening(string text, int position)
        {
            return isOpening(text, position);
        }

        public bool IsClosing(string text, int position)
        {
            return isClosing(text, position);
        }
    }
}
