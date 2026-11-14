using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    internal class CascadTagToken : TagToken
    {
        public CascadTagToken(ITagInfo tag, List<IToken> children) : base(tag, children) { }
    }
}
