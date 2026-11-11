using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class EmphasisTests
	{
		private Md _md = new();

		[Test]
		public void SimpleEm()
		{
			_md.Render("Текст, _окруженный_ тегом.").Should().Be("<p>Текст, <em>окруженный</em> тегом.</p>");
		}

		[Test]
		public void EmphasisWorksWithinSingleNewline()
		{
			_md.Render("_текст\nещё_").Should().Be("<p><em>текст ещё</em></p>");
		}

		[TestCase("сер_еди_не", "<p>сер<em>еди</em>не</p>")]
		[TestCase("_нач_але", "<p><em>нач</em>але</p>")]
		[TestCase("кон_це_", "<p>кон<em>це</em></p>")]
		public void EmInPartOfWord(string input, string expected)
		{
			_md.Render(input).Should().Be(expected);
		}

		[Test]
		public void EmBetweenWordsNotWorking()
		{
			_md.Render("ра_зных сл_овах").Should().Be("<p>ра_зных сл_овах</p>");
		}

		[Test]
		public void EmInsideDigitsNotWorking()
		{
			_md.Render("цифры_12_3").Should().Be("<p>цифры_12_3</p>");
		}

		[Test]
		public void EmUnpairedStaysAsIs()
		{
			_md.Render("_Непарные символы").Should().Be("<p>_Непарные символы</p>");
		}

		[Test]
		public void EmOpeningFollowedBySpaceStays()
		{
			_md.Render("это_ подчерки_").Should().Be("<p>это_ подчерки_</p>");
		}

		[Test]
		public void EmClosingPrecededBySpaceStays()
		{
			_md.Render("это _подчерки").Should().Be("<p>это _подчерки</p>");
		}
	}
}
