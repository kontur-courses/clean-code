using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class HTMLTags
    {
        private static List<TagSpecification> TagsByPriority = new List<TagSpecification>
        {
            new TagSpecification("<em>", "</em>", TagType.italics),
            new TagSpecification("<strong>", "</strong>", TagType.bold),
            new TagSpecification("<h1>", "</h1>", TagType.firstHeading),
            new TagSpecification("<h2>", "</h2>", TagType.secondHeading),
            new TagSpecification("<h3>", "</h3>", TagType.threeHeading),
            new TagSpecification("<h4>", "</h4>", TagType.fourHeading),
            new TagSpecification("<h5>", "</h5>", TagType.fiveHeading),
            new TagSpecification("<h6>", "</h6>", TagType.sixHeading)
        };

        public static List<TagSpecification> GetAllTags()
        {
            return new List<TagSpecification>(TagsByPriority);
        }
    }
}
