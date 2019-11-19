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
            var tagWithTokens = TokensToTagWithTokens(tokens, text);
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

        private List<TagWithToken> TokensToTagWithTokens(List<Token> tokens, string text)
        {
            var openingTagList = new List<TagWithToken>();
            var tagWithTokens = new List<TagWithToken>();
            foreach (var token in tokens)
            {
                if (token.IsTag)
                {
                    var tag = Tags.Find(t => t.MatchTagAndTokenCompletely(token.Index, token.Length, text));
                    var tagWithToken = new TagWithToken(tag, token);
                    if (tagWithToken.CanBeTag(text))
                    {
                        tagWithTokens.Add(tagWithToken);
                        HandlerTag(openingTagList, tagWithTokens, tagWithToken, text);
                    }
                    else
                        tagWithTokens.Add(new TagWithToken(null, token));
                }
                else
                {
                    tagWithTokens.Add(new TagWithToken(null, token));
                }
            }
            return tagWithTokens;
        }

        private void HandlerTag(List<TagWithToken> openingTagList, List<TagWithToken> tagWithTokens, TagWithToken tagWithToken, string text)
        {
            if (tagWithToken.CanTagBeClosing(text))
            {
                var indexOpeningTag = openingTagList.FindLastIndex(tag => tagWithToken.Tag.MarkdownTag == tag.Tag.MarkdownTag && tag.CanTagBeOpening(text));
                if (indexOpeningTag >= 0)
                {
                    openingTagList[indexOpeningTag].IsOpen = true;
                    tagWithToken.IsClose = true;
                    openingTagList.RemoveRange(indexOpeningTag, openingTagList.Count - indexOpeningTag);

                    switch (tagWithToken.Tag.MarkdownTag)
                    {
                        case "_":
                            var i = tagWithTokens.Count - 1;
                            while (tagWithTokens[i] != tagWithTokens[indexOpeningTag])
                            {
                                if (tagWithTokens[i].IsTag && tagWithTokens[i].Tag.MarkdownTag == "__")
                                {
                                    tagWithTokens[i].IsClose = false;
                                    tagWithTokens[i].IsOpen = false;
                                }
                                i--;
                            }
                            break;
                    }
                    return;
                }
            }
            if (tagWithToken.CanTagBeOpening(text))
            {
                openingTagList.Add(tagWithToken);
            }
        }
    }
}
