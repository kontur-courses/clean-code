using static MarkdownTask.TagInfo;


namespace MarkdownTask
{
    public class ItalicTagParser : ITagParser
    {
        public ICollection<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            var opened = new Stack<Token>();
            var inTag = false;
            var startInMiddle = false;
            var hitSpace = false;

            if (text.Length <= 2)
                return tokens;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    opened = new Stack<Token>();
                    continue;
                }
                if (text[i] == '\\')
                {
                    i++;
                    continue;
                }

                if (text[i] == '_' && i < text.Length - 1 && text[i + 1] == '_')
                {
                    i++;
                    continue;
                }

                if (inTag && text[i] == ' ')
                    hitSpace = true;

                if (!inTag && IsOpeningTag(text, i))
                {
                    opened.Push(new Token(TagInfo.TagType.Italic, i, Tag.Open, 1));
                    inTag = true;
                    if (i == 0 || char.IsWhiteSpace(text[i - 1]) || text[i - 1] == '\\')
                        startInMiddle = false;
                    else
                        startInMiddle = true;
                    hitSpace = false;
                }
                else if (IsClosingTag(text, i))
                {
                    if (!opened.Any())
                        continue;

                    var o = opened.Pop();
                    if (!startInMiddle || !hitSpace)
                    {
                        tokens.Add(o);
                        tokens.Add(new Token(TagInfo.TagType.Italic, i, Tag.Close, 1));
                    }
                    inTag = false;
                    hitSpace = false;
                }
            }

            return tokens;
        }

        private static bool IsOpeningTag(string text, int pos)
        {
            if (text[pos] != '_')
                return false;

            if (pos < text.Length - 1 && text[pos + 1] == '_')
                return false;

            if (pos == 0)
                return true;

            if (pos < text.Length - 1 && char.IsDigit(text[pos + 1]))
                return false;

            if (char.IsLetter(text[pos - 1]) || char.IsWhiteSpace(text[pos - 1]) || text[pos - 1] == '\\')
                return true;

            return false;
        }
        private static bool IsClosingTag(string text, int pos)
        {
            if (text[pos] != '_')
                return false;

            if (pos < text.Length - 1 && text[pos + 1] == '_')
                return false;

            if (pos == text.Length - 1)
                return true;

            if (pos > 0 && (char.IsDigit(text[pos - 1]) || char.IsWhiteSpace(text[pos - 1])))
                return false;

            if (char.IsLetter(text[pos + 1]) || char.IsWhiteSpace(text[pos + 1]))
                return true;

            return false;
        }
    }
}