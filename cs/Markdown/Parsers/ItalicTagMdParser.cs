
namespace Markdown.Parsers
{
    public class ItalicTagMdParser : BaseParser
    {
        public static readonly Tag Tag = MdTags.Italic;

        public ItalicTagMdParser() : base(Tag)
        {
        }

        /// <summary>
        /// Метод, который проверяет наличие курсивного текста. В случае успеха возвращает токен, иначе null
        /// </summary>
        /// <param name="position">Позиция начала предполагаемого тега</param>
        /// <param name="text">Текст, в котором проверяется наличие курсива</param>
        /// <returns>Token, если в тексте курсив; null - если другой тег</returns>
        public override Token TryParseTag(int position, string text)
        {
            if (!HasThisTagOpening(position, text)) return null;
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
                        i += 2;
                        continue;
                }
                state = GetState(text[i], state);
                if (state != Tag.Close.Length) continue;
                return IsEmptySelection(position, i) || IsIntoNumber(position, i, text)
                    ? null
                    : new Token(position, i - position + 1, Tag, TextType.Italic);
            }

            return null;
        }
    }
}