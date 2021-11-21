using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkDown
{
    public static class Tokenizer
    {
        public static Token GetToken(string text)
        {
            var statesDict = new Dictionary<CaseType, bool>();
            statesDict.Add(CaseType.Italic, false);
            statesDict.Add(CaseType.Bold, false);
            var start = -1;
            var token = new Token(0, text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0 && text[i] == '#')
                {
                    token.nestedTokens.Add(new HeaderToken(0, text.Length));
                }
                if (text[i] == '_') {
                    if (statesDict.ContainsValue(true))
                    {
                        if (i - start == 1)
                        {
                            statesDict[CaseType.Bold] = true;
                            statesDict[CaseType.Italic] = false;
                        }
                        else if (statesDict[CaseType.Italic])
                        {
                            token.nestedTokens.Add(new ItalicToken(start, i - start));
                            statesDict[CaseType.Italic] = false;
                        }
                        else
                        {
                            if (CheckIfPreviousIsSpecificChar(text, i, '_'))
                            {
                                token.nestedTokens.Add(new BoldToken(start, i - start));
                                statesDict[CaseType.Bold] = false;
                            }
                        }
                    }
                    else
                    {
                        if (!CheckIfPreviousIsSpecificChar(text, i, '\\'))
                        {
                            start = i;
                            statesDict[CaseType.Italic] = true;
                        }
                    }
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
    }
}
