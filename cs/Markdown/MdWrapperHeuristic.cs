using System;
using System.IO;
using System.Web.UI;


namespace Markdown
{
    public class MdWrapperHeuristic : IMdHeuristic
    {
        private readonly string wrappingSequence;
        private readonly Md target;
        public HtmlTextWriterTag Tag { get; }
        

        public MdWrapperHeuristic(string wrappingSequence, HtmlTextWriterTag tag, Md target)
        {
            this.wrappingSequence = wrappingSequence;
            this.target = target;
            Tag = tag;
        }

        public int OpenHeuristic(int index)
        {
            throw new NotImplementedException();
        }

        public int CloseHeuristic(int index)
        {
            throw new NotImplementedException();
        }
    }
}