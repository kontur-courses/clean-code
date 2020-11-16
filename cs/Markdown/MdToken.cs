using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class MdToken
    {
        public int StartPosition;
        public int Length;
        public MdToken ParentToken;
    }

    public abstract class MdTokenWithSubTokens : MdToken
    {
        public List<MdToken> SubTokens = new List<MdToken>();
    }
}