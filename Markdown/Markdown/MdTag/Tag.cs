using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework.Constraints;

namespace Markdown.MdTag
{
    public class Tag: ITag
    {
        public string mdTag = "";
        public string htmlTag = "";
        public string fullToken = "";
        public readonly List<string> tagContent = new List<string>();
        public readonly List<Tag> NestedTags = new List<Tag>();

        public Tag()
        {
            this.tagContent = new List<string>();
        }

        public string WrapTagIntoHtml()
        {
            var finalString = new StringBuilder();
            if (htmlTag != "") finalString.Append("<" + htmlTag + ">");
            FillTagByContent(finalString);
            if (htmlTag != "") finalString.Append("</" + htmlTag + ">");
            return finalString.ToString();
        }

        private void FillTagByContent(StringBuilder finalString)
        {
            foreach (var elementToPrint in tagContent.Zip(NestedTags, (first, second) => (first, second)))
            {
                finalString.Append(elementToPrint.first + "");
                finalString.Append(elementToPrint.second.WrapTagIntoHtml() + "");
            }
            finalString.Append(tagContent.Last());
        }

        public void AddNestedTag(Tag tag)
        {
            NestedTags.Add(tag);
        }

        public string GetLastContent() => tagContent.Last();

        public void AddTagContent(string content)
        {
            tagContent.Add(content);
        }
    }
}
