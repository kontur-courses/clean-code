namespace Markdown
{
    public abstract class TagWithIndex : Tag
    {
        public int OpenTagIndex;
        public int CloseTagIndex;
    }
}