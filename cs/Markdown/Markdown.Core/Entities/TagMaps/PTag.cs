using Markdown.Core.Entities.Abstract;

namespace Markdown.Core.Entities.TagMaps
{
    public class PTag : BaseTag
    {
        private const string Prefix = "<p>";
        private const string Postfix = "</p>";
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

                    return new Token(res, Prefix, Postfix, Priority, i - startPos + 2, true);
                }

            res = input.Substring(startPos, input.Length - startPos)
                .Split('\n')
                .Select(str => str.TrimStart(' '))
                .Aggregate((sum, s) => sum + '\n' + s)
                .TrimStart(' ', '\n');

            return res == "" ? 
                new Token("", "", "", Priority, input.Length - startPos, true) 
                : new Token(res, Prefix, Postfix, Priority, input.Length - startPos, true);
        }
    }
}