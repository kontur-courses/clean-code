using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Markdown
{
    public static class HtmlWriter
    {
        private static readonly Dictionary<TextType, Tag> Tags = new Dictionary<TextType, Tag>()
        {
            { TextType.VirtualAllText, new Tag("", "")},
            { TextType.Default, new Tag("", "")},
            { TextType.Italic, new Tag("<em>", "</em>")},
            { TextType.Bold, new Tag("<strong>", "</strong>")},
            { TextType.Heading, new Tag("<h1>", "</h1>")},
            { TextType.Link, new Tag("<a href=", "</a>")},
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
            return rootToken.GetValue(ConvertTokenToHtmlTag, originalText);
        }
        
        private static Tag ConvertTokenToHtmlTag(Token token)
        {
            if (token.Type != TextType.Link) return Tags[token.Type] ?? Tags[TextType.Default];
            if (!(token is LinkToken linkToken)) return Tags[TextType.Default];
            
            var tagOpening = new StringBuilder();
            tagOpening.Append(Tags[token.Type].Open);
            tagOpening.Append($"\"{linkToken.Link}\">");
            return new Tag(tagOpening.ToString(), Tags[token.Type].Close);
        }
    }
}