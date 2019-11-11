using System;
using System.Collections.Generic;
using Markdown.MdTokens;

namespace Markdown.MdProcessing
{
    public class MdToHtmlProcessor : IMdProcessor
    {
        private readonly Dictionary<string, Func<MdToken, string>> tokenProcessors;

        public MdToHtmlProcessor()
        {
            tokenProcessors = new Dictionary<string, Func<MdToken, string>>();
            tokenProcessors["#"] = ProcessEmphasis;
        }

        public string GetProcessedResult(MdToken token)
        {
            return tokenProcessors[token.SpecialSymbolBeginning](token);
        }

        private static string ProcessEmphasis(MdToken token)
        {
            throw new NotImplementedException();
        }
    }
}