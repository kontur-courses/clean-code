using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class EscapingTests
	{
		private Md _md = new();

		[Test]
		public void EscapedUnderscoresNotParsed()
		{
			_md.Render("\\_Вот это\\_").Should().Be("<p>_Вот это_</p>");
		}

		[Test]
		public void EscapedBackslashStays()
		{
			_md.Render("сим\\\\волы").Should().Be("<p>сим\\волы</p>");
		}

		[Test]
		public void LoneBackslashStays()
		{
			_md.Render("сим\\волы").Should().Be("<p>сим\\волы</p>");
		}

		[Test]
		public void DoubleEscapedUnderscoreBecomesEm()
		{
			_md.Render("\\\\_вот это\\\\_").Should().Be("<p>\\<em>вот это\\</em></p>");
		}
	}
}
