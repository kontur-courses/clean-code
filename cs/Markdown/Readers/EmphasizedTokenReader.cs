using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            var stack = new Stack<int>();
            var oneWordInTag = true;
            var value = new StringBuilder();
            token = null;

            if (!IsEmphasizedStartTag(text, index))
                return false;

            value.Append(text[index]);
            for (var i = index + 1; i < text.Length; i++)
            {
                if (IsEndOfLine(text, i))
                    return false;

                if (char.IsWhiteSpace(text[i]))
                    oneWordInTag = false;

                if (IsStrongEndTag(text, i) && IsIntersectedBehind(text, index, i))
                    return false;

                if (IsStrongStartTag(text, i) && IsIntersectedAhead(text, i))
                    return false;

                if (text[i] == '\\' && i + 1 != text.Length)
                {
                    if (IsEmphasizedEndTag(text, i + 1) && stack.Count == 0)
                        return false;

                    if (text[i + 1] == '\\' || IsEmphasizedStartTag(text, i + 1) || IsEmphasizedEndTag(text, i + 1))
                    {
                        value.Append(text[i..(i + 2)]);
                        i++;
                        continue;
                    }
                }

                if (IsEmphasizedEndTag(text, i) && stack.Count == 0)
                {
                    if (!IsEndOfWord(text, i) && !oneWordInTag)
                        return false;

                    value.Append(text[i]);
                    token = new EmphasizedToken(index, value.ToString()[1..^1], i);
                    return true;
                }

                if (IsEmphasizedStartTag(text, i))
                    stack.Push(i);

                if (IsEmphasizedEndTag(text, i) && stack.Count != 0)
                    stack.Pop();

                value.Append(text[i]);
            }

            return false;
        }

        private static bool IsIntersectedBehind(string text, int emphasizedStart, int index)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (IsStrongEndTag(text, i))
                    return false;

                if (IsStrongStartTag(text, i) && emphasizedStart > i)
                    return true;
            }

            return false;
        }

        private static bool IsIntersectedAhead(string text, int index)
        {
            var emphasizedEnd = -1;

            for (var i = index + 1; i < text.Length; ++i)
            {
                if (IsStrongStartTag(text, i))
                    return false;

                if (IsEmphasizedEndTag(text, i))
                    emphasizedEnd = i;

                if (IsStrongEndTag(text, i) && emphasizedEnd != -1 && emphasizedEnd < i)
                    return true;
            }

            return false;
        }

        private static bool IsEndOfWord(string text, int index)
        {
            return index + 1 == text.Length || char.IsWhiteSpace(text[index + 1]);
        }

        private static bool IsEndOfLine(string text, int index)
        {
            return text[index] == '\n' || text[index] == '\r';
        }

        private static bool IsEmphasizedStartTag(string text, int index)
        {
            return text[index] == '_'
                   && index + 1 < text.Length
                   && text[index + 1] != '_'
                   && !text[index + 1].IsDigitOrWhiteSpace()
                   && (index - 1 < 0 || text[index - 1] != '_');
        }

        private static bool IsEmphasizedEndTag(string text, int index)
        {
            return text[index] == '_'
                   && index - 1 >= 0
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && (index + 1 == text.Length || text[index + 1] != '_');
        }

        private static bool IsStrongStartTag(string text, int index)
        {
            return index >= 0
                   && text[index] == '_'
                   && index + 2 < text.Length
                   && text[index + 1] == '_'
                   && !text[index + 2].IsDigitOrWhiteSpace()
                   && text[index + 2] != '_';
        }

        private static bool IsStrongEndTag(string text, int index)
        {
            return text[index] == '_'
                   && index - 1 >= 0
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && index + 1 < text.Length
                   && text[index + 1] == '_';
        }
    }
}