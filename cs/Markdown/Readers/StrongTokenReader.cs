using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class StrongTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            var stack = new Stack<int>();
            var oneWordInTag = true;
            var value = new StringBuilder();
            token = null;

            if (IsEmphasizedStartTag(context, 0) && IsEmphasizedEndTag(context, context.Length - 1))
                return false;

            if (!IsStrongStartTag(text, index))
                return false;

            value.Append(text[index..(index + 2)]);
            for (var i = index + 2; i < text.Length; i++)
            {
                if (IsEndOfLine(text, i))
                    return false;

                if (char.IsWhiteSpace(text[i]))
                    oneWordInTag = false;

                if (IsEmphasizedEndTag(text, i) && IsIntersectedBehind(text, index, i))
                    return false;

                if (IsEmphasizedStartTag(text, i) && IsIntersectedAhead(text, i))
                    return false;

                if (text[i] == '\\' && i + 1 != text.Length)
                {
                    if (IsStrongEndTag(text, i + 1) && stack.Count == 0)
                        return false;

                    if (text[i + 1] == '\\' || IsStrongStartTag(text, i + 1) || IsStrongEndTag(text, i + 1))
                    {
                        value.Append(text[i..(i + 2)]);
                        i++;
                        continue;
                    }
                }

                if (IsStrongEndTag(text, i) && stack.Count == 0)
                {
                    if (!IsEndOfWord(text, i + 1) && !oneWordInTag)
                        return false;

                    value.Append(text[i..(i + 2)]);
                    token = new StrongToken(index, value.ToString()[2..^2], i + 1);
                    return true;
                }

                if (IsStrongStartTag(text, i))
                    stack.Push(i);

                if (IsStrongEndTag(text, i) && stack.Count != 0)
                    stack.Pop();

                value.Append(text[i]);
            }

            return false;
        }

        private static bool IsIntersectedBehind(string text, int strongStart, int index)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (IsEmphasizedEndTag(text, i))
                    return false;

                if (IsEmphasizedStartTag(text, i) && strongStart > i)
                    return true;
            }

            return false;
        }

        private static bool IsIntersectedAhead(string text, int index)
        {
            var strongEnd = -1;

            for (var i = index + 1; i < text.Length; ++i)
            {
                if (IsEmphasizedStartTag(text, i))
                    return false;

                if (IsStrongEndTag(text, i))
                    strongEnd = i;

                if (IsEmphasizedEndTag(text, i) && strongEnd != -1 && strongEnd < i)
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
    }
}