using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace Markdown
{
	[TestFixture]
	class ParserTests
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
		public void Parser_ShouldReturnValidParseResult_ForOrdinaryValidInput()
		{
			var input = "haha _go __away__from_";
			var output = new[]
			{
				new RawToken("Text", "haha "),
				new RawToken("Italics"),
				new RawToken("Text", "go "),
				new RawToken("Bold"),
				new RawToken("Text", "away"),
				new RawToken("Bold"),
				new RawToken("Text", "from"),
				new RawToken("Italics")
			};

			Parser.Initialize("Text", tokensDescriptionsArray);
			Parser.Parse(input).ShouldBeEquivalentTo(output);
		}


		[Test]
		public void Parser_ReturnValidOutput_ForDoubleTagging()
		{
			var input = "___kek___";
			var output = new[]
			{
				new RawToken("Bold"),
				new RawToken("Italics"),
				new RawToken("Text", "kek"),
				new RawToken("Bold"),
				new RawToken("Italics")
			};

			Parser.Initialize("Text", tokensDescriptionsArray);
			Parser.Parse(input).ShouldBeEquivalentTo(output);
		}

	}
}
