using System;
using System.IO;
using System.Web.UI;


namespace Markdown
{
    public class MdWrapperHeuristic : IMdHeuristic
    {
        private readonly string wrappingSequence;

        public MdWrapperHeuristic(string wrappingSequence, HtmlTextWriterTag tag, Md target)
        {
            this.wrappingSequence = wrappingSequence;
        }

        public int OpenHeuristicLength => wrappingSequence.Length + 1;
        public bool OpenHeuristic(char[] str)
        {
            throw new NotImplementedException();
        }

        public int CloseHeuristicLength => wrappingSequence.Length + 1;
        public bool CloseHeuristic(char[] str)
        {
            throw new NotImplementedException();
        }
    }
}