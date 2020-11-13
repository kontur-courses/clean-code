using System;

namespace Markdown
{
    public class MdBoldToken : MdToken
    {
        public override string Parse(MdTokenReader reader) => $"<strong>{ParseSubtokens(reader)}</strong>";
        
        public static bool TryRead(MdTokenReader reader, out MdToken result)
        {
            throw new NotImplementedException();
        }
    }
}