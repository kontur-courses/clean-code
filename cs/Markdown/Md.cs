using System.Text;

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
                AddTextWithTagsToBuilder(res, line + Environment.NewLine, tokens);
                if (!res.EndsWith(Environment.NewLine))
                    res.Append(Environment.NewLine);
            }
            res.Remove(res.Length - Environment.NewLine.Length, Environment.NewLine.Length);

            RemoveShielding(res, parser.Tags.Select(t => t.OpenMark));
            return res.ToString();
        }

        private void AddTextWithTagsToBuilder(StringBuilder builder, string text, List<Token> tokens)
        {
            var index = 0;
            while (index < text.Length)
            {
                var (token, isStart) = GetClosestToken(index, tokens);
                if (token == null!)  break;

                var target = isStart ? 
                    token.StartOpenMark : token.EndCloseMark - token.Tag.CloseMark.Length + 1;
                target = Math.Min(target, text.Length);
                builder.Append(text.AsSpan(index, target - index));
                builder.Append(isStart ? 
                    $"<{token.Tag.HtmlTag}{token.GetProperties(text, ref target)}>" 
                    : $"</{token.Tag.HtmlTag}>");
                var mark = isStart ? token.Tag.OpenMark : token.Tag.CloseMark;
                index = target + mark.Length;
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

        private (Token, bool) GetClosestToken(int index, List<Token> tokens)
        {
            var closest = int.MaxValue;
            Token res = null!;
            bool isStart = false;
            foreach (var token in tokens)
            {
                if (token.StartOpenMark < closest && token.StartOpenMark >= index)
                {
                    closest = token.StartOpenMark;
                    res = token;
                    isStart = true;
                }

                if (token.EndCloseMark < closest && token.EndCloseMark >= index)
                {
                    closest = token.EndCloseMark - token.Tag.CloseMark.Length + 1;
                    res = token;
                    isStart = false;
                }
            }
            return (res, isStart);
        }
    }
}
