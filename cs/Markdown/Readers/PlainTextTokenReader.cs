using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class PlainTextTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            token = null;
            var value = new StringBuilder();

            for (var i = index; i < text.Length; ++i)
            {
                if (text[i] == '\\' && i + 1 != text.Length)
                    if (text[i + 1] == '\\' || IsPlainTextEnd(text, i + 1))
                    {
                        value.Append(text[i + 1]);
                        i++;

                        if (i + 1 == text.Length)
                        {
                            token = new PlaintTextToken(index, value.ToString(), i);
                            return true;
                        }

                        continue;
                    }

                if (i + 1 == text.Length)
                {
                    value.Append(text[i]);
                    token = new PlaintTextToken(index, value.ToString(), i);
                    return true;
                }

                if (IsPlainTextEnd(text, i) && value.Length != 0)
                {
                    token = new PlaintTextToken(index, value.ToString(), i - 1);
                    return true;
                }

                value.Append(text[i]);
            }

            return false;
        }

        private static bool IsPlainTextEnd(string text, int index)
        {
            return text[index] == '#' || text[index] == '_' || text[index] == '!';
        }
    }
}