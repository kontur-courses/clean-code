using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using Markdown;

namespace Markdown
{
    public class Md
    {
        private TokenParser parser;

        public Md()
        {
            parser = new TokenParser();
        }

        public string Render(string text)
        {
            var res = new StringBuilder();
            foreach (var line in text.Split(Environment.NewLine))
            {
                var tokens = parser.ParseTokens(line + Environment.NewLine);
                AddTagsToTextAndAddToBuilder(res, line + Environment.NewLine, tokens);
                if (!res.EndsWith(Environment.NewLine))
                    res.Append(Environment.NewLine);
            }
            res.Remove(res.Length - Environment.NewLine.Length, Environment.NewLine.Length);

            RemoveShielding(res, parser.Tags.Select(t => t.OpenMark));
            return res.ToString();
        }

        private void AddTagsToTextAndAddToBuilder(StringBuilder builder, string text, List<Token> tokens)
        {
            var index = 0;
            while (index < text.Length)
            {
                var (target, tag, mark) = GetClosestIndex(index, tokens);
                target = Math.Min(target, text.Length);
                builder.Append(text.AsSpan(index, target - index));
                builder.Append(tag);
                index += target - index + mark.Length;
            }

            if (index < text.Length)
                builder.Append(text.AsSpan(index, text.Length - index));
        }

        private void RemoveShielding(StringBuilder builder, IEnumerable<string> marks)
        {
            foreach (var mark in marks)
                builder.Replace($@"\{mark}", $"{mark}");
            builder
                .Replace(@"\\", @"\");
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
