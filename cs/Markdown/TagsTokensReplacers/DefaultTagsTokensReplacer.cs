using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = text;
            foreach (var tt in replacableTags.OrderByDescending(t => t.Token.StartIndex))
                result =
                    result
                    .Remove(tt.Token.StartIndex, tt.Token.Count)
                    .Insert(tt.Token.StartIndex, replaceTo(tt.Tag).Value);
            return result;
        }
    }
}