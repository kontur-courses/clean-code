using System;

namespace Markdown
{
    public class MdTokenReader
    {                
        public const string EscapeSymbol = @"\";
        public const string Em = "_";
        public const string Strong = "__";
        
        public IMdToken ReadField(string source, int startPosition)
        {
            throw new NotImplementedException();
        }
    }
}