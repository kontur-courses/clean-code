using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;


namespace Markdown
{
    public class MdWrappingTagMatcher : IMdTagMatcher
    {
        private readonly Lazy<Md> target;
        private string targetString;
        private readonly Func<bool> externalCheck;

        public MdWrappingTagMatcher(string wrappingSequence, HtmlTextWriterTag tag, Lazy<Md> target, Func<bool> externalCheck = null)
        {
            WrappingSequence = wrappingSequence;
            this.target = target;
            this.externalCheck = externalCheck;
            HtmlPairs = new HtmlPairReplacingsManager(tag);
        }

        public HtmlPairReplacingsManager HtmlPairs { get; }
        public string WrappingSequence { get; }

        public int AmountSkippedCharsWhileMatching => WrappingSequence.Length - 1;

        public bool MatchMdTag(int machStartIndex)
        {
            if(targetString == null) 
                targetString = target.Value.RenderingString;
            
            var result = HtmlPairs.IsOpened ?  
                MatchCloseMdTag(machStartIndex):
                MatchOpenMdTag(machStartIndex);
            if (result) HtmlPairs.AddUniversalTag(machStartIndex, WrappingSequence.Length);
            return result;
        }

        private bool MatchOpenMdTag(int machStartIndex)
        {
            return (externalCheck?.Invoke() ?? true) &&
                   targetString.Length >= WrappingSequence.Length + machStartIndex + 1 &&
                   !char.IsWhiteSpace(targetString[machStartIndex + WrappingSequence.Length]) &&
                   targetString.Substring(machStartIndex, WrappingSequence.Length) == WrappingSequence;
        }

        private bool MatchCloseMdTag(int machStartIndex)
        {
            return (externalCheck?.Invoke() ?? true) &&
                   targetString.Length >= WrappingSequence.Length + machStartIndex &&
                   !char.IsWhiteSpace(targetString[machStartIndex - 1]) && 
                   targetString.Substring(machStartIndex, WrappingSequence.Length) == WrappingSequence;
        }
    }
}