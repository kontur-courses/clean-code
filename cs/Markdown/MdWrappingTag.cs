using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;


namespace Markdown
{
    public class MdWrappingTag : IMdTag
    {
        private readonly string wrappingSequence;
        private readonly Md target;
        public HtmlTextWriterTag Tag { get; }
        

        public MdWrappingTag(string wrappingSequence, HtmlTextWriterTag tag, Md target)
        {
            this.wrappingSequence = wrappingSequence;
            this.target = target;
            Tag = tag;
        }

        public IEnumerable<StringChange> GetChanges { get; }
        public bool IsOpened { get; }
        public bool CheckTag(int index)
        {
            throw new NotImplementedException();
        }
    }
}