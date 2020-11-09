using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Markdown
    {
        public static void MakeMarkDown()
        {
            var text = "";
            var tagsFounder = new TagsFounder(text);
            HTMLMaker.SaveToHTML(tagsFounder.Tags);
        }
    }
}
