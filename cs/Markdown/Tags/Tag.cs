using Markdown.TagValidator;

namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public TagType Type { get; }
        public string OpeningSubTag { get; }
        public string ClosingSubTag { get; }
        private readonly ITagValidator validator;

        public Tag(TagType type, string opening, string closing, ITagValidator validator)
        {
            Type = type;
            OpeningSubTag = opening;
            ClosingSubTag = closing;
            this.validator = validator;
        }

        public string GetSubTag(SubTagOrder order)
        {
            if (order == SubTagOrder.Opening)
                return OpeningSubTag;

            return ClosingSubTag;
        }

        public bool IsValid(string text, SubTagOrder order, int start)
        {
            return validator.IsValid(text, this, order, start);
        }
    }
}