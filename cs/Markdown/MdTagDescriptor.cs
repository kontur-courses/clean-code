using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdTagDescriptor
    {
        public readonly string Border;

        public readonly string LeftReplacement;
        public readonly string RightReplacement;

        private Action<MdToken> internalTokenProcessor = t => { };

        public MdTagDescriptor(string border, string leftReplacement, string rightReplacement)
        {
            Border = border;
            LeftReplacement = leftReplacement;
            RightReplacement = rightReplacement;
        }

        public void ProcessTokensInsideTag(List<MdToken> tokenList, MdTag parentTag)
        {
            var internalTokens = tokenList.GetSubTokens(parentTag.LeftBorder, parentTag.RightBorder);
            foreach (var subToken in internalTokens) internalTokenProcessor.Invoke(subToken);
        }

        public void SetInternalTokenProcessor(Action<MdToken> processor)
        {
            internalTokenProcessor = processor;
        }
    }
}