namespace Markdown
{
    public class MdToken
    {
        public string Content { get; }
        public string SpecialSymbol { get; }
        public string HTMLTag { get; }

        public MdToken(string content, string specialSymbol, string htmlTag)
        {
            Content = content;
            SpecialSymbol = specialSymbol;
            HTMLTag = htmlTag;
        }
    }
}