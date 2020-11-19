using System.Linq;

namespace Markdown
{
    public class EscapeTagHelper : TagHelper
    {
        public EscapeTagHelper()
            : base(@"\", "")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false, int lineNumber = 0)
        {
            if (IsTag(position, text) && TagParser.SupportedTags.Values
                .Any(x => x.ParseForEscapeTag(position, text)))
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