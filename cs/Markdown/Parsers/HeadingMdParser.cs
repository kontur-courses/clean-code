namespace Markdown.Parsers
{
    public class HeadingMdParser : BaseParser
    {
        public static readonly Tag Tag = MdTags.Heading;
        
        public HeadingMdParser() : base(Tag)
        {
        }
        
        /// <summary>
        /// Метод, который проверяет наличие заголовка. В случае успеха возвращает токен, иначе null
        /// </summary>
        /// <param name="position">Позиция начала предполагаемого заголовка (Начинается с 0)</param>
        /// <param name="text">Текст, в котором проверяется наличие заголовка</param>
        /// <returns>Token, если присутствует заголовок; null - если другой тег</returns>
        public override Token TryHandleTag(int position, string text)
        {
            if (!HasThisToken(position, text)) return null;
            if (!IsFirstOnLine(position, text)) return null;
            var headingEnd = text.IndexOf('\n', position);
            var headingLength = headingEnd == -1
                ? text.Length - position
                : headingEnd - position;
            return new Token(position, headingLength, MdTags.Heading, TextType.Heading);
        }

        private static bool IsFirstOnLine(int position, string text)
        {
            if (position == 0) return true;
            return text[position - 1] == '\n';
        }
    }
}