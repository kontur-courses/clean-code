using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class TokenReader
    {
        private readonly IReadOnlyList<Tag> tags;
        private readonly string markdownString;


        public TokenReader(IReadOnlyList<Tag> tags, string markdownString)
        {
            this.markdownString = markdownString;
            this.tags = tags;
        }

        public Token TryReadUpToNextTag(int pos)
        {
            for (var index = pos; index < markdownString.Length; index++)
            {
                var tag = TryGetTagFrom(index);
                if (tag != null)
                    return new Token(index, -1, tag);
            }
            return new Token(markdownString.Length, -1, null);
        }

        private Tag TryGetTagFrom(int pos)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                var curentMdTag = tags[i].markdownTag;
                if (pos + curentMdTag.Length >= markdownString.Length)
                    continue;
                if (markdownString.Substring(pos, curentMdTag.Length) == curentMdTag)
                    return tags[i];
            }
            return null;
        }


    }
}
