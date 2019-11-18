namespace Markdown
{
    public abstract class Tag
    {
        public string StringRepr;
        public int Length;
        public static bool isOpen = true;
        
        public Tag(string str)
        {
            this.StringRepr = str;
            this.Length = str.Length;
        }
    }
}