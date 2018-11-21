using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public  string Value;
        public bool IsWhiteSpace;
        public bool IsOpen;
        public bool IsClose;
        public bool HasNumber;
        public Tag PosibleTag;

        public Token()
        {
            Value = "";
            IsWhiteSpace = false;
            HasNumber = false;
            PosibleTag = null;
            IsOpen = false;
            IsClose = false;
        }
    }
}