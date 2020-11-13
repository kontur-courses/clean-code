using System;

namespace Markdown
{
    public class MdHeaderToken : MdToken
    {
        public override string Parse(MdTokenReader reader) => $"<h1>{ParseSubtokens(reader)}</h1>";
        
        public static bool TryRead(MdTokenReader reader, out MdToken result)
        {
            throw new NotImplementedException();
        }
    }
}