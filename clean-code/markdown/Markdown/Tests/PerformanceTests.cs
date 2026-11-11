using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Text;

namespace Markdown.Tests
{
	[TestFixture]
	public class PerformanceTests
	{
		private const int ThresholdMs = 2000;

		[Test]
		public void RenderLargeInputIsLinear()
		{
			var md = new Md();
			string chunk = "текст _курсив_ и __сильный__ # ещё - пункт ";
			var sb = new StringBuilder();

			while (sb.Length < 100000)
			{
				sb.Append(chunk);
			}
			string input = sb.ToString();

			var sw = Stopwatch.StartNew();
			string output = md.Render(input);
			sw.Stop();

			output.Should().NotBeNullOrEmpty();
			sw.ElapsedMilliseconds.Should().BeLessThan(ThresholdMs,
				$"рендер 100k символов занял {sw.ElapsedMilliseconds} мс — это не похоже на O(n)");
		}

		[Test]
		public void RenderManyUnderscoresIsLinear()
		{
			var md = new Md();
			string input = new string('_', 50000) + "x" + new string('_', 50000);

			var sw = Stopwatch.StartNew();
			md.Render(input);
			sw.Stop();

			sw.ElapsedMilliseconds.Should().BeLessThan(ThresholdMs,
				$"рендер 100k '_' занял {sw.ElapsedMilliseconds} мс — возможен O(n²)");
		}
	}
}
