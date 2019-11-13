using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TextToTokensParser
    {
        public static List<Token> Parse(string text)
        {
            //стэк для отслеживания парных символов _ и __ 
            var result = new List<Token>();
            var charStack = new Stack<Tuple<int,string>>();
            for (int i = 0; i < text.Length; i++)
            {
                if (charStack.Count() == 0 && text[i] == '_')
                {
                    charStack.Push(Tuple.Create(i, "_"));
                    continue;
                }
                if (charStack.Count != 0 && text[i] == '_')
                {
                    if (charStack.Peek().Item1 == i - 1)
                    {
                        var last = charStack.Pop();
                        if (charStack.Count!=0&&charStack.Peek().Item2 == "__")
                        {
                            var index = charStack.Pop().Item1;
                            result.Add(text.GetToken(index,i));
                        }
                        else
                        {
                            charStack.Push(Tuple.Create(last.Item1, "__"));
                        }
                    }
                    else
                    {
                        if (charStack.Peek().Item2 == text[i].ToString())
                        {
                            var index = charStack.Pop().Item1;
                            result.Add(text.GetToken(index, i));
                        }
                        else
                        {
                            charStack.Push(Tuple.Create(i,text[i].ToString()));
                        }
                    }
                }
            }
            return result;
        }
    }

    public static class ExtensionsMethods
    {
        public static Token GetToken(this string str, int start, int end)
        {
            var length = end - start + 1;
            return new Token(str.Substring(start, length), start, length);
        }
    }
}