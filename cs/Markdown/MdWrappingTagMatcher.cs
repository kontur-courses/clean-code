using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Web.UI;
 
 
 namespace Markdown
 {
     public class MdWrappingTagMatcher : IMdTagMatcher
     {
 
         public MdWrappingTagMatcher(string wrappingSequence, HtmlTextWriterTag tag)
         {
             this.tag = tag;
             this.wrappingSequence = wrappingSequence;
         }

         //Md.Shitpile method leaked in a form of public fields   
         public HtmlTextWriterTag tag;
         public string wrappingSequence;
         
         public string TargetString { private get; set; }
 
         public int AmountSkippedCharsWhileMatching => wrappingSequence.Length - 1;
 
         public bool MatchOpenMdTag(int machStartIndex) =>
                    TargetString.Length >= wrappingSequence.Length + machStartIndex + 1 &&
                    !char.IsWhiteSpace(TargetString[machStartIndex + wrappingSequence.Length]) &&
                    TargetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
 
         public bool MatchCloseMdTag(int machStartIndex) => machStartIndex > 0 &&
                    TargetString.Length >= wrappingSequence.Length + machStartIndex &&
                    !char.IsWhiteSpace(TargetString[machStartIndex - 1]) && 
                    TargetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
     }
 }