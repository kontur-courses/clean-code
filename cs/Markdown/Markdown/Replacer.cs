using System.Collections.Generic;
using System;

namespace Markdown
{
    public class Replacer<T> where T: Tag
    {
        private readonly
            Dictionary<MdTag, HtmlTag> _markdownToHtml = new Dictionary<MdTag, HtmlTag>();
        
        public string ReplaceTagOnHtml(Dictionary<T, (int startTagIndex, int closeTagIndex)> tags, string text)
        {
            return String.Empty;
        }
    }
}
