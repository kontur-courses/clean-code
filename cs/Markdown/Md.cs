using System;
using System.Linq;
using Markdown.MdProcessing;
using Markdown.MdTokens;

namespace Markdown
{
    public class Md
    {
        private IMdProcessor mdProcessor;
        private MdTokenizer tokenizer;

        public void SetMdProcessor(IMdProcessor processor)
        {
            mdProcessor = processor;
        }
        
        public string Render(string text)
        {
            if(mdProcessor is null) throw new InvalidOperationException("No MdProcessor is set");
            tokenizer = new MdTokenizer();
            var result = tokenizer.MakeTokens(text).Select(token => mdProcessor.GetProcessedResult((MdToken)token));
            return string.Join(" ", result);
        }
    }
}