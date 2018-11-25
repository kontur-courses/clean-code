using System.Text;

namespace Markdown
{
    class ParagraphRegister : IReadable
    {
        private string trimFirstSpaces(string text)
        {
            StringBuilder result = new StringBuilder();

            foreach (var str in text.Split('\n'))
            {
                result.Append(str.TrimStart(' ', '\r'));
                result.Append('\n');
            }
            result.Remove(result.Length - 1, 1);

            return result.ToString();
        }

        public Token tryGetToken(string input, int startPos)
        {
            string res;
            //input = input.TrimStart(' ', '\r', '\n');

            for (int i = startPos; i < input.Length - 1; i++)
            {
                if (input[i] == '\n' && input[i + 1] == '\n')
                {
                    res = trimFirstSpaces(input.Substring(startPos, i - startPos));
                    return new Token(res, "<p>", "</p>\n", 1, i - startPos + 2);
                }
            }

            res = trimFirstSpaces(input.Substring(startPos, input.Length - startPos));
            return new Token(res, "<p>", "</p>", 1, input.Length - startPos + 1); 
        }
    }
}
