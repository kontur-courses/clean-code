using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class ImageTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            var foundAltText = false;
            var value = new StringBuilder();
            Token? altText = null;
            token = null;

            if (!IsImageTagStart(text, index))
                return false;

            for (var i = index; i < text.Length; i++)
            {
                if (IsEndOfLine(text, i))
                    return false;

                if (text[i] == '\\' && i + 1 != text.Length)
                    if (text[i + 1] == '\\' || IsAltTexEnd(text, i + 1) || IsImageTagEnd(text, i + 1))
                    {
                        value.Append(text[i + 1]);
                        i++;
                        continue;
                    }

                if (IsAltTexEnd(text, i))
                {
                    altText = new PlaintTextToken(index + 1, text[(index + 2)..i], i);
                    foundAltText = true;
                }

                if (text[i] == ']' && !IsAltTexEnd(text, i))
                    return false;

                if (IsImageTagEnd(text, i) && foundAltText)
                {
                    value.Append(text[i]);
                    Token? url = new PlaintTextToken(altText!.EndPosition + 1, text[(altText.EndPosition + 2)..i], i);
                    token = new ImageToken(index, value.ToString(), index + value.Length - 1);
                    token.ChildTokens.Add(altText!);
                    token.ChildTokens.Add(url!);

                    return true;
                }

                value.Append(text[i]);
            }

            return false;
        }

        private static bool IsEndOfLine(string text, int index)
        {
            return text[index] == '\n' || text[index] == '\r';
        }

        private static bool IsImageTagStart(string text, int index)
        {
            return text[index] == '!' && (index + 1 == text.Length || text[index + 1] == '[');
        }

        private static bool IsImageTagEnd(string text, int index)
        {
            return text[index] == ')';
        }

        private static bool IsAltTexEnd(string text, int index)
        {
            return text[index] == ']' && (index + 1 == text.Length || text[index + 1] == '(');
        }
    }
}