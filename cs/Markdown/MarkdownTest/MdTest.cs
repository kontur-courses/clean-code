using FluentAssertions;
using Markdown;

namespace MarkdownTest;

[TestFixture]
public class MdTest
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }

    [TestCase("", "", TestName = "EmptyStringTest")]
    [TestCase("a", "a", TestName = "TextWithoutTag")]
    [TestCase("_", "_", TestName = "SingleTag")]
    [TestCase("__", "__", TestName = "TwoUnderscores")]
    [TestCase("_1_", "_1_", TestName = "ContainNumbersTest")]
    [TestCase("_a_", "<em>a</em>", TestName = "OneEmTag")]
    [TestCase("_a_ _b_", "<em>a</em> <em>b</em>", TestName = "TwoEmTags")]
    [TestCase("_a__b_", "<em>a</em><em>b</em>", TestName = "TwoEmTagsWithoutSpace")]
    [TestCase("_a_ _b_ _c_", "<em>a</em> <em>b</em> <em>c</em>", TestName = "ThreeEmTags")]
    [TestCase("_a _bc_ _bc_ d_", "<em>a <em>bc</em> <em>bc</em> d</em>", TestName = "NestedTags")]
    public void EmTagTest(string mdstring, string result)
    {
        md.Render(mdstring)
            .Should()
            .Be(result);
        md = new Md();
        md.Render(" " + mdstring)
            .Should()
            .Be(" " + result);
        md = new Md();
        md.Render(mdstring + " ")
            .Should()
            .Be(result + " ");
        md.Render(" " + mdstring + " ")
            .Should()
            .Be(" " + result + " ");
    }


    [TestCase("__a__", "<strong>a</strong>", TestName = "OneStrongTags")]
    [TestCase("__a__ __b__", "<strong>a</strong> <strong>b</strong>", TestName = "TwoStrongTags")]
    [TestCase("__a____b__", "<strong>a</strong><strong>b</strong>", TestName = "TwoStrongTagWithoutSpaces")]
    [TestCase("__a__ __b__ __c__", "<strong>a</strong> <strong>b</strong> <strong>c</strong>",
        TestName = "ThreeStrongTags")]
    public void StrongTagTest(string mdstring, string result)
    {
        md.Render(mdstring)
            .Should()
            .Be(result);
        md = new Md();
        md.Render(" " + mdstring)
            .Should()
            .Be(" " + result);
        md = new Md();
        md.Render(mdstring + " ")
            .Should()
            .Be(result + " ");
        md.Render(" " + mdstring + " ")
            .Should()
            .Be(" " + result + " ");
    }

    [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>")]
    [TestCase("_a __b__ c_", "<em>a __b__ c</em>")]
    public void TagInteractionTest(string mdstring, string result)
    {
        md.Render(mdstring)
            .Should()
            .Be(result);
    }
}