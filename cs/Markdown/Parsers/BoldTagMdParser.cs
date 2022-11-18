using Markdown;
using Markdown.Parsers;

namespace Markdown.Handlers
{
    public class BoldTagMdParser : BaseParser
    {
        public static readonly Tag Tag = MdTags.Bold;

        public BoldTagMdParser() : base(Tag)
        {
        }

        /// <summary>
        /// Метод, который проверяет наличие жирного текста. В случае успеха возвращает токен, иначе null
        /// </summary>
        /// <param name="position">Позиция начала предполагаемого тега (Начинается с 0)</param>
        /// <param name="text">Текст, в котором проверяется наличие жирности</param>
        /// <returns>Token, если текст жирный; null - если другой тег</returns>
        public override Token TryHandleTag(int position, string text)
        {
            if (!HasThisToken(position, text)) return null;
            if (NextCharAfterTag_IsSpaceOrEndLine(position, text, Tag.Open)) return null;
            
            var openInWord = IsOpenInWord(position, text);
            var state = 0;
            for (var i = position + Tag.Open.Length; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                    case ' ' when openInWord:
                        return null;
                    case '\\':
                    case ' ':
                        i++;
                        continue;
                }
                state = GetState(text[i], state);
                if (state != Tag.Close.Length) continue;
                return IsEmptySelection(position, i) || IsIntoNumber(position, i, text)
                    ? null
                    : new Token(position, i - position + 1, Tag, TextType.Bold);
            }

            return null;
        }
    }
}