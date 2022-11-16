using System;
using System.Collections.Generic;

namespace Markdown
{
    public class HTMLWriter
    {
        public readonly Dictionary<TextType, Tag> Tags = new Dictionary<TextType, Tag>()
        {
            { TextType.Bold, new Tag("<strong>", "</strong>")},
            { TextType.Italic, new Tag("<i>", "</i>")},
            { TextType.Default, new Tag("", "")},
            { TextType.Heading, new Tag("<h1>", "<h1>")},
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
            // token.GetValue(type => tags[type])
            throw new NotImplementedException();
        }
    }
}