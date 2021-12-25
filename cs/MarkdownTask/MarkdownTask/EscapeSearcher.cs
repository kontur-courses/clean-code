using System.Collections.Generic;

namespace MarkdownTask
{
    public class EscapeSearcher
    {
        private const char Slash = '\\';

        private readonly HashSet<char> escapingChars = new() { '\\', '_', '#' };

        private int currentPosition;

        public List<int> GetPositionOfEscapingSlashes(string mdText)
        {
            currentPosition = 0;
            var result = new List<int>();

            for (; currentPosition < mdText.Length; currentPosition++)
                if (mdText[currentPosition] == Slash && IsSlashEscapeSomething(mdText))
                {
                    result.Add(currentPosition);
                    currentPosition++;
                }

            return result;
        }

        private bool IsSlashEscapeSomething(string mdText)
        {
            return currentPosition + 1 < mdText.Length
                   && escapingChars.Contains(mdText[currentPosition + 1]);
        }
    }
}