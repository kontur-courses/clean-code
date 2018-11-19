using Markdown.Data.Nodes;
using Markdown.Data.TagsInfo;

namespace MarkdownTests
{
    public class TestTreeBuilder
    {
        private readonly TokenTreeNode node;

        public static TestTreeBuilder Tree() => new TestTreeBuilder(new RootTreeNode());

        public static  TestTreeBuilder Tag(ITagInfo tagInfo) => new TestTreeBuilder(new TagTreeNode(tagInfo));

        public static TestTreeBuilder RawTag(ITagInfo tagInfo) => new TestTreeBuilder(new TagTreeNode(tagInfo) {IsRaw = true});

        public TestTreeBuilder(TokenTreeNode node)
        {
            this.node = node;
        }

        public TestTreeBuilder WithText(string text)
        {
            node.Children.Add(new TextTreeNode(text));
            return this;
        }

        public TestTreeBuilder WithTag(TokenTreeNode tag)
        {
            node.Children.Add(tag);
            return this;
        }

        public TestTreeBuilder WithSpace()
        {
            node.Children.Add(new TextTreeNode(" "));
            return this;
        }

        public TokenTreeNode Build() => node;
    }
}