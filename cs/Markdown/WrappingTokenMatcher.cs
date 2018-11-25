using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Web.UI;
 
 
 namespace Markdown
 {
     public class WrappingTokenMatcher : ITokenMatcher
     {  
         private readonly string wrappingSequence;
 
         public WrappingTokenMatcher(string wrappingSequence, HtmlTextWriterTag tag)
         {
             this.wrappingSequence = wrappingSequence;
             this.Tag = tag;
         }
         
         public string TargetString { private get; set; }
         public HtmlTextWriterTag Tag { get; }
         
         public bool TryOpen(int matchStartIndex, out Range openTagRange)
         {
             openTagRange = default(Range);
             if (!MatchOpenMdTag(matchStartIndex)) 
                 return false;
             openTagRange = new Range(matchStartIndex,wrappingSequence.Length);
             return true;
         }

         public bool TryClose(int matchStartIndex, out Range closeTagRange)
         {
             closeTagRange = default(Range);
             if (!MatchCloseMdTag(matchStartIndex)) 
                 return false;
             closeTagRange = new Range(matchStartIndex,wrappingSequence.Length);
             return true;
         }

         private bool MatchOpenMdTag(int machStartIndex) =>
                    TargetString.Length >= wrappingSequence.Length + machStartIndex + 1 &&
                    !char.IsWhiteSpace(TargetString[machStartIndex + wrappingSequence.Length]) &&
                    TargetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;

         private bool MatchCloseMdTag(int machStartIndex) => machStartIndex > 0 &&
                    TargetString.Length >= wrappingSequence.Length + machStartIndex &&
                    !char.IsWhiteSpace(TargetString[machStartIndex - 1]) && 
                    TargetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
     }
 }