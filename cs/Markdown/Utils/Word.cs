using System.Linq;

namespace Markdown
{
    public class Word
    {
        public readonly int Length;
        public readonly int StartPosition;
        public readonly string Text;
        public readonly string Value;

        public Word(string text, int startPosition, int length)
        {
            Text = text;
            StartPosition = startPosition;
            Length = length;
            Value = Text.Substring(StartPosition, Length);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Word))
                return false;
            var otherWord = (Word) obj;
            return Text == otherWord.Text && StartPosition == otherWord.StartPosition;
        }

        public bool ContainsDigit()
        {
            return Value.Any(ch => char.IsDigit(ch));
        }

        public bool ContainsInside(string tag, int tagPosition)
        {
            return tagPosition > StartPosition
                   && StartPosition + Length > tagPosition + tag.Length
                   && Value.Substring(tagPosition - StartPosition + 1, tag.Length) != tag
                   && Text.Substring(tagPosition - StartPosition - 1, tag.Length) != tag;
        }
    }
}