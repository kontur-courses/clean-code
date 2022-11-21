using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Markdown
{
    public class Md
    {
        private TokenParser parser;

        public Md()
        {

            var underlinesContentRules = new List<Func<Tag, bool, string, bool>>
            {
                (tag, isPartial, text) =>
                    text[0] != ' ' && text[text.Length-1] != ' ',
                (tag, isPartial, text) =>
                    !text
                        .All(char.IsNumber),
                (tag, isPartial, text) =>
                    !isPartial || !text.Any(char.IsWhiteSpace),
            };
            var tags = new List<Tag>
            {
                new Tag("__", "__", "strong", underlinesContentRules,
                    new List<Func<Token, List<Token>, bool>>
                    {
                        (token, tokens) => token.FindParent(tokens)?.Tag.HtmlTag != "em",
                        (token, tokens) => !tokens.Any(t => t.IntersectsWith(token)),
                    }),

                new Tag("_", "_", "em", underlinesContentRules,
                    new List<Func<Token, List<Token>, bool>>
                    {
                        (token, tokens) => !tokens.Any(t => t.IntersectsWith(token)),
                    }),

                new Tag("# ", Environment.NewLine, "h1",
                    new List<Func<Tag, bool, string, bool>>
                    {

                    }, new List<Func<Token, List<Token>, bool>>
                    {

                    }),
            };
            parser = new TokenParser(tags);
        }

        public string Render(string text)
        {
            var tokens = parser.ParseTokens(text);
            return GetResultString(text, tokens);
        }

        private string GetResultString(string text, List<Token> tokens)
        {
            var result = new StringBuilder();
            var index = 0;
            while (index < text.Length)
            {
                var (target, tag, mark) = GetClosestIndex(index, tokens);
                target = Math.Min(target, text.Length);
                result.Append(text.Substring(index, target - index));
                result.Append(tag);
                index += target - index + mark.Length;
                if (mark == Environment.NewLine && index < text.Length)
                    result.Append(Environment.NewLine);
            }

            if (index < text.Length)
                result.Append(text.Substring(index, text.Length - index));

            result
                .Replace(@"\_", "_")
                .Replace(@"\\", @"\");
            return result.ToString();
        }

        private (int, string, string) GetClosestIndex(int index, List<Token> tokens)
        {
            var closest = int.MaxValue;
            var tag = "";
            var mark = "";
            foreach (var token in tokens)
            {
                if (token.StartOpenMark < closest && token.StartOpenMark >= index)
                {
                    closest = token.StartOpenMark;
                    tag = $"<{token.Tag.HtmlTag}>";
                    mark = token.Tag.OpenMark;
                }

                if (token.EndCloseMark < closest && token.EndCloseMark >= index)
                {
                    closest = token.EndCloseMark - token.Tag.CloseMark.Length + 1;
                    tag = $"</{token.Tag.HtmlTag}>";
                    mark = token.Tag.CloseMark;
                }
            }
            return (closest, tag, mark);
        }
    }
}
