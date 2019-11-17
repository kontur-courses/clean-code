using System.Collections.Generic;

namespace Markdown
{
    public static class TextToTokensParser
    {
        public static HashSet<Token> Parse(string text)
        {
            var specialSymbols = new HashSet<char> { '_', '[', ']' };
            var result = new HashSet<Token>();
            var temp = new List<Token>();
            var tagStack = new Stack<(int Index, string Value)>();
            for (var i = 0; i < text.Length; i++)
            {
                if (tagStack.Count == 0 && specialSymbols.Contains(text[i]))
                {
                    tagStack.Push((i, text[i].ToString()));
                    continue;
                }
                if (tagStack.Count == 0 || !specialSymbols.Contains(text[i])) continue;
                if (tagStack.Peek().Index == i - 1)
                {
                    var last = tagStack.Pop();
                    if (tagStack.Count != 0 && tagStack.Peek().Value == "__")
                    {
                        last = tagStack.Pop();
                        if (tagStack.Count == 0 || tagStack.Peek().Value != "_")
                            result.Add(text.GetToken(last.Index, i,last.Value));
                        else
                            temp.Add(text.GetToken(last.Index, i,last.Value));
                    }
                    else
                        tagStack.Push((last.Index, "__"));
                }
                else
                {
                    if (tagStack.Peek().Value == text[i].ToString() &&(i==text.Length-1 ||!specialSymbols.Contains(text[i+1])))
                    {
                        var last = tagStack.Pop();
                        result.Add(text.GetToken(last.Index, i,last.Value));
                        temp= new List<Token>();
                    }
                    else
                        tagStack.Push((i, text[i].ToString()));
                }
            }

            foreach (var token in temp)
                result.Add(token);
            return result;
        }
    }

    public static class ExtensionsMethods
    {
        public static Token GetToken(this string str, int start, int end, string tag)
        {
            var length = end - start + 1;
            return new Token(str.Substring(start, length), start, length,tag);
        }
    }
}