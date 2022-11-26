namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdCommentTag : Tag
    {
        public MdCommentTag() : base("\\")
        {
            
        }

        public override IToken ToHtml() => new TextToken("\\");

        public override bool IsValidTag(string data, int position) => true;
    }
}