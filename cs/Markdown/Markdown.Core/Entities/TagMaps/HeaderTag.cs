using Markdown.Core.Entities.Abstract;

namespace Markdown.Core.Entities.TagMaps
{
    public class HeaderTag : BaseTag
    {
        protected override int Priority => 1;

        public override Token TryGetToken(string input, int startPos)
        {
            var i = startPos;
            for (; i < input.Length && char.IsWhiteSpace(input[i]); i++)
            {
                if (i - startPos >= 3)
                    return null;
            }

            var level = GetLevel(input, ref i);
            if (level == 0)
                return null;

            while (i < input.Length && char.IsWhiteSpace(input[i]))
                i++;

            var value = GetValue(input, ref i);
            return new Token(value, $"<h{level}>", $"</h{level}>", Priority, i - startPos, false);
        }

        private static int GetLevel(string input, ref int i)
        {
            int level = 0;
            while (i < input.Length && input[i] == '#')
            {
                level++;
                i++;
            }

            if (level == 0 || level > 6 || i < input.Length && !char.IsWhiteSpace(input[i]))
                return 0;

            return level;
        }

        private static string GetValue(string input, ref int i)
        {
            int valueStartIndex = i, valueEndIndex = 0;
            while (i < input.Length && input[i] != '\n')
            {
                if (input[i] == '#' && char.IsWhiteSpace(input[i - 1]))
                {
                    while (i < input.Length && input[i] == '#')
                        i++;

                    continue;
                }

                if (input[i] != ' ')
                    valueEndIndex = i;

                i++;
            }

            return valueEndIndex != 0 ? input.Substring(valueStartIndex, valueEndIndex - valueStartIndex + 1) : "";
        }
    }
}
