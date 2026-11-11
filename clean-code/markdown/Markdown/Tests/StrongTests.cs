using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class StrongTests
	{
		private Md _md = new();

		[Test]
		public void SimpleStrong()
		{
			_md.Render("Выделенный __текст__").Should().Be("<p>Выделенный <strong>текст</strong></p>");
		}

		[Test]
		public void StrongContainsEm()
		{
			_md.Render("__с _разными_ символами__").Should().Be("<p><strong>с <em>разными</em> символами</strong></p>");
		}

		[Test]
		public void EmptyUnderscoresStay()
		{
			_md.Render("____").Should().Be("<p>____</p>");
		}
	}
}
