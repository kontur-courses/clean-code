using static MarkdownTask.PairedTagsParser;

namespace MarkdownTask
{
    public static class Utils
    {
        private const char escapingChar = '\\';

        public static bool IsEscaped(string text, int position)
        {
            if (position <= 0 || position >= text.Length)
            {
                return false;
            }

            int escapingCharCount = 0;

            while (position > 0)
            {
                if (text[position - 1] == escapingChar)
                {
                    escapingCharCount++;
                    position--;
                }
                else
                {
                    break;
                }
            }

            return escapingCharCount % 2 != 0;
        }

        public static bool IsAfterSpace(string text, int position)
        {
            if (position < 0 || position > text.Length)
            {
                return false;
            }

            return position == 0 || char.IsWhiteSpace(text[position - 1]);
        }

        public static bool IsBeforeSpace(string text, int position)
        {
            if (position < 0 || position >= text.Length)
            {
                return false;
            }

            return position == text.Length - 1 || char.IsWhiteSpace(text[position + 1]);
        }

        public static bool CanSelect(string text, Candidate open, Candidate close)
        {
            if (close.position - open.position == 1)
            {
                return true;
            }

            if (open.edgeType == EdgeType.EDGE || close.edgeType == EdgeType.EDGE)
            {
                return true;
            }

            var substring = text.Substring(open.position, close.position - open.position);

            var withNumber = IsWordWithDigits(substring);
            var oneWord = substring.Split(' ').Count() == 1;

            return !withNumber && oneWord;
        }

        private static bool IsWordWithDigits(string word)
        {
            return word.Where(x => char.IsDigit(x)).Any();
        }
    }
}
