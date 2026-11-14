using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    internal class CascadTagInfo : ITagInfo
    {
        public string OpeningTag => "<ul>";
        public string ClosingTag => "</ul>";
    }
}
