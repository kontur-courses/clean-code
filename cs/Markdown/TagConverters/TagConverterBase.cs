using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal abstract class TagConverterBase : ITagConverter
    {
        public virtual bool IsSingleTag { get; }
        protected abstract HashSet<string> TagInside { get; }

        public abstract string TagHtml { get; }
        public abstract string TagName { get; }

        public string OpenTag() => $@"<{TagHtml}>";
        public string CloseTag() => $@"<\{TagHtml}>";

        public virtual bool IsTag(string text, int pos) => !TextWithDigits(text, pos);

        public bool CanProcessTag(string tag) => TagInside.Contains(tag);
        public virtual StringBuilder Convert(StringBuilder tagsText, StringBuilder text, int start, int finish)
        {
            if (IsSingleTag)
            {
                var t = new StringBuilder();
                t.Append(tagsText);
                t.Append(TagName);
                return FormTags(t);
            }
            if (ResultIsMoreThenOneWord(tagsText))
                return GetResultWithWhiteSpace(tagsText, text, start, finish);
            return FormTags(tagsText);
        }

        public virtual bool CanOpen(StringBuilder text, int pos) =>
            pos < text.Length - TagName.Length && !char.IsWhiteSpace(text[pos + TagName.Length]);
        public virtual bool CanClose(StringBuilder text, int pos) =>
            !IsSingleTag && pos > 1 && !char.IsWhiteSpace(text[pos - 1]);

        protected internal StringBuilder FormTags(StringBuilder text)
        {
            var result = new StringBuilder(OpenTag());
            for (var i = TagName.Length; i < text.Length - TagName.Length; i++)
                result.Append(text[i]);
            result.Append(CloseTag());
            return result;
        }

        private bool PositionInCenterWord(StringBuilder text, int position) =>
                (position > 1 && !char.IsWhiteSpace(text[position - 1])) &&
                (position < text.Length - TagName.Length && !char.IsWhiteSpace(text[position + TagName.Length]));

        private bool TextWithDigits(string text, int position) =>
            (position > 1 && char.IsDigit(text[position - 1])) ||
            (position < text.Length - TagName.Length && char.IsDigit(text[position + TagName.Length]));

        private bool ResultIsMoreThenOneWord(StringBuilder result)
        {
            int i;
            for (i = 0; i < result.Length; i++)
                if (!char.IsWhiteSpace(result[i]))
                    break;
            for (; i < result.Length; i++)
                if (char.IsWhiteSpace(result[i]))
                    break;
            for (; i < result.Length; i++)
                if (!char.IsWhiteSpace(result[i]))
                    break;
            return i != result.Length;
        }

        private StringBuilder GetResultWithWhiteSpace(StringBuilder tagsText, StringBuilder text, int posStart, int posEnd)
        {
            if (PositionInCenterWord(text, posStart) || PositionInCenterWord(text, posEnd))
            {
                return tagsText;
            }
            return FormTags(tagsText);
        }
    }
}
