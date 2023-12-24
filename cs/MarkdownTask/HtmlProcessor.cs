using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public static class HtmlProcessor
    {
        private static readonly Dictionary<TagInfo.TagType, string> htmlTags = new Dictionary<TagInfo.TagType, string>
        {
            {TagInfo.TagType.Header, "h1" },
            {TagInfo.TagType.Italic, "em" },
            {TagInfo.TagType.Strong, "strong" },
            {TagInfo.TagType.Empty, "" }
        };

        public static string Process(string text, ICollection<Token> tokens)
        {
            ;

            var sb = new StringBuilder();

            int head = 0;
            int shift = 0;
            foreach (var token in tokens.OrderBy(x => x.Position))
            {
                if (token.TagType == TagInfo.TagType.Empty)
                {
                    sb.Append(text.Substring(head, 1));
                    head = token.Position + 1;
                    continue;
                }

                sb.Append(text.Substring(head, token.Position - head));
                string h = string.Format("</{0}>", htmlTags[token.TagType]);

                if (token.Tag == TagInfo.Tag.Open)
                    h = string.Format("<{0}>", htmlTags[token.TagType]);

                sb.Append(h);
                head = token.Position + token.TagLength;
            }
            sb.Append(text.Substring(head, text.Length - head));

            return sb.ToString();
        }
    }
}