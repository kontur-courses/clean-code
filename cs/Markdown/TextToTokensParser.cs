using System.Collections.Generic;

namespace Markdown
{
    public class TextToTokenParserContext
    {
        public string Text { get; }
        public HashSet<Token> Result { get; }
        public List<Token> Temp { get; }
        public List<Token> TempNotDoubleTags { get; }
        public Stack<(int Index, string Value)> TagStack { get; }
        public Stack<(int Index,string Value)> TempTagStack { get; }

        public TextToTokenParserContext(string text)
        {
            Text = text;
            Result=new HashSet<Token>();
            Temp=new List<Token>();
            TempNotDoubleTags= new List<Token>();
            TagStack= new Stack<(int Index, string Value)>();
            TempTagStack = new Stack<(int Index, string Value)>();
        }
    }

    public static class TextToTokensParser
    {
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char> { '_', '[', ']', '(', ')' };
        public static HashSet<Token> Parse(string text)
        {
            var context = new TextToTokenParserContext(text);
            for (var i = 0; i < text.Length; i++)
            {
                if (!SpecialSymbols.Contains(text[i]) || i!=0 && text[i-1]=='\\') continue;
                if (text[i].IsLinkTag())
                    WorkWithLinks(context, i);
                if (text[i].Is_Tag())
                    WorkWith_(context,i);
            }
            foreach (var token in context.Temp)
                context.Result.Add(token);
            foreach (var tempNotDoubleTag in context.TempNotDoubleTags) context.Result.Add(tempNotDoubleTag);
            return context.Result;
        }

        private static void WorkWith_(TextToTokenParserContext context, int i)
        {
            if (context.TagStack.Count == 0)
            {
                if ((i != context.Text.Length - 1 && context.Text[i + 1] == ' ') || (i != 0 && char.IsDigit(context.Text[i - 1]))) return;
                context.TagStack.Push((i, context.Text[i].ToString()));
                return;
            }
            if (context.TagStack.Peek().Index == i - 1 && context.TagStack.Peek().Value == "_")
            {
                var (index, value) = context.TagStack.Pop();
                if (context.TagStack.Count != 0 && context.TagStack.Peek().Value == "__")
                    context.TagStack.Pop().TryToAddClose__Tag(context,i);
                else
                {
                    if(context.TagStack.Count!=0 && context.TagStack.Peek().Value=="_")
                        tempNotDoubleTags.Add(text.GetToken(context.TagStack.Peek().Index,i-1,"_"));
                    (index, "__").TryToAddOpenTag(tagStack, i, text, tempNotDoubleTags,result);
                }
            }
            else
            {
                if (context.TagStack.Peek().Value == text[i].ToString() &&
                    (i == text.Length - 1 || !SpecialSymbols.Contains(text[i + 1])))
                    context.TagStack.Pop().TryToAddClose_Tag(context,i);
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