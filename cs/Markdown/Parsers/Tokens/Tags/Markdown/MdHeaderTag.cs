using Markdown.Extensions;
using System.Text.RegularExpressions;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {
        }

        public override bool IsValidTag(MdParsingLine context)
        {
            if (!IsValidTag(context.Line, context.CurrentPosition))
                return false;
            else
            {
                context.CurrentPosition++;
                return true;
            }
        }

        protected override bool IsValidTag(string currentLine, int position)
        {
            return Regex.IsMatch(currentLine.Substring(0, position - Text.Length), @"\s*")
                   && currentLine.IsWhiteSpaceIn(position);
        }
    }
}
