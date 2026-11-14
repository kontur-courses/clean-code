using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    public class TagToken : IToken
    {
        public ITagInfo Tag { get; }

        public List<IToken> Children { get; }

        public TagToken(ITagInfo tag, List<IToken> children ) 
        { 
            Tag = tag;
            Children = children;
        }
    }
}
