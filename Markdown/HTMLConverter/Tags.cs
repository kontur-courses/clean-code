using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.HTMLConverter
{
    class Tags
    {
        public static Dictionary<TokenProperty, string> Open = new() {
            { TokenProperty.Bold, "<strong>"},
            { TokenProperty.Head, "<h1>"},
            { TokenProperty.Italic, "<em>"},
            { TokenProperty.Paragraph, string.Empty},
            { TokenProperty.Normal, string.Empty},
            { TokenProperty.Link , "<a href="}
        };

        public static Dictionary<TokenProperty, string> Close = new() {
            { TokenProperty.Bold, "</strong>"},
            { TokenProperty.Head, "</h1>\n"},
            { TokenProperty.Italic, "</em>"},
            { TokenProperty.Paragraph, "\n"},
            { TokenProperty.Normal, string.Empty},
            {TokenProperty.Link , "</a>"}
        };
    }
}

