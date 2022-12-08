using System.Linq;
using Markdown.Extensions;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {
        }

        public override bool TryToValidate(MarkdownParsingLine context)
        {
            if (!IsValidTag(context.Line, context.CurrentPosition))
                return false;

            context.CurrentPosition++;
            return true;
        }

        protected override bool IsValidTag(string currentLine, int position)
        {
            int count = position - Text.Length;
            return (count == 0 || Enumerable.Range(0, count).Any(currentLine.IsWhiteSpaceIn))
                   && currentLine.IsWhiteSpaceIn(position);
        }
    }
}
