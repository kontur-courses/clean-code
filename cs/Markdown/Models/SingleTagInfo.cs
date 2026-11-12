using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    public class SingleTagInfo : ITagInfo
    {
        public string MarkdownPrefix { get; }
        public string OpeningTag { get; }
        public string ClosingTag { get; }

        public SingleTagInfo(string prefix, string htmlTag)
        {
            MarkdownPrefix = prefix;
            OpeningTag = $"<{htmlTag}>";
            ClosingTag = $"</{htmlTag}>";
        }
    }
}
