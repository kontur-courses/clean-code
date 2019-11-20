using System.Collections.Generic;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Serialization;
using System;

namespace Markdown
{
    static class MdTags
    {
        private static readonly List<TagSpecification> TagsByPriority = new List<TagSpecification>
        {
            new TagSpecification("__", "__", TagType.bold),
            new TagSpecification("_", "_", TagType.italics, new List<TagType>(){TagType.bold}),
            new TagSpecification("######", "######", TagType.sixHeading, endWithEndLine:true),
            new TagSpecification("#####", "#####", TagType.fiveHeading, endWithEndLine:true),
            new TagSpecification("####", "####", TagType.fourHeading, endWithEndLine:true),
            new TagSpecification("###", "###", TagType.threeHeading, endWithEndLine:true),
            new TagSpecification("##", "##", TagType.secondHeading, endWithEndLine:true),
            new TagSpecification("#", "#", TagType.firstHeading, endWithEndLine:true),
            new TagSpecification("", Environment.NewLine, TagType.EndLine)
        };

        public static List<TagSpecification> GetAllTags()
        {
            return new List<TagSpecification>(TagsByPriority);
        }
    }
}
