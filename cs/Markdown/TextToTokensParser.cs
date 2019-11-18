using System.Collections.Generic;

namespace Markdown
{
    public static class TextToTokensParser
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char> {'_', '[', ']', '(', ')'};

        public static HashSet<Token> Parse(string text)
        {
            var result = new HashSet<Token>();
            var temp = new List<Token>();
            var tagStack = new Stack<(int Index, string Value)>();
            for (var i = 0; i < text.Length; i++)
            {
                if (tagStack.Count == 0 && SpecialSymbols.Contains(text[i]))
                {
                    tagStack.Push((i, text[i].ToString()));
                    continue;
                }

                if (tagStack.Count == 0 || !SpecialSymbols.Contains(text[i])) continue;
                if (text[i].IsLinkTag())
                    WorkWithLinks(text, tagStack, temp, result, i);
                if (text[i].Is_Tag())
                    WorkWith_(text, tagStack, temp, result, i);
            }

            foreach (var token in temp)
                result.Add(token);
            return result;
        }

        private static void WorkWith_(string text, Stack<(int Index, string Value)> tagStack, List<Token> temp,
            HashSet<Token> result, int i)
        {
            if (tagStack.Peek().Index == i - 1)
            {
                var (index, value) = tagStack.Pop();
                if (tagStack.Count != 0 && tagStack.Peek().Value == "__")
                {
                    (index, value) = tagStack.Pop();
                    if (tagStack.Count == 0 || tagStack.Peek().Value != "_")
                        result.Add(text.GetToken(index, i, value));
                    else
                        temp.Add(text.GetToken(index, i, value));
                }
                else
                {
                    tagStack.Push((index, "__"));
                }
            }
            else
            {
                if (tagStack.Peek().Value == text[i].ToString() &&
                    (i == text.Length - 1 || !SpecialSymbols.Contains(text[i + 1])))
                {
                    var (index, value) = tagStack.Pop();
                    result.Add(text.GetToken(index, i, value));
                    temp.Clear();
                }
                else
                {
                    tagStack.Push((i, text[i].ToString()));
                }
            }
        }

        private static void WorkWithLinks(string text, Stack<(int Index, string Value)> tagStack, List<Token> temp,
            HashSet<Token> result, int i)
        {
            if (text[i] == '[')
            {
                if (tagStack.Count != 0 && tagStack.Peek().Value == "[")
                    tagStack.Pop();
                tagStack.Push((i, text[i].ToString()));
                return;
            }

            if (tagStack.Count == 0) return;
            if (text[i] == ']')
            {
                var last = tagStack.Pop();
                if (last.Value == "[")
                    tagStack.Push((last.Index, "[]"));
                return;
            }

            if (text[i] == '(')
            {
                var last = tagStack.Pop();
                if (i != 0 && text[i - 1] == ']' && last.Value == "[]")
                    tagStack.Push((last.Index, "[]("));
                return;
            }

            if (text[i] == ')')
            {
                var last = tagStack.Pop();
                if (last.Value == "[](")
                    result.Add(text.GetToken(last.Index, i, last.Value));
            }
        }
    }
}