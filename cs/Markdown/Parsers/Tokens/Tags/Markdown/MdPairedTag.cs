using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public abstract class MdPairedTag : PairedTag
    {
        public bool IntoWord { get; set; }
        protected MdPairedTag(TagPosition position, string data) : base(position, data)
        {

        }

        public override bool IsValidTag(string data, int position)
        {
            return this.position == TagPosition.End ||
                   position == data.Length - 1;
        }
    }
}
