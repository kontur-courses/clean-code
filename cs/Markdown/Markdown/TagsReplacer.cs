using System.Collections.Generic;
using System;

namespace Markdown
{
    public class TagsReplacer<T, H> 
        where T: Tag
        where H: Tag
    {
        private readonly
            Dictionary<T, H> _tagToTag;

        public TagsReplacer(Dictionary<T, H> tagToTag)
        {
            _tagToTag = tagToTag;
        }

        public string ReplaceTagOnHtml(List<T> tags, string text)
        {
            return String.Empty;
        }
    }
}
