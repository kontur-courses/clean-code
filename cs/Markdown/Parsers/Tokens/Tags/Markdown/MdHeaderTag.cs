using Markdown.Extensions;
using System.Text.RegularExpressions;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdHeaderTag : Tag
    {
        public MdHeaderTag() : base("#")
        {

        }


        public override bool IsValidTag(string currentLine, int position)
        {
            return Regex.IsMatch(currentLine.Substring(0, position - Text.Length), @"\s*") 
                   && currentLine.IsWhiteSpaceIn(position);
        }
        
    }
}
