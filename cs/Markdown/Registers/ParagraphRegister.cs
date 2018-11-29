using System.Linq;

namespace Markdown.Registers
{
    internal class ParagraphRegister : BaseRegister
    {
        private readonly string prefix = "<p>";
        private readonly string suffix = "</p>";
        protected override int Priority => 0;

        public override Token TryGetToken(string input, int startPos)
        {
            string res;

            for (var i = startPos; i < input.Length - 1; i++)
                if (input[i] == '\n' && input[i + 1] == '\n')
                {
                    res = input.Substring(startPos, i - startPos)
                        .Split('\n')
                        .Select(str => str.TrimStart(' '))
                        .Aggregate((sum, s) => sum + '\n' + s);
                    return new Token(res, prefix, suffix, Priority, i - startPos + 2, true);
                }

            res = input.Substring(startPos, input.Length - startPos)
                .Split('\n')
                .Select(str => str.TrimStart(' '))
                .Aggregate((sum, s) => sum + '\n' + s)
                .TrimStart(' ', '\n');

            if (res == "")
                return new Token("", "", "", Priority, input.Length - startPos, true);

            return new Token(res, prefix, suffix, Priority, input.Length - startPos, true);
        }
    }
}