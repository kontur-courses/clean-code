using System;

namespace Markdown
{
    public class TagConverter
    {
        private readonly IMdSpecification _specification;

        public TagConverter(IMdSpecification specification)
        {
            _specification = specification;
        }

        public string ToHTML(string mdTag) => _specification.MdToHTML[mdTag];
        public string ToMD(string htmlTag) => _specification.HTMLToMd[htmlTag];
    }
}
