using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public List<Token> Value;
        public int StartPosition;
        public int Length;
        public string Tag;
        
        public string ConvertToHTMLTag()
        {
            throw new NotImplementedException();
        }
    }
}