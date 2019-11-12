using System;

namespace Markdown.Features
{
	internal class Link: IToken
	{
		public string OpeningSequence { get; } = "[";
		public string ClosingSequence { get; } = ")";
		
		public KeySequenceType RecognizeKeySequence(Context context, string sourceText)
		{
			throw new NotImplementedException();
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new NotImplementedException();
		}

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new NotImplementedException();
		}

		private static bool ContentIsValid(string content)
		{
			throw new NotImplementedException();
		}
	}
}