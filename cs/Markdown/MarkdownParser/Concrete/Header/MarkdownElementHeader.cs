using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Header
{
    public class MarkdownElementHeader : MarkdownElement
    {
        public MarkdownElementHeader(Token headerToken, ICollection<MarkdownElement> content) 
            : base(content.SelectMany(t => t.Tokens).Prepend(headerToken).ToArray())
        {
            Content = content;
        }
        
        public ICollection<MarkdownElement> Content { get; }
    }
}