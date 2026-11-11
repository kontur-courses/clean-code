using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class HeadingTests
	{
		private Md _md = new();

		[Test]
		public void Simple_Heading()
		{
			_md.Render("# Заголовок").Should().Be("<h1>Заголовок</h1>");
		}

		[Test]
		public void HeadingWithoutSpaceIsText()
		{
			_md.Render("#Заголовок").Should().Be("<p>#Заголовок</p>");
		}
	}
}
