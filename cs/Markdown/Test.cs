using NUnit.Framework;

namespace Markdown;

[TestFixture]
public class Test
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }
    [Test]
    public void TTest()
    {
        var a = md.Render("a\\bc");
        Console.Write(a);
    }
    [Test]
    public void ATest()
    {
        var a = md.Render("_abc_");
        Console.Write(a);
    }
    [Test]
    public void BTest()
    {
        var a = md.Render("__abc__");
        Console.Write(a);
    }
    [Test]
    public void CTest()
    {
        var a = md.Render("\\__a_");
        Console.Write(a);
    }
    [Test]
    public void DTest()
    {
        var a = md.Render("__ab__c");
        Console.Write(a);
    }
}