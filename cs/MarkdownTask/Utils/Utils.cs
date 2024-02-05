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

        public static bool IsAfterNonSpace(string text, int position)
        {
            if (position < 0 || position > text.Length)
            {
                return false;
            }

            return position == 0 || !char.IsWhiteSpace(text[position - 1]);
        }

        public static bool IsBeforeNonSpace(string text, int position)
        {
            if (position < 0 || position >= text.Length)
            {
                return false;
            }

            return position == text.Length - 1 || !char.IsWhiteSpace(text[position + 1]);
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



        public static bool CanSelect(string text, int start, int end)
        {
            if (end - start <= 1)
            {
                return true;
            }

            var spacePosition = text.IndexOf(' ', start, end - start);

            var a = IsAfterNonSpace(text, start);
            var b = IsBeforeNonSpace(text, end);


            return spacePosition < 0
                || (IsAfterSpace(text, start) && IsBeforeSpace(text, end));
        }
    }
}
