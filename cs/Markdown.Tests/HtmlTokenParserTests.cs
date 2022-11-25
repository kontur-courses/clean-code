using System.Collections.Immutable;
using Markdown.Html;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class HtmlTokenParserTests
{
    [TestCase("# H", ExpectedResult = "<h1>H</h1>")]
    [TestCase("#H",  ExpectedResult = "<p>#H</p>")]
    public string Parse_HeadingTags_ShouldReturnHtmlFormatString(string markdownData)
    {
        return new HtmlBuilder().Build(new HtmlTokenParser().Parse(markdownData));
    }
}