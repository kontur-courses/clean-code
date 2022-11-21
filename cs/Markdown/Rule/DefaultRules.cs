using System.Collections.Generic;

namespace Markdown
{
    public class DefaultRules : IRules
    {
        public IDictionary<ITag, IRule> Rules { get; } = new Dictionary<ITag, IRule>()
        {
            [MarkdownTag.Italic] = new Rule(tag => HTMLTag.Emphasys),
            [MarkdownTag.Bold] = new Rule(tag => HTMLTag.Strong),
            [MarkdownTag.Heading(1)] = new Rule(tag => HTMLTag.Heading(1)),
            [MarkdownTag.Heading(2)] = new Rule(tag => HTMLTag.Heading(2)),
            [MarkdownTag.Heading(3)] = new Rule(tag => HTMLTag.Heading(3)),
            [MarkdownTag.Heading(4)] = new Rule(tag => HTMLTag.Heading(4)),
            [MarkdownTag.Heading(5)] = new Rule(tag => HTMLTag.Heading(5)),
            [MarkdownTag.Heading(6)] = new Rule(tag => HTMLTag.Heading(6)),
        };
    }
}
