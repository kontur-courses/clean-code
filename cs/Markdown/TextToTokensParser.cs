using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Markdown
{
    public enum Added
    {
        Temp,
        Result
    }
    public class TextToTokenParserContext
    {
        public Added Last { get; set; }
        public string Text { get; }
        public List<Token> Result { get; }
        public List<Token> TempStrongTokens { get; }
        public Stack<(int Index, string Value)> TagStack { get; }
        public Stack<(int Index,string Value)> TempStrongTagStack { get; }
        public List<int> ShieldingList { get; }
        public bool HasDigitBefore { get; set; } 
        public bool Checkpoint { get; set; }
        public TextToTokenParserContext(string text)
        {
            Last = Added.Result;
            Checkpoint = false;
            ShieldingList = new List<int>();
            Text = text;
            HasDigitBefore = false;
            Result=new List<Token>();
            TempStrongTokens=new List<Token>();
            TagStack= new Stack<(int Index, string Value)>();
            TempStrongTagStack = new Stack<(int Index, string Value)>();
        }
    }

    public static class TextToTokensParser
    {   private static readonly HashSet<char> ShieldingSymbols = new HashSet<char> {'\\'};
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char> { '_', '[', ']', '(', ')' };
        public static List<Token> Parse(string text)
        {
            var context = new TextToTokenParserContext(text);
            for (var i = 0; i < text.Length; i++)
            {
                if (ShieldingSymbols.Contains(text[i]))
                {
                    WorkWithShielding(context, i);
                    continue;
                }
                if(char.IsDigit(text[i]))
                    WorkWithDigits(context,i);
                if (text[i] == ' ')
                {
                    context.Checkpoint = true;
                    context.HasDigitBefore = false;
                }
                if(i!=0 && text[i-1]!='\\')
                        context.ShieldingList.Clear();
                if (!SpecialSymbols.Contains(text[i]) || 
                    i!=0 && text[i-1]=='\\' && context.ShieldingList.Count % 2 == 1) continue;
                context.Checkpoint = false;
                if (text[i].IsLinkTag())
                    WorkWithLinks(context, i);
                if (text[i].Is_Tag())
                    WorkWith_(context,i);
            }

            foreach (var token in context.TempStrongTokens) context.Result.Add(token);
            return context.Result;
        }

        private static void WorkWithDigits(TextToTokenParserContext context, int i)
        {
            if (context.TagStack.Count != 0 || context.TempStrongTagStack.Count != 0) return;
            context.HasDigitBefore = true;
            if (!context.Checkpoint)
            {
                if(context.Last==Added.Result && context.Result.Count!=0)
                    context.Result.RemoveAt(context.Result.Count - 1);
                if(context.Last==Added.Temp && context.TempStrongTokens.Count!=0)
                    context.TempStrongTokens.RemoveAt(context.TempStrongTokens.Count-1);
            }
        }

        private static void WorkWithShielding(TextToTokenParserContext context, int i)
        {
            context.ShieldingList.Add(i);
        }

        private static void WorkWith_(TextToTokenParserContext context, int i)
        {
            if (context.TagStack.Count == 0 && context.IsCharCorrectToAdd(i,1,OperationContext.Context.ToOpen))
            {
                context.TryToAdd_Tag((i,context.Text[i].ToString()));
                return;
            }

            if (context.TagStack.Count != 0 && context.TagStack.Peek().Index == i - 1 && context.TagStack.Peek().Value=="_")
            {
                context.TryToAdd__Tag((context.TagStack.Pop().Index, "__"));
                return;
            }

            if(context.TagStack.Count!=0)
                context.TryToAdd_Tag((i,context.Text[i].ToString()));

        }
        

        private static void WorkWithLinks(TextToTokenParserContext context, int i)
        {
            if (context.Text[i] == '[')
            {
                if (context.TagStack.Count == 0)
                {
                    context.TagStack.Push((i, context.Text[i].ToString()));
                    return;
                }

                if (context.TagStack.Count != 0 && context.TagStack.Peek().Value == "[")
                    context.TagStack.Pop();
                context.TagStack.Push((i, context.Text[i].ToString()));
                return;
            }

            if (context.TagStack.Count == 0) return;
            if (context.Text[i] == ']')
            {
                var last = context.TagStack.Pop();
                if (last.Value == "[")
                    context.TagStack.Push((last.Index, "[]"));
                return;
            }

            if (context.Text[i] == '(')
            {
                var last = context.TagStack.Pop();
                if (i != 0 && context.Text[i - 1] == ']' && last.Value == "[]")
                    context.TagStack.Push((last.Index, "[]("));
                return;
            }

            if (context.Text[i] == ')')
            {
                var last = context.TagStack.Pop();
                if (last.Value == "[](")
                    context.Result.Add(context.Text.GetToken(last.Index, i, last.Value));
            }
        }
    }
}