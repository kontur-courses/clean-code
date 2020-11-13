using System;

namespace Markdown
{
    public class MdItalicToken : MdToken
    {
        public override string Parse(MdTokenReader reader) => $"<em>{ParseSubtokens(reader)}</em>";
        
        public static bool TryRead(MdTokenReader reader, out MdToken result)
        {
            throw new NotImplementedException();
        }
    }
}