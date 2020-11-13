using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TextWorker
    {
        public readonly char EscapeChar;
        private readonly List<char> charsThatCanBeEscaped;

        public TextWorker(IEnumerable<char> charsThatCanBeEscaped, char escapeChar = '\\')
        {
            EscapeChar = escapeChar;
            this.charsThatCanBeEscaped = charsThatCanBeEscaped.ToList();
            this.charsThatCanBeEscaped.Add(EscapeChar);
        }

        public bool IsEscapedChar(string line, int indexOfCharToCheck)
        {
            var escapeCharCount = GoThroughText(line, indexOfCharToCheck, 0)
                .Skip(1)
                .TakeWhile(character => character == EscapeChar)
                .Count();

            return escapeCharCount != 0 && escapeCharCount % 2 != 0;
        }

        public bool IsThereDigit(string line, int firstIndex, int lastIndex)
        {
            return GoThroughText(line, firstIndex, lastIndex).Any(char.IsDigit);
        }

        public bool InTwoDifferentWords(string line, int firstIndex, int secondIndex)
        {
            return GoThroughText(line, firstIndex, secondIndex).Any(char.IsWhiteSpace);
        }

        public IEnumerable<char> GoThroughText(string line, int firstIndex, int lastIndex)
        {
            if (firstIndex < lastIndex)
                for (int i = firstIndex; i <= lastIndex; i++)
                    yield return line[i];
            else
                for (int i = firstIndex; i >= lastIndex; i--)
                    yield return line[i];
        }

        public bool IsStartOfWord(string line, int index)
        {
            return !char.IsWhiteSpace(line[index]) &&
                   (index == 0 || !char.IsLetterOrDigit(line[index - 1]));
        }

        public bool IsEndOfWord(string line, int index)
        {
            return !char.IsWhiteSpace(line[index]) &&
                   (index == line.Length - 1 || !char.IsLetterOrDigit(line[index + 1]));
        }

        public bool ThereAreNotOnlySpecialChars(string line, int firstIndex, int lastIndex)
        {
            return GoThroughText(line, firstIndex, lastIndex).Any(c =>
                !charsThatCanBeEscaped.Contains(c));
        }

        public string ReplaceCharsAt(string line, char replacingChar, params int[] indexes)
        {
            var builder = new StringBuilder(line);
            foreach (var index in indexes)
            {
                builder[index] = replacingChar;
            }

            return builder.ToString();
        }

        public string DeleteEscapeCharFromLine(string line)
        {
            var lineWithoutEscapeChar = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == EscapeChar && EscapingSomething(line, i))
                    i++;

                lineWithoutEscapeChar.Append(line[i]);
            }

            return lineWithoutEscapeChar.ToString();
        }

        private bool EscapingSomething(string line, int indexOfEscapeChar)
        {
            return indexOfEscapeChar < line.Length - 1 &&
                   charsThatCanBeEscaped.Any(escapedChar =>
                       line[indexOfEscapeChar + 1] == escapedChar);
        }
    }
}