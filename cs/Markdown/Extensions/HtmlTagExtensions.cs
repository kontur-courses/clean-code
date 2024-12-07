using Markdown.Domain.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Extensions
{
    public static class HtmlTagExtensions
    {
        public static string GetMarkup(this HtmlTag htmlTag)
        {
            if (htmlTag.Tag == Tag.EscapedSymbol)
                return "";

            var format = htmlTag.IsClosing ? "</{0}>" : "<{0}>";

            return string.Format(format, htmlTag.Markup);
        }
    }
}
