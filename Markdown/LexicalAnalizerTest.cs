using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class LexicalAnalizerTest
	{
		private TokenDescription[] tokensDescriptionsArray;

		[SetUp]
		public void SetUp()
		{ 
			tokensDescriptionsArray = new[]
			{
				new TokenDescription("Text", null, null, (previous, next)=> TagType.Undefined),

				new TokenDescription("Italics", "_", "<em>", TagTypeDeterminantForItalicsAndBold),

				new TokenDescription("Strong", "__", "<strong>", TagTypeDeterminantForItalicsAndBold)
			};
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForValidInput1()
		{
			var inputTokens = new[]
			{
				new RawToken("Text", "haha "),
				new RawToken("Italics"),
				new RawToken("Text", "go "),
				new RawToken("Strong"),
				new RawToken("Text", "aw"),
				new RawToken("Italics"),
				new RawToken("Text", "ay"),
				new RawToken("Strong"),
				new RawToken("Text", " from"),
				new RawToken("Italics")
			};

			var output = new []
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

			LexicalAnalizer.Initialize("Text", tokensDescriptionsArray);
			LexicalAnalizer.Analize(inputTokens).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForValidInput2()
		{
			var inputTokens = new[]
			{
				new RawToken("Text", "haha "),
				new RawToken("Italics"),
				new RawToken("Text", "go "),
				new RawToken("Strong"),
				new RawToken("Text", "aw"),
				new RawToken("Italics"),
				new RawToken("Text", "ay"),
				new RawToken("Strong"),
				new RawToken("Text", "from"),
				new RawToken("Italics")
			};

			var output = new[]
			{
				new Token("Text", TagType.Undefined, "haha "),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "go "),
				new Token("Strong", TagType.Opening),
				new Token("Text", TagType.Undefined, "aw_ay__from"),
				new Token("Italics", TagType.Closing)
			};

			LexicalAnalizer.Initialize("Text", tokensDescriptionsArray);
			LexicalAnalizer.Analize(inputTokens).ShouldBeEquivalentTo(output);
		}

		[Test]
		public void Analizer_ReturnValidOutput_ForKek()
		{
			var inputTokens = new[]
			{
				new RawToken("Strong"),
				new RawToken("Italics"),
				new RawToken("Text", "kek"),
				new RawToken("Strong"),
				new RawToken("Italics")
			};

			var output = new[]
			{
				new Token("Strong", TagType.Opening),
				new Token("Italics", TagType.Opening),
				new Token("Text", TagType.Undefined, "kek"),
				new Token("Strong", TagType.Closing),
				new Token("Italics", TagType.Closing)
			};

			LexicalAnalizer.Initialize("Text", tokensDescriptionsArray);
			LexicalAnalizer.Analize(inputTokens).ShouldBeEquivalentTo(output);
		}

		private TagType TagTypeDeterminantForItalicsAndBold(char? previousSymbol, char? nextSymbol)
		{
			if (previousSymbol == '\\')
				return TagType.Undefined;


			if (previousSymbol == null || previousSymbol == ' ')
			{
				if (nextSymbol == null)
					return TagType.Undefined;
				if (nextSymbol != ' ')
					return TagType.Opening;
				
			}

			if (nextSymbol == null || nextSymbol == ' ')
			{
				if (previousSymbol == null)
					return TagType.Undefined;
				if (previousSymbol != ' ')
					return TagType.Closing;
			}

			if (!char.IsLetterOrDigit((char)previousSymbol) && char.IsLetterOrDigit((char)nextSymbol))
				return TagType.Opening;

			if (char.IsLetterOrDigit((char) previousSymbol) && !char.IsLetterOrDigit((char) nextSymbol))
				return TagType.Closing;
			return TagType.Undefined;
		}
	}
}
