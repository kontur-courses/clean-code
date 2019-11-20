using System;
using System.Linq;
using Markdown.MdProcessing;
using Markdown.MdTokens;

namespace Markdown
{
    public class Md
    {
        private readonly MdTokenizer tokenizer;

        public Md()
        {
            tokenizer = new MdTokenizer();
        }

        public string RenderUsingCustomProcessor(string text, IMdProcessor processor)
        {
            if(processor is null) throw new ArgumentNullException(nameof(processor));
            var result = tokenizer.MakeTokens(text).Select(token => processor.GetProcessedResult((MdToken)token));
            return string.Join(" ", result);
        }

        public string RenderToHtml(string text)
        {
            return RenderUsingCustomProcessor(text, new MdToHtmlProcessor());
        }
    }
}