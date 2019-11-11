using System;

namespace Markdown.Features
{
	internal class Link: IToken
	{
		public string OpeningSequence { get; } = "[";
		public string ClosingSequence { get; } = ")";
		public bool IsComplex { get; } = true;

		public string GetOpeningTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new System.NotImplementedException();
		}

		private static bool ContentIsValid(string content)
		{
			throw new NotImplementedException();
		}

		public string GetClosingTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new System.NotImplementedException();
		}
	}
}