using System.Collections.Generic;

namespace Markdown
{
    public static class TextToTokensParser
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char> { '_', '[', ']', '(', ')' };

        public static HashSet<Token> Parse(string text)
        {
            var result = new HashSet<Token>();
            var temp = new List<Token>();
            var tempNotDoubleTags = new List<Token>();
            var tagStack = new Stack<(int Index, string Value)>();
            for (var i = 0; i < text.Length; i++)
            {
                if (!SpecialSymbols.Contains(text[i]) || i!=0 && text[i-1]=='\\') continue;
                if (text[i].IsLinkTag())
                    WorkWithLinks(text, tagStack, temp, result, i);
                if (text[i].Is_Tag())
                    WorkWith_(text, tagStack, temp, result, i, tempNotDoubleTags);
            }
            foreach (var token in temp)
                result.Add(token);
            foreach (var tempNotDoubleTag in tempNotDoubleTags) result.Add(tempNotDoubleTag);
            return result;
        }

        private static void WorkWith_(string text, Stack<(int Index, string Value)> tagStack, List<Token> temp,
            HashSet<Token> result, int i, List<Token> tempNotDoubleTags)
        {
            if (tagStack.Count == 0)
            {
                if ((i != text.Length - 1 && text[i + 1] == ' ') || (i != 0 && char.IsDigit(text[i - 1]))) return;
                tagStack.Push((i, text[i].ToString()));
                return;
            }
            if (tagStack.Peek().Index == i - 1 && tagStack.Peek().Value == "_")
            {
                var (index, value) = tagStack.Pop();
                if (tagStack.Count != 0 && tagStack.Peek().Value == "__")
                    tagStack.Pop().TryToAddClose__Tag(result, temp, i, text, tagStack,tempNotDoubleTags);
                else
                {
                    if(tagStack.Count!=0 && tagStack.Peek().Value=="_")
                        tempNotDoubleTags.Add(text.GetToken(tagStack.Peek().Index,i-1,"_"));
                    (index, "__").TryToAddOpenTag(tagStack, i, text, tempNotDoubleTags,result);
                }
            }
            else
            {
                if (tagStack.Peek().Value == text[i].ToString() &&
                    (i == text.Length - 1 || !SpecialSymbols.Contains(text[i + 1])))
                    tagStack.Pop().TryToAddClose_Tag(result, temp, text, i, tempNotDoubleTags);
                else
                {
                    if(tagStack.Count!=0 && tagStack.Peek().Value=="__")
                        tempNotDoubleTags.Add(text.GetToken(tagStack.Peek().Index+1,i,"_"));
                    (i, text[i].ToString()).TryToAddOpenTag(tagStack, i, text, tempNotDoubleTags,result);
                }
            }
        }

        private static void WorkWithLinks(string text, Stack<(int Index, string Value)> tagStack, List<Token> temp,
            HashSet<Token> result, int i)
        {
            if (text[i] == '[')
            {
                if (tagStack.Count == 0)
                {
                    tagStack.Push((i, text[i].ToString()));
                    return;
                }

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