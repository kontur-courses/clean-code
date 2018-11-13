using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverter
	{
		private readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
		{
			{"_", new SingleUnderLineTag()},
			{"__", new DoubleUnderLineTag()}
		};

		public string ConvertToHtml(TextStream stream)
		{
			var existingPairedTags = GetAllPairedTags(stream);

			if (existingPairedTags.Count == 0)
				return stream.Text.RemoveScreenCharacters();

			var textInHtml = GetHtmlCode(existingPairedTags, stream.Text);


			return textInHtml.RemoveScreenCharacters();
		}

		private List<ITag> GetAllPairedTags(TextStream stream)
		{
			var pairedTags = new List<ITag>();

			while (stream.Position < stream.Text.Length - 2)
			{
				var symbol = stream.Current();
				var twoSymbol = stream.Text.Substring(stream.Position, 2);

				if (IsOpenTag(twoSymbol, stream, 2))
					pairedTags.AddRange(GetOnePairOfTags(twoSymbol, stream));

				if (IsOpenTag(symbol.ToString(), stream, 1))
					pairedTags.AddRange(GetOnePairOfTags(symbol.ToString(), stream));

				else
					stream.MoveNext();
			}

			return pairedTags;
		}

		private List<ITag> GetOnePairOfTags(string symbol, TextStream stream)
		{
			var pairedTags = new List<ITag>();
			var tag = dictionaryTags[symbol];
			tag.OpenIndex = stream.Position;
			tag.CloseIndex = tag.FindCloseIndex(stream);

			if (tag.CloseIndex != -1)
			{
				pairedTags.Add(tag);
				stream.MoveTo(pairedTags.Last().CloseIndex + pairedTags.Last().Length);
			}

			return pairedTags;
		}

		private bool IsOpenTag(string symbol, TextStream stream, int tagLength)
		{
			var prevSymbol = stream.Lookahead(-1);
			var nextSymbol = stream.Lookahead(tagLength);

			return dictionaryTags.ContainsKey(symbol) && char.IsLetter(nextSymbol)
			                                          && (char.IsWhiteSpace(prevSymbol) || stream.Position == 0);
		}

		private string GetHtmlCode(List<ITag> pairedTags, string text)
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();

			foreach (var tag in pairedTags)
			{
				htmlBuilder.Append(text.Substring(startIndex, tag.OpenIndex - startIndex));
				htmlBuilder.Append(tag.HtmlOpen);
				htmlBuilder.Append(GetInnerFormattedText(tag, text));
				htmlBuilder.Append(tag.HtmlClose);
				htmlBuilder.Append(text.Substring(tag.CloseIndex + tag.Length));

				startIndex = tag.CloseIndex + tag.Length;
			}

			return htmlBuilder.ToString();
		}

		private string GetInnerFormattedText(ITag tag, string text)
		{
			var innerText = tag.Body(text);
			var innerPairedTags = GetAllPairedTags(new TextStream(innerText));

			return innerPairedTags.Count != 0 && innerPairedTags.Last().Length > tag.Length || innerPairedTags.Count == 0
				? innerText
				: GetHtmlCode(innerPairedTags, innerText);
		}
	}
}