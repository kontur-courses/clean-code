using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdProcessor
    {
        private Dictionary<string, Func<MdToken, string>> tokenProcessors;

        public MdProcessor()
        {
            tokenProcessors = new Dictionary<string, Func<MdToken, string>>();
        }
        public string Process(MdToken token)
        {
            return tokenProcessors[token.SpecialSymbol](token);
        }
    }
}