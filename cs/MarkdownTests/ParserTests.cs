using FluentAssertions;
using Markdown;
using Markdown.AstNodes;
using Markdown.Enums;
using NUnit.Framework;

namespace MarkdownTests;

public class ParserTests
{
    private MarkdownParser parser;
    private MarkdownLexer lexer;

    [SetUp]
    public void Setup()
    {
        parser = new MarkdownParser();
        lexer = new MarkdownLexer();
    }

    [Test]
    [Description("Ручная проверка корректности AST для тегов Heading, Italic, Bold, Text")]
    public void Parse_ReturnsAst_WithAllTags()
    {
        const string md = "# a b\\_c _d_ __e__\n___f___ 1_234";
        var tokens = lexer.Tokenize(md);
        var actual = parser.Parse(tokens);

        var space = new TextMarkdownNode(" ");
        var newLine = new TextMarkdownNode("\n");
        
        var heading = new HeadingMarkdownNode();
        heading.Children.Add(new TextMarkdownNode("a"));
        heading.Children.Add(space);
        heading.Children.Add(new TextMarkdownNode("b"));
        heading.Children.Add(new TextMarkdownNode("_"));
        heading.Children.Add(new TextMarkdownNode("c"));
        heading.Children.Add(space);
        
        var italicD = new ItalicMarkdownNode();
        italicD.Children.Add(new TextMarkdownNode("d"));
        
        heading.Children.Add(italicD);
        heading.Children.Add(space);
        
        var boldE = new BoldMarkdownNode();
        boldE.Children.Add(new TextMarkdownNode("e"));
        
        heading.Children.Add(boldE);
        
        var boldF = new BoldMarkdownNode();
        var italicF = new ItalicMarkdownNode();
        italicF.Children.Add(new TextMarkdownNode("f"));
        boldF.Children.Add(italicF);
        
        var text1_234 = new TextMarkdownNode("1_234");

        var expected = new RootMarkdownNode();
        expected.Children.Add(heading);
        expected.Children.Add(newLine);
        expected.Children.Add(boldF);
        expected.Children.Add(space);
        expected.Children.Add(text1_234);
        
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    [Description("Глубина дерева должна быть не больше 5: Root->Heading->Bold->Italic->Text")]
    public void Parse_ReturnsAst_WithDepthLessThanFive()
    {
        var dir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsData");
        var md = File.ReadAllText(Path.Combine(dir, "test.txt"));
        var tokens = lexer.Tokenize(md);
        var ast = parser.Parse(tokens);
        var depth = GetAstDepth(ast);
        depth.Should().BeLessThanOrEqualTo(5);
    }

    [Test]
    [Description("Вложенность должна быть корректной: Root->Heading->Bold->Italic->Text")]
    public void Parse_ReturnsAst_WithCorrectNesting()
    {
        var dir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsData");
        var md = File.ReadAllText(Path.Combine(dir, "test.txt"));
        var tokens = lexer.Tokenize(md);
        var ast = parser.Parse(tokens);
        CheckNesting(ast);
    }
    
    [Test]
    public void Parse_Ast_NotHaveEmptyItalic()
    {
        var dir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsData");
        var md = File.ReadAllText(Path.Combine(dir, "test.txt"));
        var tokens = lexer.Tokenize(md);
        var ast = parser.Parse(tokens);
        AstNotHaveEmpty<ItalicMarkdownNode>(ast);
    }
    
    [Test]
    public void Parse_Ast_NotHaveEmptyBold()
    {
        var dir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsData");
        var md = File.ReadAllText(Path.Combine(dir, "test.txt"));
        var tokens = lexer.Tokenize(md);
        var ast = parser.Parse(tokens);
        AstNotHaveEmpty<BoldMarkdownNode>(ast);
    }

    private void AstNotHaveEmpty<TDisallowed>(MarkdownNode node) where TDisallowed : MarkdownNode
    {
        if (node is TextMarkdownNode) return;
        if (node is IMarkdownNodeWithChildren nodeWithChildren)
        {
            if (nodeWithChildren is TDisallowed)
                nodeWithChildren.Children.Should().NotBeEmpty();
            foreach (var child in nodeWithChildren.Children)
                AstNotHaveEmpty<TDisallowed>(child);
        }
        else throw new ArgumentException($"Not expected node type: {node.Type}");
    }

    private int GetAstDepth(MarkdownNode node, int level = 1)
    {
        var maxLevel = level;
        if (node is not IMarkdownNodeWithChildren nodeWithChildren) return maxLevel;
        foreach (var child in nodeWithChildren.Children)
            maxLevel = Math.Max(level, GetAstDepth(child, level + 1));
        return maxLevel;
    }

    private void CheckNesting(MarkdownNode node)
    {
        if (node is TextMarkdownNode) return;
        if (node is IMarkdownNodeWithChildren nodeWithChildren)
        {
            var childrenTypes = nodeWithChildren.Children.Select(n => n.Type).ToHashSet();
            var allowedTypes = GetAllowedChildrenFor(node);

            foreach (var type in childrenTypes)
                allowedTypes.Should().Contain(type);

            foreach (var child in nodeWithChildren.Children)
                CheckNesting(child);
        }
        else throw new ArgumentException($"Not expected node type: {node.Type}");
    }

    private MarkdownNodeName[] GetAllowedChildrenFor(MarkdownNode node)
    {
        return node switch
        {
            RootMarkdownNode =>
                [MarkdownNodeName.Bold, MarkdownNodeName.Italic, MarkdownNodeName.Text, MarkdownNodeName.Heading],
            HeadingMarkdownNode => [MarkdownNodeName.Bold, MarkdownNodeName.Italic, MarkdownNodeName.Text],
            BoldMarkdownNode => [MarkdownNodeName.Italic, MarkdownNodeName.Text],
            ItalicMarkdownNode => [MarkdownNodeName.Text],
            TextMarkdownNode => [],
            _ => throw new ArgumentException($"Not expected node type: {node.Type}")
        };
    }
}