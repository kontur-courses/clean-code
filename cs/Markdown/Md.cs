using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Md
    {
        private readonly List<Tag> Tags;
        private readonly char[] Digits = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

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
            var tokens = ParseTextToTokens(text);
            var htmlText = ConvertTokensToHtml(tokens, text);
            return htmlText;
        }

        //Проверяет есть ли символ в словаре и если есть то действует по предписаниям тэга: продолжает собирать токен или начинает новый
        //Если символа нет в словате считает это токеном с каким-то текстом
        private List<Token> ParseTextToTokens(string text)
        {
            var tokens = new List<Token>();
            var index = 0;
            var length = 1;
            var isContext = false;
            while (index + length <= text.Length)
            {
                if (text[index + length - 1] == '\\')
                {
                    var matchCompletely = Tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text));
                    tokens.Add(new Token(index, length - 1, matchCompletely));
                    index = index + length;
                    length = 2;
                    isContext = true;
                }
                else if (isContext)
                {
                    var partiallyMatch = Tags.Any(tag => tag.PartiallyMatchTagAndToken(index + length - 1, 1, text));
                    if (partiallyMatch)
                    {
                        tokens.Add(new Token(index, length - 1, false));
                        index = index + length - 1;
                        length = 1;
                        isContext = false;
                    }
                    else
                    {
                        length++;
                    }
                }
                else
                {
                    var partiallyMatch = Tags.Any(tag => tag.PartiallyMatchTagAndToken(index, length, text));
                    if (!partiallyMatch)
                    {
                        var matchCompletely = Tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text));
                        if (matchCompletely)
                        {
                            tokens.Add(new Token(index, length - 1, true));
                            index = index + length - 1;
                            length = 1;
                        }
                        else
                        {
                            isContext = true;
                        }
                    }
                    else
                    {
                        length++;
                    }
                }
            }
            tokens.Add(new Token(index, length - 1, Tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text))));
            return tokens;
        }

        //Проходит по полученым токенам и переводит их в html по предписаниям из тэга
        private string ConvertTokensToHtml(List<Token> tokens, string text)
        {
            var tagWithTokens = TokensToTagWithTokens(tokens, text);

            var stringBuilder = new StringBuilder();
            foreach (var tagWithToken in tagWithTokens)
            {
                if (!tagWithToken.IsClose && !tagWithToken.IsOpen)
                {
                    stringBuilder.Append(text, tagWithToken.Token.Index, tagWithToken.Token.Length);
                }
                else if (tagWithToken.IsOpen)
                {
                    stringBuilder.Append(tagWithToken.Tag.OpenHtml);
                }
                else if (tagWithToken.IsClose)
                {
                    stringBuilder.Append(tagWithToken.Tag.CloseHtml);
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
                    var tag = FindMatchingTag(token, text);
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
                var indexOpeningTag = FindIndexMatchingOpenTag(openingTagList, tagWithToken.Tag, text);
                if (indexOpeningTag >= 0)
                {
                    openingTagList[indexOpeningTag].IsOpen = true;
                    tagWithToken.IsClose = true;
                    for (var i = 0; i < openingTagList.Count - indexOpeningTag + 1; i++)
                    {
                        openingTagList.RemoveAt(openingTagList.Count - 1);
                    }

                    switch (tagWithToken.Tag.TagString) // switch case который определяет специфическое поведение для закрывающих тегов
                    {
                        case "_":
                            var i = tagWithTokens.Count - 1;
                            while (tagWithTokens[i] != tagWithTokens[indexOpeningTag])
                            {
                                if (tagWithTokens[i].IsTag && tagWithTokens[i].Tag.TagString == "__")
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

        private int FindIndexMatchingOpenTag(List<TagWithToken> tagList, Tag tag, string text)
        {
            for (var i = tagList.Count - 1; i >= 0; i--)
            {
                if (tag.TagString == tagList[i].Tag.TagString && tagList[i].CanTagBeOpening(text))
                    return i;
            }
            return -1;
        }

        private Tag FindMatchingTag(Token token, string text)
        {
            foreach (var tag in Tags)
                if (tag.MatchTagAndTokenCompletely(token.Index, token.Length, text))
                    return tag;
            return null;
        }
    }
}
