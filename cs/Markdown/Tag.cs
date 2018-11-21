using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tag
    {
        public string Md { get; }
        public string Html { get; }
        public string[] IgnoringNestedTags { get; }
        public string CloserHtml => Html.Insert(1, "/");

        public Tag(
            string md,
            string html,
            IEnumerable<string> ignoringNestedTags = null)
        {
            Md = md;
            Html = html;
            IgnoringNestedTags = ignoringNestedTags == null ? new string[0] : ignoringNestedTags.ToArray();
        }

        private int GetLengthOfSpecialSymbols(string text, int position)
        {
            var result = 0;
            for (var i = position; i < text.Length; i++)
            {
                if (ContainsMdSymbols(text[i].ToString()))
                    result++;
                else
                    break;
            }
            return result;
        }

        public bool IsIgnoringMd(string md)
            => IgnoringNestedTags.Contains(md);

        public bool IsMd(string text, int position)
        {
            var mdLength = Md.Length;
            var specialSymbolsLength = GetLengthOfSpecialSymbols(text, position);
            if (mdLength != specialSymbolsLength)
                return false;
            var fragment = text.Substring(position, mdLength);
            return fragment.Equals(Md);
        }

        public bool ContainsMdSymbols(string symbols)
            => Md.Contains(symbols);

        public override bool Equals(object obj)
        {
            var otherTag = (Tag) obj;
            if (otherTag == null)
                return false;
            return Md.Equals(otherTag.Md) && Html.Equals(otherTag.Html);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Md.GetHashCode() << 15 + Html.GetHashCode();
            }
        }
    }
}
