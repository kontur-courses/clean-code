using NUnit.Framework.Constraints;

namespace Markdown
{
    public class Word
    {
        public readonly int Length;
        public readonly int StartPosition;
        public readonly string Text;

        public Word(string text, int startPosition, int length)
        {
            Text = text;
            StartPosition = startPosition;
            Length = length;
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
            for (var i = StartPosition; i < StartPosition + Length; i++)
                if (char.IsDigit(Text[i]))
                    return true;
            return false;
        }

        public bool IsInside(string tag, int tagPosition)
        {
            return tagPosition > StartPosition
                   && tagPosition + tag.Length < StartPosition + Length;
        }
    }
}