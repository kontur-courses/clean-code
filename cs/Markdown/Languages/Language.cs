using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public abstract class Language
    {
        protected abstract Dictionary<Tag, string> OpeningTags { get; }
        protected abstract Dictionary<Tag, string> ClosingTags { get; }

        public int MaxTagLength => OpeningTags.Concat(ClosingTags).Max(pair => pair.Value.Length);

        public string ConvertOpeningTag(Tag tag)
        {
            return OpeningTags[tag];
        }

        public string ConvertClosingTag(Tag tag)
        {
            return ClosingTags[tag];
        }

        public bool TryParseOpeningTag(string code, out Tag tag)
        {
            tag = OpeningTags
                .FirstOrDefault(pair => pair.Value == code)
                .Key;

            return tag != default(Tag);  // нормально ли, что моя логика по сути зависит от дефолтного состояния (первый в списке enum)
        }

        public bool TryParseClosingTag(string code, out Tag tag)
        {
            tag = ClosingTags
                .FirstOrDefault(pair => pair.Value == code)
                .Key;

            return tag != default(Tag);
        }
    }
}