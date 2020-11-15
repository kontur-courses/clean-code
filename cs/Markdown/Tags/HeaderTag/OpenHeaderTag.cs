namespace Markdown
{
    public class OpenHeaderTag : HeaderTag
    {
        public OpenHeaderTag( int index) : base("<h1>", index, 1)
        {
        }
    }
}