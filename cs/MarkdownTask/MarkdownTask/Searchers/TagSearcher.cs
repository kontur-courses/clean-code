using System;
using System.Collections.Generic;
using System.Text;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class TagSearcher : ITagSearcher
    {
        protected readonly string MdText;
        protected int CurrentPosition;

        protected TagSearcher(string mdText)
        {
            MdText = mdText.Trim();
        }

        public virtual List<Tag> SearchForTags(List<int> escapedChars)
        {
            throw new NotImplementedException();
        }

        protected virtual void PrepareToSearch()
        {
            CurrentPosition = 0;
        }

        protected virtual string GetFullPrefix(TagStyleInfo styleInfo)
        {
            var prefix = new StringBuilder();
            for (var i = 0; i < styleInfo.TagPrefix.Length; i++)
            {
                if (CurrentPosition + i >= MdText.Length)
                    break;
                prefix.Append(MdText[CurrentPosition + i]);
            }


            return prefix.ToString();
        }
    }
}