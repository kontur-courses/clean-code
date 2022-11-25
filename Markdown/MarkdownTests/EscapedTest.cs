using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

public class EscapedTest
{
    [SetUp]
    public void Setup() { }
    
    [TestCase(@"\_", 1, ExpectedResult = false, TestName = "Avoid characters")]
    [TestCase(@"\\_", 2, ExpectedResult = true, TestName = "Avoid escape characters")]
    [TestCase(@"\\\_", 3, ExpectedResult = false, TestName = "Recursive escape characters")]
    [TestCase(@"\_", 50, ExpectedResult = true, TestName = "Index lager then string")]
    [TestCase(@"\_", -5, ExpectedResult = true, TestName = "Index is below zero")]
    [TestCase(@"\_", 0, ExpectedResult = true, TestName = "Index is zero")]
    public bool Render_Format_OnTest(string input, int index)
    {
        return EscapeRules.IsNotEscaped(input, index);
    }
}