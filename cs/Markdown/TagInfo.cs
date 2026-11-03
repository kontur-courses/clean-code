using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    /// <summary>
    /// Класс, содержащий информацию о связке Markdown-HTML 
    /// </summary>
    public class TagInfo
    {
        public string MarkdownSymbol { get; }
        public string OpeningTag {  get; }
        public string ClosingTag { get; }
        public int Lenght => MarkdownSymbol.Length;

        public TagInfo(string symbol, string htmlTag)
        {
            this.MarkdownSymbol = symbol;
            this.OpeningTag = $"<{htmlTag}>";
            this.ClosingTag = $"</{htmlTag}>";
        }
    }
}
