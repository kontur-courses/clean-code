using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

public class EscapedTest
{
    [SetUp]
    public void Setup()
    {
        
    }
    
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
    
    [TestCase(@"\_\_Hello\_\_", ExpectedResult = @"__Hello__", TestName = "Avoid bold start")]
    [TestCase(@"\_Hello\_", ExpectedResult = @"_Hello_", TestName = "Avoid italic start")]
    [TestCase(@"Hell\o", ExpectedResult = @"Hell\o", TestName = "Not avoid non tag chars in word")]
    [TestCase(@"\Hello", ExpectedResult = @"\Hello", TestName = "Not avoid non tag chars at the word start")]
    [TestCase(@"Hello\", ExpectedResult = @"Hello\", TestName = "Not avoid non tag chars at the word end")]
    [TestCase(@"Hello\ World", ExpectedResult = @"Hello\ World", TestName = "Not avoid spaces")]
    [TestCase(@"\\", ExpectedResult = @"\", TestName = "Save escaped escape character")]
    public string RemoveEscapes_Remove_OnTest(string input)
    {
        return EscapeRules.RemoveEscapes(input);
    }
}