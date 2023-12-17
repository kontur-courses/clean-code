using Markdown.Tokenizer;

namespace MarkDownTests;

public class TokenizerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize("#данный текст __ содержит __ в _себе_ 9 токенов, # вот");
        Assert.AreEqual(9,tokens.Count());
    }
}