using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "";
            var tagsFounder = new TagsFounder();
            var tags = tagsFounder.FindTags(text);
            HTMLMaker.SaveToHTML(tags);
        }
    }
}
