using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class TokenAnalizerTests
	{

		private TokenDescription[] tokensDescriptionsArray;

		[SetUp]
		public void SetUp()
		{
			tokensDescriptionsArray = new[]
			{
				new TokenDescription("Text", null, null, null),
				new TokenDescription("Italics", "_", "<em>", null),
				new TokenDescription("Strong", "__", "<strong>", null)
			};
		}


		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForDoubleTagging()
		{
			var input = new[]
			{
				new Token("Strong", TagType.Opening),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "kek"),
				new Token("Strong", TagType.Closing),
				new Token("Italics", TagType.Closing)
			};

			var output = new[]
			{
				new Token("Strong", TagType.Opening),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "kek"),
				new Token("Italics", TagType.Closing),
				new Token("Strong", TagType.Closing)
			};

			TokenAnalizer.Initialize("Text", tokensDescriptionsArray);
			TokenAnalizer.Analize(input).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForOrdinaryValidInput()
		{
			var input = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from"),
				new Token("Italics", TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from"),

				new Token("Italics", TagType.Closing)
			};

			TokenAnalizer.Initialize("Text", tokensDescriptionsArray);
			TokenAnalizer.Analize(input).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForValidInputWithExtraClosingTag()
		{
			var input = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from"),
				new Token("Strong", TagType.Closing),
				new Token("Italics", TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from__"),
				new Token("Italics", TagType.Closing)
			};

			TokenAnalizer.Initialize("Text", tokensDescriptionsArray);
			TokenAnalizer.Analize(input).ShouldBeEquivalentTo(output);
		}


		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForValidInputWithExtraOpeningTag()
		{
			var input = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from"),
				new Token("Strong", TagType.Closing),
				new Token("Italics", TagType.Closing)
			};

			var output = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "go __aw_ay"),
				new Token("Strong", TagType.Closing),
				new Token("Text", TagType.Undefined, " from"),
				new Token("Italics", TagType.Closing)
			};

			TokenAnalizer.Initialize("Text", tokensDescriptionsArray);
			TokenAnalizer.Analize(input).ShouldBeEquivalentTo(output);
		}

	}
}
