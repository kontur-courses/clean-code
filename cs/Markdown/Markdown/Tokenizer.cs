using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        private string[] tags;
        private string text;
        public int CurrentPosition { get; private set; }
        private Stack<Token> tagStack;

        public Tokenizer(string text, string[] tags)
        {
            this.text = text;
            this.tags = tags;
            tagStack = new Stack<Token>();
        }

        public Token ReadTag()
        {
            var lastTag = tagStack.FirstOrDefault();
            var possibleTags = tags
                .Where(tag => text.Substring(CurrentPosition, tag.Length) == tag)
                .Where(tag => IsAfterWhiteSpace() && !IsInsideNumber())
                .ToList();

            if (!possibleTags.Any())
                return null;
            var longestTag = possibleTags
                .Aggregate("",
                    (cur, max) => cur.Length > max.Length ? cur : max);
            CurrentPosition += longestTag.Length;
            var tagToken = new Token(CurrentPosition, longestTag.Length);
            tagToken.Parent = tagStack.Peek();
            tagStack.Push(tagToken);

            return tagToken;
        }

        private string FindFirstPossibleTag()
        {
            return tags
                .Where(_ => !Escaped())
                .Where(possibleTag => CurrentPosition + possibleTag.Length < text.Length)
                .Where(possibleTag => text.Substring(CurrentPosition, possibleTag.Length) == possibleTag)
                .FirstOrDefault(possibleTag => IsAfterWhiteSpace() && !IsInsideNumber());
        }

        public Token ReadText()
        {
            var startingPosition = CurrentPosition;
            while (true)
            {
                if (CurrentPosition == text.Length)
                    break;
                var tag = FindFirstPossibleTag();
                if (tag != null)
                    break;
                CurrentPosition++;
            }

            return new Token(startingPosition, CurrentPosition - startingPosition);
        }

        private bool Escaped()
        {
            if (CurrentPosition == 0)
                return false;
            var i = CurrentPosition - 1;
            var slashesCount = 0;
            while (text[i] == '\\')
            {
                slashesCount++;
                i -= 1;
            }

            return slashesCount % 2 != 0;
        }

        private bool IsInsideNumber()
        {
            if (CurrentPosition == 0)
                return false;
            return char.IsDigit(text[CurrentPosition - 1]);
        }

        private bool IsAfterWhiteSpace()
        {
            if (CurrentPosition == 0)
                return false;
            return text[CurrentPosition - 1] == ' ';
        }

        public IEnumerable<Token> GetTokens()
        {
            if (text == "")
                yield return new Token(0, 0);
            while (CurrentPosition < text.Length)
            {
                var tagToken = ReadTag();
                var textToken = ReadText();
                if (tagToken != null)
                    yield return tagToken;
                yield return textToken;
            }
        }
    }
}