namespace Markdown
{
    public class Token
    {
        public bool HasNumber;
        public bool IsClose;
        public bool IsOpen;
        public bool IsWhiteSpace;
        public Tag PosibleTag;
        public string Value;

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