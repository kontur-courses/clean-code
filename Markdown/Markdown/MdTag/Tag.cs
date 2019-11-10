using System;

namespace Markdown.MdTag
{
    public class Tag: ITag
    {
        private string htmlTag;
        private string tagContent;

        public Tag(string htmlTag, string tagContent)
        {
            throw new NotImplementedException();
        }

        public string WrapTagIntoHtml()
        {
            throw new NotImplementedException();
        }
    }
}
