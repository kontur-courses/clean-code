using FluentAssertions;
using Markdown;

namespace MarkdownTest;

[TestFixture]
public class TreeTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ToHTMLStringTest()
    {
        var tree = new TokenTree("_a_ _a_  _a_");
        tree.TryAddToken(0, 2, new EmTag());
        tree.TryAddToken(4, 6, new EmTag());
        tree.TryAddToken(9, 11, new EmTag());
        tree
            .ToHTMLString()
            .Should()
            .Be("<em>a</em> <em>a</em>  <em>a</em>");
    }

    [Test]
    public void TryAddTokenTest()
    {
        var res = new List<bool>();
        var tree = new TokenTree("123123123");
        res.Add(tree.TryAddToken(1, 4, new EmptyTag()));
        res.Add(tree.TryAddToken(2, 3, new EmptyTag()));
        res.Add(tree.TryAddToken(5, 6, new EmptyTag()));
        res.Add(tree.TryAddToken(3, 5, new EmptyTag()));
        res
            .Should()
            .BeEquivalentTo(new[] { true, true, true, false });
        tree.GetLeafs()
            .Select(node => node.LeftBorder)
            .ToArray()
            .Should()
            .BeEquivalentTo(new[] { 2, 5 });
    }
}