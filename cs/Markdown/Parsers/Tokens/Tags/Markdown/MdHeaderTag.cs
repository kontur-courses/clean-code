namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {

        }

        public override bool IsValidTag(string currentLine, int position)
        {
            return
                currentLine.Length > position && (char.IsWhiteSpace(currentLine, position) || currentLine.Length - 1 == position);
        }
    }
}
