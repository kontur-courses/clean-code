using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;


namespace Markdown
{
    public class MdWrappingTagMatcher : IMdTagMatcher
    {
        private readonly string wrappingSequence;
        private readonly Lazy<Md> target;
        private string targetString;

        public MdWrappingTagMatcher(string wrappingSequence, HtmlTextWriterTag tag, Lazy<Md> target)
        {
            this.wrappingSequence = wrappingSequence;
            this.target = target;
            HtmlPairs = new HtmlPairReplacingsManager(tag);
        }

        public HtmlPairReplacingsManager HtmlPairs { get; }

        public int AmountSkippedCharsWhileMatching => wrappingSequence.Length - 1;

        public bool MatchMdTag(int machStartIndex)
        {
            if(targetString == null) 
                targetString = target.Value.RenderingString;
            
            var result = HtmlPairs.IsOpened ?  
                MatchCloseMdTag(machStartIndex):
                MatchOpenMdTag(machStartIndex);
            if (result) HtmlPairs.AddUniversalTag(machStartIndex, wrappingSequence.Length);
            return result;
        }

        private bool MatchOpenMdTag(int machStartIndex)
        {
            return targetString.Length >= wrappingSequence.Length + machStartIndex + 1 &&
                   !char.IsWhiteSpace(targetString[machStartIndex + wrappingSequence.Length]) &&
                   targetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
        }

        private bool MatchCloseMdTag(int machStartIndex)
        {
            return targetString.Length >= wrappingSequence.Length + machStartIndex &&
                   !char.IsWhiteSpace(targetString[machStartIndex - 1]) && 
                   targetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
        }
    }
}