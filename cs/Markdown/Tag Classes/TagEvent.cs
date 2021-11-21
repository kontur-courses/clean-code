namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public Side Side;
        public Mark Mark;
        public string TagContent;

        public TagEvent(Side side, Mark mark, string con)
        {
            Side = side;
            Mark = mark;
            TagContent = con;
        }

        public override string ToString()
        {
            return $"TagContent = {TagContent}\n";
        }
    }
}
