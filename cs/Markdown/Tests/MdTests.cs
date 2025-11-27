using NUnit.Framework;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Tokenizing;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Tokenizer tokenizer;
    private NodeParser parser;

    [SetUp]
    public void Setup()
    {
        tokenizer = new Tokenizer();
        parser = new NodeParser();
    }

    private void AssertStringsAreEqualAndPrint(string expected, string actual)
    {
        Console.WriteLine($"Expected: {expected}");
        Console.WriteLine($"Actual: {actual}");
        actual.Should().Be(expected);
    }

    [TestCase("test", "<p>test</p>")]
    [TestCase("test123", "<p>test123</p>")]
    [TestCase("two words", "<p>two words</p>")]
    public void Md_SimpleText(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("Here w\\e should\\ still have \\backslashes.\\",
        "<p>Here w\\e should\\ still have \\backslashes.\\</p>")]
    [TestCase("How about \\_underscores?_", "<p>How about _underscores?_</p>")]
    public void Md_EscapedText(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("__bold _italic_ text__", "<p><strong>bold <em>italic</em> text</strong></p>")]
    [TestCase("_italic __not bold__ text_", "<p><em>italic __not bold__ text</em></p>")]
    [TestCase("# Headers love __bold__ text too!", "<h1>Headers love <strong>bold</strong> text too!</h1>")]
    [TestCase("# And _italic_!", "<h1>And <em>italic</em>!</h1>")]
    public void Md_InnerTags(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("__bold__", "<p><strong>bold</strong></p>")]
    [TestCase("_italic_", "<p><em>italic</em></p>")]
    [TestCase("_expe_ct t_hi_s be corr_ect._", "<p><em>expe</em>ct t<em>hi</em>s be corr<em>ect.</em></p>")]
    [TestCase("_three italic words_", "<p><em>three italic words</em></p>")]
    [TestCase("__three bold words__", "<p><strong>three bold words</strong></p>")]
    public void Md_DefaultUnderscores(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("__forgot to close", "<p>__forgot to close</p>")]
    [TestCase("forgot to open_", "<p>forgot to open_</p>")]
    [TestCase("digits_12_3", "<p>digits_12_3</p>")]
    [TestCase("digits__12__3", "<p>digits__12__3</p>")]
    [TestCase("diff_erent wor_ds", "<p>diff_erent wor_ds</p>")]
    [TestCase("__no pair_", "<p>__no pair_</p>")]
    [TestCase("__intersection _goes__ here_", "<p>__intersection _goes__ here_</p>")]
    [TestCase("____", "<p>____</p>")]
    public void Md_IncorrectUnderscores(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("# It's level1 header", "<h1>It's level1 header</h1>")]
    [TestCase("## It's level2 header", "<h2>It's level2 header</h2>")]
    [TestCase("### And like that we go up to 6 level", "<h3>And like that we go up to 6 level</h3>")]
    public void Md_CorrectHeaders(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("#No whitespace no header", "<p>#No whitespace no header</p>")]
    [TestCase("No we can't # have mid-sentence header", "<p>No we can't # have mid-sentence header</p>")]
    public void Md_IncorrectHeaders(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("[Google](https://google.com)", "<p><a href=\"https://google.com\">Google</a></p>")]
    [TestCase("[_Google_](https://google.com)", "<p><a href=\"https://google.com\"><em>Google</em></a></p>")]
    public void Md_CorrectLinks(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }

    [TestCase("[[Google](https://google.com)", "<p>[<a href=\"https://google.com\">Google</a></p>")]
    [TestCase("[Google]((https://google.com)", "<p>[Google]((https://google.com)</p>")]
    [TestCase("[Google](https://google.com", "<p>[Google](https://google.com</p>")]
    public void Md_IncorrectLinks(string input, string expected)
    {
        var tokens = tokenizer.Tokenize(input);
        var node = parser.ParseLine(tokens);
        var html = node.ToHtml();
        AssertStringsAreEqualAndPrint(expected, html);
    }
}