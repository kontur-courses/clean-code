using FluentAssertions;
using Markdown.Interfaces;

namespace Markdown.Tests;

public class MdGenerateTreeTests
{
    public ILexer Parser = new MdParser();


    /*[Test]
    public void MdParser_GenerateTree_OnlyHeader()
    {
        var text = "# ";

        var tokens = Parser.Tokenize(text);

        var tree = Parser.GenerateTree(tokens);

        var expected = new NodeHeader() { };

        tree.Should().BeEquivalentTo(expected);
    }*/
    
    
}