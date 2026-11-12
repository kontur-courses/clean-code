using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    /// <summary>
    /// Класс, содержащий информацию о связке Markdown-HTML 
    /// </summary>
    public class PairedTagInfo : ITagInfo
    {
        public string MarkdownSymbol { get; }
        public string OpeningTag { get; }
        public string ClosingTag { get; }
        public int Length => MarkdownSymbol.Length;
        public PairedTagInfo(string symbol, string htmlTag)
        {
            MarkdownSymbol = symbol;
            OpeningTag = $"<{htmlTag}>";
            ClosingTag = $"</{htmlTag}>";
        }
    }
}
