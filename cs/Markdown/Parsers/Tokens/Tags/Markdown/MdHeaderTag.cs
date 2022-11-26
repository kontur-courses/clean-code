namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {

        }

        public override bool IsValidTag(string data, int position)
        {
            return
                data.Length > position && (char.IsWhiteSpace(data, position) || data.Length - 1 == position);
        }
    }
}
