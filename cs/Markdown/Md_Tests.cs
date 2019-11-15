using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class Md_Render_Should
	{
		[TestCase(null, "", TestName = "Null")]
		[TestCase("", "", TestName = "Empty string")]
		[TestCase("_italic_", "<em>italic</em>", TestName = "Italic")]
		[TestCase("plain text", "plain text", TestName = "Plain text")]
		public void ReturnCorrectHtmlText(string markdownText, string expectedHtmlText)
		{
			var actualHtmlText = Md.Render(markdownText);
			actualHtmlText.Should().Be(expectedHtmlText);
		}
	}
}