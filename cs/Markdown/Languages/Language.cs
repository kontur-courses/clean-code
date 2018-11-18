using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public class Language
    {
        public ReadOnlyDictionary<Tag, string> OpeningTags => new ReadOnlyDictionary<Tag, string>(openingTags);
        public ReadOnlyDictionary<Tag, string> ClosingTags => new ReadOnlyDictionary<Tag, string>(closingTags);

        private Dictionary<Tag, string> openingTags;
        private Dictionary<Tag, string> closingTags;

        public int MaxTagLength => openingTags.Concat(closingTags).Max(pair => pair.Value.Length);

        public Language()
        {
            openingTags = new Dictionary<Tag, string>();
            closingTags = new Dictionary<Tag, string>();
        }

        protected void AddTag(Tag tag, string openingTag, string closingTag)
        {
            openingTags.Add(tag, openingTag);
            closingTags.Add(tag, closingTag);
        }

        //public string ConvertOpeningTag(Tag tag)
        //{
        //    return openingTags[tag];
        //}

        //public string ConvertClosingTag(Tag tag)
        //{
        //    return closingTags[tag];
        //}

        //public bool TryParseOpeningTag(string code, out Tag tag)
        //{
        //    tag = openingTags
        //        .FirstOrDefault(pair => pair.Value == code)
        //        .Key;

        //    return tag != default(Tag);  // нормально ли, что моя логика по сути зависит от дефолтного состояния (первый в списке enum)
        //}

        //public bool TryParseClosingTag(string code, out Tag tag)
        //{
        //    tag = closingTags
        //        .FirstOrDefault(pair => pair.Value == code)
        //        .Key;

        //    return tag != default(Tag);
        //}
    }
}