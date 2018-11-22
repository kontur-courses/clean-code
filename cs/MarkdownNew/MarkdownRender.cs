using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    static class MarkdownRender
    {
        public static string Render(string markdown)
        {
            var renderer = new Renderer();
            return renderer.ConvertFromMarkdownToHtml(markdown);
        }
    }
}
