using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class Md_Render_Should
	{
		[TestCase(null, "", TestName = "Null")]
		[TestCase("", "", TestName = "Empty string")]
		[TestCase(" \n", " \n", TestName = "Whitespaces")]
		[TestCase("_i_", "<em>i</em>", TestName = "Italic")]
		[TestCase("p t", "p t", TestName = "Plain text")]
		[TestCase(@"\_i_", "_i_", TestName = "Shielded opening sequence")]
		[TestCase(@"\_i\_", "_i_", TestName = "Shielded opening and closing sequence")]
		[TestCase(@"_\_i_", "<em>_i</em>", TestName = "Shielded opening sequence part")]
		[TestCase("__b__", "<strong>b</strong>", TestName = "Bold")]
		[TestCase("__b__ t", "<strong>b</strong> t", TestName = "Text after closing sequence")]
		[TestCase("t_12_3", "t_12_3", TestName = "Italic underlines inside text with digits")]
		[TestCase("t__12__", "t__12__", TestName = "Bold underlines inside text with digits")]
		[TestCase("__b", "__b", TestName = "Opened token without closing sequence")]
		[TestCase("__b_i_b__", "<strong>b<em>i</em>b</strong>", TestName = "Italic inside bold")]
		[TestCase("_i__b__i_", "<em>i__b__i</em>", TestName = "Bold inside italic")]
		[TestCase("____", "____", TestName = "Bold token with empty content")]
		[TestCase("__ __", "__ __", TestName = "Bold token with whitespace content")]
		[TestCase("_", "_", TestName = "Only open sequence")]
		[TestCase("__b_", "__b_", TestName = "Wrong closing sequence")]
		[TestCase("__ i_", "__ i_", TestName = "Underline after italic opening sequence")]
		[TestCase("_ i_", "_ i_", TestName = "Whitespace after italic opening sequence")]
		[TestCase("__ b__", "__ b__", TestName = "Whitespace after bold opening sequence")]
		[TestCase("_i _", "_i _", TestName = "Whitespace before italic closing sequence")]
		[TestCase("__b __", "__b __", TestName = "Whitespace before bold closing sequence")]
		[TestCase("[t](l)", "<a href='l'>t</a>", TestName = "Link")]
		[TestCase("[t]c(l)", "[t]c(l)", TestName = "Char between link-text and link-reference")]
		[TestCase("[_i_](l)", "<a href='l'><em>i</em></a>", TestName = "Link with italic link-text")]
		[TestCase("a__i_bi__a", "a<strong>i_bi</strong>a", TestName = "Only opened token inside other token")]
		[TestCase("_[t](l)_", "<em><a href='l'>t</a></em>", TestName = "Link inside italic")]
		[TestCase("__[_t_](l)__", "<strong><a href='l'><em>t</em></a></strong>", 
			TestName = "Link with italic text inside bold")]
		public void ReturnCorrectHtmlText(string markdownText, string expectedHtmlText)
		{
			var actualHtmlText = Md.Render(markdownText);
			actualHtmlText.Should().Be(expectedHtmlText);
		}

		[Test]
		[Timeout(15000)]
		public void PerformanceTest()
		{
			var sb = new StringBuilder();
			foreach (var token in GenerateTokens((int)5e6))
				sb.Append(token);

			Md.Render(sb.ToString());
		}

		private static IEnumerable<string> GenerateTokens(int count)
		{
			for (var i = 0; i < count / 15; i++)
			{
				yield return "a a a";
				yield return "_ita_";
				yield return "__b__";
			}
		}
	}
}