namespace Markdown
{
    public class TagSpace : IToken
    {
        public string Content => " ";

        public bool IsNotToPairToken => false;
    }
}
