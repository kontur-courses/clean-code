    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System.Xml.Schema;
    using Markdown.TagValidator;

    namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public TagType Type { get; }
        public string OpeningSubTag { get; }
        public string ClosingSubTag { get; }
        public ITagValidator Validator { get; }

        public Tag(TagType type, string opening, string closing, ITagValidator validator)
        {
            Type = type;
            OpeningSubTag = opening;
            ClosingSubTag = closing;
            Validator = validator;
        }

        public string GetSubTag(SubTagOrder order)
        {
            if (order == SubTagOrder.Opening)
                return OpeningSubTag;

            return ClosingSubTag;
        }

        public bool IsValid(string text, SubTagOrder order, int start)
        {
            return Validator.IsValid(text, this, order, start);
        }
    }
}
