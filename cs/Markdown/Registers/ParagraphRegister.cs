using System.Linq;

namespace Markdown
{
    class ParagraphRegister : IReadable
    {
        string[] tags = { "<p>", "</p>" };
        int priority = 1;

        public Token TryGetToken(string input, int startPos)
        {
            string res;

            for (int i = startPos; i < input.Length - 1; i++)
            {
                if (input[i] == '\n' && input[i + 1] == '\n')
                {
                    res = input.Substring(startPos, i - startPos)
                        .Split('\n')
                        .Select(str => str.TrimStart(' ', '\r'))
                        .Aggregate((sum, s) => sum + '\n' + s);
                    return new Token(res, tags[0], tags[1] +'\n', priority, i - startPos + 1);
                }
            }
            res = input.Substring(startPos, input.Length - startPos)
                .Split('\n')
                .Select(str => str.TrimStart(' ', '\r'))
                .Aggregate((sum, s) => sum + '\n' + s)
                .TrimStart(' ', '\r', '\n');

            if (res == "")
                return new Token("", "", "", 1, input.Length - startPos);

            return new Token(res, tags[0], tags[1], priority, input.Length - startPos); 
        }
    }
}
