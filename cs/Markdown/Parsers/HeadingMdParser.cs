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
        public override Token TryParseTag(int position, string text)
        {
            if (!HasThisTagOpening(position, text)) return null;
            if (!IsFirstOnLine(position, text)) return null;
            var headingEnd = text.IndexOf('\n', position);
            var headingLength = headingEnd == -1
                ? text.Length - position
                : headingEnd - position;
            return IsEmptySelection(position, headingEnd)
                ? null
                : new Token(position, headingLength, Tag, TextType.Heading);
        }
    }
}