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
            return position == data.Length - 1 ||
                   this.position == TagPosition.Start && data.Length > position && char.IsLetter(data[position]) ||
                   this.position == TagPosition.End && ((position - text.Length >=0 && char.IsLetter(data[position - text.Length])) ||
                                                        IntoWord == IntoWord);//TODO: проверить эти условия
        }
    }
}
