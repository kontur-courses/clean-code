using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkDown
{
    public static class Tokenizer
    {
        public static Token GetToken(string text)
        {
            var isOpenedItalic = false;
            var isOpenedBold = false;
            var start = -1;
            var token = new Token(0, text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '_') {
                    if (isOpenedItalic || isOpenedBold)
                    {
                        if (i - start == 1)
                        {
                            isOpenedBold = true;
                            isOpenedItalic = false;
                        }
                        else if (isOpenedItalic)
                        {
                            token.nestedTokens.Add(new ItalicToken(start, i - start));
                            isOpenedItalic = false;
                        }
                        else
                        {
                            if (text[i-1] == '_')
                            {
                                token.nestedTokens.Add(new BoldToken(start, i - start));
                                isOpenedBold = false;
                            }  
                        }
                    }
                    else
                    {
                        start = i;
                        isOpenedItalic = true;
                    }
                }
            }
            return token;
        }
    }
}
