using System.Linq;

namespace Markdown
{
    public class EscapeTagHelper : TagHelper
    {
        public EscapeTagHelper()
            : base(@"\", "")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            var isAnyTagAfterEscapeSymbol = TagParser.SupportedTags.Values
                .Any(x => x.ParseForEscapeTag(position, text));
            if (IsTag(position, text) && isAnyTagAfterEscapeSymbol)
            {
                tag = new Tag(position, TagType.Escape, true, MdTag.Length, inWord, false);
                return true;
            }

            tag = null;
            return false;
        }

        public override int GetSymbolsCountToSkipForParsing()
        {
            return MdTag.Length + 1;
        }
    }
}