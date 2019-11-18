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
                    var matchCompletely = Tags.Any(tag => MatchTagAndTokenCompletely(index, length - 1, text, tag));
                    tokens.Add(new Token(index, length - 1, matchCompletely));
                    index = index + length;
                    length = 2;
                    isContext = true;
                }
                else if (isContext)
                {
                    var partiallyMatch = Tags.Any(tag => PartiallyMatchTagAndToken(index + length - 1, 1, text, tag));
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
                    var partiallyMatch = Tags.Any(tag => PartiallyMatchTagAndToken(index, length, text, tag));
                    if (!partiallyMatch)
                    {
                        var matchCompletely = Tags.Any(tag => MatchTagAndTokenCompletely(index, length - 1, text, tag));
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
            tokens.Add(new Token(index, length - 1, Tags.Any(tag => MatchTagAndTokenCompletely(index, length - 1, text, tag))));
            return tokens;
        }

        private bool PartiallyMatchTagAndToken(int index, int length, string text, Tag tag)
        {
            if (tag.isMultiple)
                return PartiallyMatchMultipleTagAndToken(index, length, text, tag);
            return PartiallyMatchNoMultipleTagAndToken(index, length, text, tag);
        }

        private bool PartiallyMatchMultipleTagAndToken(int index, int length, string text, Tag tag)
        {
            return tag.getTagString[tag.getTagString.Length - 1] == text[index + length - 1];
        }

        private bool PartiallyMatchNoMultipleTagAndToken(int index, int length, string text, Tag tag)
        {
            if (length > tag.getTagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (!(tag.getTagString[i] == text[index + i]))
                    return false;
            return true;
        }

        private bool MatchTagAndTokenCompletely(int index, int length, string text, Tag tag)
        {
            if (length != tag.getTagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (!(tag.getTagString[i] == text[index + i]))
                    return false;
            return true;
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
                    stringBuilder.Append(tagWithToken.Tag.getOpenHtml);
                }
                else if (tagWithToken.IsClose)
                {
                    stringBuilder.Append(tagWithToken.Tag.getCloseHtml);
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
                    if (CanBeTag(tagWithToken, text))
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

        private bool CanBeTag(TagWithToken tagWithToken, string text)
        {
            switch (tagWithToken.Tag.getTagString) // switch case который определяет может ли токен помеченый как тег быть тегом
            {
                case "_":
                    if (tagWithToken.Token.Index + tagWithToken.Token.Length < text.Length &&
                        Digits.Contains(text[tagWithToken.Token.Index + tagWithToken.Token.Length]) &&
                        tagWithToken.Token.Index - 1 >= 0 &&
                        Digits.Contains(text[tagWithToken.Token.Index - 1]))
                    {
                        return false;
                    }
                    break;
                case "__":
                    if (tagWithToken.Token.Index + tagWithToken.Token.Length < text.Length &&
                        Digits.Contains(text[tagWithToken.Token.Index + tagWithToken.Token.Length]) &&
                        tagWithToken.Token.Index - 1 >= 0 &&
                        Digits.Contains(text[tagWithToken.Token.Index - 1]))
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        private void HandlerTag(List<TagWithToken> openingTagList, List<TagWithToken> tagWithTokens, TagWithToken tagWithToken, string text)
        {
            if (CanTagBeClosing(tagWithToken, text))
            {
                var indexOpeningTag = FindIndexMatchingOpenTag(openingTagList, tagWithToken.Tag, text);
                if (indexOpeningTag >= 0)
                {
                    openingTagList[indexOpeningTag].IsOpen = true;
                    tagWithToken.IsClose = true;
                    for (var i = 0; i < indexOpeningTag; i++)
                    {
                        openingTagList.RemoveAt(openingTagList.Count - 1);
                    }

                    switch (tagWithToken.Tag.getTagString) // switch case который определяет специфическое поведение для закрывающих тегов
                    {
                        case "_":
                            var i = tagWithTokens.Count - 1;
                            while (tagWithTokens[i] != tagWithTokens[indexOpeningTag])
                            {
                                if (tagWithTokens[i].IsTag && tagWithTokens[i].Tag.getTagString == "__")
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
            if (CanTagBeOpening(tagWithToken, text))
            {
                openingTagList.Add(tagWithToken);
            }
        }

        private bool CanTagBeClosing(TagWithToken tagWithToken, string text)
        {
            var index = tagWithToken.Token.Index - 1;
            if (index < 0 || text[index] == ' ')
                return false;
            return true;
        }

        private bool CanTagBeOpening(TagWithToken tagWithToken, string text)
        {
            var index = tagWithToken.Token.Index + tagWithToken.Token.Length;
            if (index >= text.Length || text[index] == ' ')
                return false;
            return true;
        }

        private int FindIndexMatchingOpenTag(List<TagWithToken> tagList, Tag tag, string text)
        {
            for (var i = tagList.Count - 1; i >= 0; i--)
            {
                if (tag.getTagString == tagList[i].Tag.getTagString && CanTagBeOpening(tagList[i], text))
                    return i;
            }
            return -1;
        }

        private Tag FindMatchingTag(Token token, string text)
        {
            foreach (var tag in Tags)
                if (MatchTagAndTokenCompletely(token.Index, token.Length, text, tag))
                    return tag;
            return null;
        }
    }
}
