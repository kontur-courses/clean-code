using System.Collections.Generic;

namespace Markdown
{
    public static class HtmlWriter
    {
        private static readonly Dictionary<TextType, Tag> Tags = new Dictionary<TextType, Tag>()
        {
            { TextType.Default, new Tag("", "")},
            { TextType.Italic, new Tag("<em>", "</em>")},
            { TextType.Bold, new Tag("<strong>", "</strong>")},
            { TextType.Heading, new Tag("<h1>", "</h1>")},
        };
        
        /// <summary>
        /// Метод, который превращает строку с некоторым форматированием в строку с html форматированием
        /// </summary>
        /// <param name="rootToken">Корневой токен в дереве токенов,
        ///  которое хранит информацию о форматировании в тексте</param>
        /// <param name="originalText">Текст в некотором форматировании</param>
        /// <returns>Строка в html форматировании</returns>
        public static string CreateHtmlFromTokens(Token rootToken, string originalText)
        {
            return rootToken.GetValue(ConvertTextTypeToHtmlTag, originalText);
        }

        private static Tag ConvertTextTypeToHtmlTag(TextType textType)
        {
            return Tags.ContainsKey(textType)
                ? Tags[textType]
                : new Tag("", ""); 
        }
    }
}