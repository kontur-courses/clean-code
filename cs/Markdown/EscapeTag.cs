using System.Linq;

namespace Markdown
{
    public class EscapeTag : Tag
    {
        private EscapeTag(int position)
            : base(@"\", "", position, true)
        {
        }

        public override bool TryParse(int position, string text, out Tag tag)
        {
            if (IsTag(position, text) && TagParser.SupportedTags
                .Any(x => x.ParseForEscapeTag(position, text)))
            {
                tag = new EscapeTag(position);
                return true;
            }

            tag = null;
            return false;
        }

        public static Tag CreateInstance()
        {
            return new EscapeTag(0);
        }

        public override int GetMdTagLengthToSkip()
        {
            return MdTag.Length + 1;
        }
    }
}