using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public IDictionary<ITag, IRule> Rules { get; private set; }
        public ITokenizer<ITag> Tokenizer { get; private set; }
        public IRenderer<ITag> Renderer { get; private set; }

        public Md()
        {
            // Rules = new DefaultRules();
            Tokenizer = new DefaultTokenizer<ITag>();
            Renderer = new DefaultRenderer<ITag>();
        }

        public string Render(string markdownText)
        {
            throw new NotImplementedException();
        }

        public Md SetTokenizer<TTag>(ITokenizer<TTag> tokenizer)
        {
            return this;
        }
        
        public Md SetRenderer<TTag>(IRenderer<TTag> renderer)
        {
            return this;
        }

        public Md SetRule(ITag tag, IRule rule)
        {
            return this;
        }
    }
}
