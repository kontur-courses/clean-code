using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        private MdProcessor mdProcessor;
        public string Render(string text)
        {
            var result = ParseForTokens(text).Select(token => mdProcessor.Process(token)).ToString();
            return result;
        }

        public IEnumerable<MdToken> ParseForTokens(string text)
        {
            throw new  NotImplementedException();
        }
    }
}