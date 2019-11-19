using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class ExtensionsMethods
    {
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

        public static void TryToAddOpenTag(this (int, string) openTag, Stack<(int, string)> tagStack, int i, string text)
        {
            if(i!=0 && char.IsDigit(text[i-1]) && i!=text.Length-1 && char.IsDigit(text[i+1])) return;
            if (i == text.Length-1 || text[i + 1] != ' ')
                tagStack.Push(openTag);
        }

        public static void TryToAddClose__Tag(this (int Index, string Value) closeTag,HashSet<Token> result,List<Token> temp, int i,
            string text, Stack<(int Index, string Value)> tagStack)
        {
            if( i!=0 && i!=text.Length-1 && (text[i-1]==' ' || char.IsDigit(text[i+1]))) return;
            if (tagStack.Count == 0 || tagStack.Peek().Value != "_" )
                result.Add(text.GetToken(closeTag.Index, i, closeTag.Value));
            else
                temp.Add(text.GetToken(closeTag.Index, i, closeTag.Value));
        }

        public static void TryToAddClose_Tag(this (int Index, string Value) closeTag, HashSet<Token> result,
            List<Token> temp,string text,int i)
        {
            if (i != 0 && i!=text.Length-1 && (text[i - 1] == ' '||char.IsDigit(text[i+1]))) return;
            result.Add(text.GetToken(closeTag.Index, i, closeTag.Value));
            temp.Clear();
        }

    }
}