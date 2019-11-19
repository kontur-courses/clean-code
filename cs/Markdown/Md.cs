using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Md
    {
        private readonly List<Tag> Tags;

        public Md()
        {
            Tags = new List<Tag>()
            {
                new Tag("_", true, true, false, "<em>", "</em>"),
                new Tag("__", true, true, true, "<strong>", "</strong>")
            };
        }

        //Получает на вход текст в формате Markdown, создаёт экземпляр парсера и передаёт их методу ConvertTextWithParser
        public string Render(string text)
        {
            var tokens = Parser.ParseTextToTokens(text, Tags);
            var tagWithTokens = TagsMarker.TokensToTagWithTokens(tokens, text, Tags);
            var htmlText = ConvertTokensToHtml(tagWithTokens, text);
            return htmlText;
        }

        //Проходит по полученым токенам и переводит их в html по предписаниям из тэга
        private string ConvertTokensToHtml(List<TagWithToken> tagWithTokens, string text)
        {
            var stringBuilder = new StringBuilder();
            foreach (var tagWithToken in tagWithTokens)
            {
                if (!tagWithToken.IsClose && !tagWithToken.IsOpen)
                {
                    stringBuilder.Append(text, tagWithToken.Token.Index, tagWithToken.Token.Length);
                }
                else if (tagWithToken.IsOpen)
                {
                    stringBuilder.Append(tagWithToken.Tag.OpenHtmlTag);
                }
                else if (tagWithToken.IsClose)
                {
                    stringBuilder.Append(tagWithToken.Tag.CloseHtmlTag);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
