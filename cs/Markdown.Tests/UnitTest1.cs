using NUnit.Framework;

namespace Markdown.Tests;

public class Tests
{
    private Markdown markdown;
    
    [SetUp]
    public void Setup()
    {
        markdown = new Markdown();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}