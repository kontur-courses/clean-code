using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public string Md { get; }
        public string Html { get; }
        public string[] IgnoringNestedTags { get; }
        public string OpenerHtml => $"<{Html}>";
        public string CloserHtml => $"</{Html}>";
        public char[] NeutralizingSymbols;

        public Tag(
            string md,
            string html,
            IEnumerable<char> neutralizingSymbols = null,
            IEnumerable<string> ignoringNestedTags = null)
        {
            Md = md;
            Html = html;
            NeutralizingSymbols = neutralizingSymbols == null ? new char[0] : neutralizingSymbols.ToArray();
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

        public bool HaveNeutralizingSymbolsAround(StringBuilder text, int position)
        {
            if (position == 0)
                return HasNeutralizingAfter(text, position);
            if (position == text.Length - 1)
                return HasNeutralizingBefore(text, position);
            return HasNeutralizingBefore(text, position) || HasNeutralizingAfter(text, position);
        }

        private bool HasNeutralizingBefore(StringBuilder text, int position)
            => NeutralizingSymbols.Contains(text[position - 1]);

        private bool HasNeutralizingAfter(StringBuilder text, int position)
            => NeutralizingSymbols.Contains(text[position + 1]);

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
