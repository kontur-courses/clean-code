using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class MdToken
    {
        public int StartPosition;
        public int Length;
        public List<MdToken> SubTokens = new List<MdToken>();

        public abstract string Parse(MdTokenReader reader);
        public string ParseSubtokens(MdTokenReader reader) => string.Concat(SubTokens.Select(t => t.Parse(reader)));
    }
}