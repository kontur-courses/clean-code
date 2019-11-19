using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Parser
    {
        //Проверяет есть ли символ в словаре и если есть то действует по предписаниям тэга: продолжает собирать токен или начинает новый
        //Если символа нет в словате считает это токеном с каким-то текстом
        public static List<Token> ParseTextToTokens(string text, List<Tag> tags)
        {
            var tokens = new List<Token>();
            var index = 0;
            var length = 1;
            var isContext = false;
            while (index + length <= text.Length)
            {
                if (text[index + length - 1] == '\\' &&
                    index + length < text.Length &&
                    PartiallyMatchCharByIndex(index + length, text, tags))
                {
                    var matchCompletely = tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text));
                    tokens.Add(new Token(index, length - 1, matchCompletely));
                    index = index + length;
                    length = 2;
                    isContext = true;
                }
                else if (isContext)
                {
                    if (PartiallyMatchCharByIndex(index + length - 1, text, tags))
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
                    var partiallyMatch = tags.Any(tag => tag.PartiallyMatchTagAndToken(index, length, text));
                    if (!partiallyMatch)
                    {
                        var matchCompletely = tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text));
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
            tokens.Add(new Token(index, length - 1, !isContext && tags.Any(tag => tag.MatchTagAndTokenCompletely(index, length - 1, text))));
            return tokens;
        }

        private static bool PartiallyMatchCharByIndex(int index, string text, List<Tag> tags)
        {
            return tags.Any(tag => tag.PartiallyMatchTagAndToken(index, 1, text));
        }
    }
}
