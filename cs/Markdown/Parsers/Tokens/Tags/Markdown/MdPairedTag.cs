using System;
using System.Reflection.Metadata;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Extensions;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public abstract class MdPairedTag : PairedTag
    {
        public bool IntoWord { get; set; }
        protected MdPairedTag(TagPosition tagPosition, string data) : base(tagPosition, data)
        {

        }

        public override bool IsValidTag(string currentLine, int currentPosition)
        {
            return position == TagPosition.Start && currentLine.IsLetter(currentPosition)
                   || position == TagPosition.End && (currentPosition == text.Length
                                                      || currentLine.IsLetter(GetPreviousPosition(currentPosition))
                                                      || MdCommentTag.IsCommented(currentLine, currentPosition))
                   && (currentPosition == currentLine.Length || currentLine[currentPosition] != '_');
        }

        public void CheckInWord(string currentLine, int currentPosition)
        {
            IntoWord = currentLine.IsLetter(GetPreviousPosition(currentPosition))
                       && currentLine.IsLetter(currentPosition);
        }

        private int GetPreviousPosition(int currentPosition) => 
            currentPosition - text.Length - 1;
    }
}
