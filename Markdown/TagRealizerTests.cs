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
	public class TagRealizerTests
	{
		private TokenDescription[] tokensDescriptionsArray;

		[SetUp]
		public void SetUp()
		{
			tokensDescriptionsArray = new[]
			{
				new TokenDescription("Text", null, null, null),
				new TokenDescription("Italics", "_", "<em>", null),
				new TokenDescription("Bold", "__", "<strong>", null)
			};
		}

		[Test]
		public void TokenAnalizer_ReturnValidOutput_ForDoubleTagging()
		{
			var input = new[]
			{
				new Token("Bold", TagType.Opening),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "kek"),
				new Token("Italics", TagType.Closing),
				new Token("Bold", TagType.Closing)
			};

			var output = "<strong><em>kek</em></strong>";

			TagRealizer.Initialize("Text", tokensDescriptionsArray);
			TagRealizer.RealizeTokens(input).ShouldBeEquivalentTo(output);
		}
	}
}
