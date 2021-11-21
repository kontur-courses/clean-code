using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarkDown
{
    public static class Tokenizer
    {
        public static Token GetToken(string text)
        {
            var isEscaping = false;
            var isSplittingWord = false;
            //храним некий currentToken
            var statesDict = new Dictionary<CaseType, bool>();
            statesDict.Add(CaseType.Italic, false);
            statesDict.Add(CaseType.Bold, false);
            var start = -1;
            var token = new Token(0, text.Length);
            var currentToken = token;
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0 && text[i] == '#')
                {
                    currentToken.nestedTokens.Add(new HeaderToken(0, text.Length));
                }
                if (char.IsDigit(text[i]))
                {
                    foreach (var pair in statesDict)
                    {
                        statesDict[pair.Key] = false;
                    }
                }
                if (char.IsWhiteSpace(text[i]) && isSplittingWord)
                {
                    foreach (var pair in statesDict)
                    {
                        statesDict[pair.Key] = false;
                    }
                }
                if (text[i] == '_')
                {
                    if (statesDict.ContainsValue(true))
                    {
                        if (i - start == 1)
                        {
                            statesDict[CaseType.Bold] = true;
                            statesDict[CaseType.Italic] = false;
                        }
                        else if (statesDict[CaseType.Italic])
                        {
                            currentToken.nestedTokens.Add(new ItalicToken(start, i - start));
                            statesDict[CaseType.Italic] = false;
                        }
                        else
                        {
                            if (CheckIfPreviousIsSpecificChar(text, i, '_'))
                            {
                                currentToken.nestedTokens.Add(new BoldToken(start, i - start));
                                statesDict[CaseType.Bold] = false;
                            }
                        }
                    }
                    else
                    {
                        if (CheckIfPreviousIsSpecificChar(text, i, '\\') || isEscaping)
                        {
                            isEscaping = true;
                        }
                        else
                        {
                            if (CheckIfPreviousIsLetter(text, i))
                            {
                                isSplittingWord = true;
                            }
                            else
                            {
                                isSplittingWord = false;
                            }
                            start = i;
                            statesDict[CaseType.Italic] = true;
                        }
                    }
                }
                else
                {
                    isEscaping = false;
                }
            }
            return token;
        }

        private static bool CheckIfPreviousIsSpecificChar(string text, int i, char ch)
        {
            if (i == 0)
            {
                return false;
            }
            return text[i - 1] == ch;
        }

        private static bool CheckIfPreviousIsLetter(string text, int i)
        {
            if (i == 0)
            {
                return false;
            }
            return char.IsLetter(text[i - 1]);
        }
    }
}
