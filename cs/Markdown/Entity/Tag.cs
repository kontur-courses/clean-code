using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Entity
{
    public class Tag
    {
        public TagType Type { get; private set; }
        public string Text { get; private set; }

        public Tag(TagType tagType, string text)
        {
            Type = tagType;
            Text = text;
        }
    }
}
