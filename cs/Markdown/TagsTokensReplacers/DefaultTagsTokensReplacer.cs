using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.TagsTokensReplacers
{
    class DefaultTagsTokensReplacer
    {
        public static string ReplaceTagTokensInString(
            string text,
            IEnumerable<TagToken> replacableTags,
            Func<Tag, Tag> replaceTo)
        {
            if (text == null || replacableTags == null || replaceTo == null)
                throw new ArgumentNullException();

            var builder = new StringBuilder();
            var startIndex = 0;
            foreach (var tt in replacableTags.OrderBy(t => t.Token.StartIndex))
            {
                var endIndex = tt.Token.StartIndex;
                builder.Append(text, startIndex, endIndex - startIndex);
                builder.Append(replaceTo(tt.Tag).Value);
                startIndex = endIndex + tt.Token.Length;
            }
            builder.Append(text, startIndex, text.Length - startIndex);
            return builder.ToString();
        }
    }
}