using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class ExtensionsMethods
    {
        public static bool IsCharCorrectToAdd(this TextToTokenParserContext context, int i, int length, OperationContext.Context opContext)
        {
            (int Start, int End) index = (i, i + length - 1);
            if (index.Start == 0 || index.Start != 0 && !char.IsDigit(context.Text[index.Start - 1]) ||
                index.End == context.Text.Length - 1 || index.End != context.Text.Length - 1 && !char.IsDigit(context.Text[index.End + 1]))
            {
                if (opContext == OperationContext.Context.ToOpen &&
                    (index.End == context.Text.Length - 1 || context.Text[index.End + 1] != ' '))
                    return true;
                if (opContext == OperationContext.Context.ToClose &&
                    (index.Start == 0 || context.Text[index.Start - 1] != ' '))
                    return true;
            }
            return false;
        }
        public static Token GetToken(this string str, int start, int end, string tag)
        {
            var length = end - start + 1;
            return new Token(str.Substring(start, length), start, length, tag);
        }

        public static bool IsLinkTag(this char s)
        {
            return s == '[' || s == ']' || s == '(' || s == ')';
        }

        public static bool Is_Tag(this char s)
        {
            return s == '_';
        }

        public static bool Is_Shielding(this char s)
        {
            return s == '\\';
        }
        public static void TryToAdd__Tag(this TextToTokenParserContext context, (int Index, string Value) tag)
        {
            if (context.TempStrongTagStack.Count == 0 && context.IsCharCorrectToAdd(tag.Index, 2, OperationContext.Context.ToOpen))
                context.TempStrongTagStack.Push(tag);
            else
                if (context.TempStrongTagStack.Count!=0 && context.IsCharCorrectToAdd(tag.Index, 2, OperationContext.Context.ToClose))  
                    context.TempStrongTokens.Add(context.Text.GetToken(context.TempStrongTagStack.Pop().Index, tag.Index + 1, tag.Value));

        }

        public static void TryToAddClose__Tag(this (int Index, string Value) closeTag, HashSet<Token> result, List<Token> temp, int i,
            string text, Stack<(int Index, string Value)> tagStack, List<Token> tempNotDoubleTags)
        {

        }

        public static void TryToAdd_Tag(this TextToTokenParserContext context, (int Index, string Value) tag)
        {
            if ( context.TagStack.Count!=0 && context.TagStack.Peek().Value == tag.Value && 
                 context.IsCharCorrectToAdd(tag.Index, 1, OperationContext.Context.ToClose))
            {
                context.Result.Add(context.Text.GetToken(context.TagStack.Pop().Index, tag.Index, tag.Value));
                context.TempStrongTokens.Clear();
                return;
            }

            if ((context.TagStack.Count == 0 || context.TagStack.Peek().Value != tag.Value) &&
                context.IsCharCorrectToAdd(tag.Index, 1, OperationContext.Context.ToOpen))
            {
                context.TagStack.Push(tag);
                foreach (var token in context.TempStrongTokens)
                    context.Result.Add(token);
            }
        }
    }
}