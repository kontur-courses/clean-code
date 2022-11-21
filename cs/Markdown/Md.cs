using System;

namespace Markdown
{
    public class Md<TagOut, TagIn> 
        where TagOut : MarkdownTag
        where TagIn : ITag
    {
        public IRules Rules { get; }
        public ITokenizer<TagOut> Tokenizer { get; }
        public IRenderer<TagIn> Renderer { get; }

        public Md(ITokenizer<TagOut> tokenizer, IRenderer<TagIn> renderer,
            IRules rules)
        {
            Tokenizer = tokenizer;
            Renderer = renderer;
            Rules = rules;
        }

        public string Render(string markdownText)
        {
            throw new NotImplementedException();
        }
    }
}
