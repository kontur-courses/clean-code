using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
	[TestFixture]
	public class AbzatsTests
	{
		private Md _md = new();

		[Test]
		public void SingleNewlineIsNotParagraphBreak()
		{
			// Одиночный перенос - пробел при рендере.
			_md.Render("первая строка\nвторая строка").Should().Be("<p>первая строка вторая строка</p>");
		}

		[Test]
		public void DoubleNewlineIsParagraphBreak()
		{
			_md.Render("Это первый абзац.\n\nЭто второй абзац.").Should().Be("<p>Это первый абзац.</p><p>Это второй абзац.</p>");
		}

		[Test]
		public void UnpairedEmphasisDoesNotMergeAcrossAbzats()
		{
			// _ из первого абзаца не должно закрыться _ из второго.
			_md.Render("_текст\n\nещё_текст").Should().Be("<p>_текст</p><p>ещё_текст</p>");
		}

		[Test]
		public void ThreeNewlinesAreStillOneBreak()
		{
			_md.Render("а\n\n\nb").Should().Be("<p>а</p><p>b</p>");
		}

		[Test]
		public void EmptyInputYieldsEmptyOutput()
		{
			_md.Render("").Should().BeEmpty();
		}
	}
}
