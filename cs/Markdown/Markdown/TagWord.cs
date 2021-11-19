namespace Markdown
{
    public class TagWord : IToken
    {
        public string Content
        { get; set; }

        public bool IsNotToPairToken => false;

        public TagWord(string word)
        {
            Content = word;
        }
    }
}
