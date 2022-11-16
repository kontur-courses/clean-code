using System.Collections;
using System.Linq.Expressions;

namespace Markdown
{
    public class ModifierToken : Token
    {
        public ConcType type;

        public ModifierToken(ConcType type, int startIndex)
        {
            this.type = type;
            length = (int) type;
            this.startIndex = startIndex;
        }
    }
}
