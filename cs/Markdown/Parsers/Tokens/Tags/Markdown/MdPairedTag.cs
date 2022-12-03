using System;
using System.Reflection.Metadata;
using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public abstract class MdPairedTag : PairedTag
    {
        public bool IntoWord { get; set; }
        protected MdPairedTag(TagPosition tagPosition, string data) : base(tagPosition, data)
        {

        }

        public override bool IsValidTag(string data, int position)
        {
            return position == data.Length - 1 ||
                   this.position == TagPosition.Start && data.Length > position && char.IsLetter(data[position]) ||
                   this.position == TagPosition.End && ((position - text.Length >=0 && char.IsLetter(data[position - text.Length])) ||
                                                        IntoWord == IntoWord);//TODO: проверить эти условия
        }

        public void CheckInWord(string currentLine, int currentPosition)
        {
            var previousPosition = currentPosition - text.Length - 1;
            IntoWord = previousPosition >= 0
                       && char.IsLetter(currentLine[previousPosition])
                       && currentPosition < currentLine.Length
                       && char.IsLetter(currentLine[currentPosition]);
        }
    }
}
