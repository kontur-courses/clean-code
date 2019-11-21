using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Builders
{
    class BuilderRule
    {
        public string OpenTag { get; }
        public string CloseTag { get; }
        public TagType TypeTag { get; }

        public BuilderRule(TagType tagType, string openTag, string closeTag)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
            TypeTag = tagType;
        }
    }
}
