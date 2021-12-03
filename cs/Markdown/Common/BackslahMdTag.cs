using Markdown.Extensions;

namespace Markdown.Common
{
    public class BackslashMdTag : BaseMdTag
    {
        public BackslashMdTag()
            : base("\\")
        {
        }

        protected override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            return IsTag(text, startIndex) &&
                   IsTag(text, startIndex + 1) &&
                   startIndex - stopIndex == 2;
        }

        public override bool TryGetToken(string text, int startIndex, out Token token)
        {
            if (CanCreateToken(text, startIndex, startIndex + 2))
            {
                token = text.GetToken(startIndex, startIndex + 2, this);
                return true;
            }

            token = null;
            return false;
        }

        public override string RemoveMdTags(string value)
        {
            return value.Remove(0, MdTag.Length);
        }

        public override string InsertHtmlTags(string text)
        {
            return text;
        }
    }
}