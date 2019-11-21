using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenConverters
{
    public sealed class HtmlConverter : TokenConverter
    {
        public override string FileType { get; } = "html";

        protected override IReadOnlyDictionary<TagType, (string start, string end)> TagAssociationsWithType { get; } =
            new Dictionary<TagType, (string start, string end)>()
            {
                [TagType.Bold] = ("<strong>", "</strong>"),
                [TagType.Italics] = ("<em>", "</em>"),
            };

        public override string ConvertTokens(IEnumerable<Token> tokens)
        {
            var str = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Type != TagType.None)
                {
                    var (start, end) = TagAssociationsWithType[token.Type];
                    str.Append(start);
                    str.Append(token.Text);
                    str.Append(end);
                }
                else
                    str.Append(token.Text);
            }

            return str.ToString();
        }
    }
}