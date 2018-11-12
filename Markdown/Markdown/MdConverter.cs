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
			var existingPairedTags = GetPairedTags(stream);

			if (existingPairedTags.Count == 0)
				return stream.Text.RemoveScreenCharacters();

			var textInHtml = GetHtmlCode(existingPairedTags, stream.Text);


			return textInHtml.RemoveScreenCharacters();
		}

		private List<ITag> GetPairedTags(TextStream stream)
		{
			var pairedTags = new List<ITag>();
			while (stream.Position < stream.Text.Length - 2)
			{
				var symbol = stream.Current();
				var twoSymbol = stream.Text.Substring(stream.Position, 2);

				if (IsOpenTag(twoSymbol, stream, 2))
					pairedTags.AddRange(GetPairedTags(twoSymbol, stream));

				if (IsOpenTag(symbol.ToString(), stream, 1))
					pairedTags.AddRange(GetPairedTags(symbol.ToString(), stream));

				else
					stream.MoveNext();
			}

			return pairedTags;
		}

		private List<ITag> GetPairedTags(string symbol, TextStream stream)
		{
			var pairedTags = new List<ITag>();
			var tag = dictionaryTags[symbol];
			tag.OpenIndex = stream.Position;
			tag.CloseIndex = tag.FindCloseIndex(stream.Text);

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

			return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
			                                                     && (char.IsWhiteSpace(prevSymbol) || stream.Position == 0);
		}

		private string GetHtmlCode(List<ITag> pairedTags, string text)
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();

			foreach (var tag in pairedTags)
			{
				var innerText = tag.Body(text);

				htmlBuilder.Append(text.Substring(startIndex, tag.OpenIndex - startIndex));
				htmlBuilder.Append(tag.HtmlOpen);

				var innerPairedTags = GetPairedTags(new TextStream(innerText));
				htmlBuilder.Append(innerPairedTags.Count == 0 ? innerText : GetHtmlCode(innerPairedTags, innerText));

				htmlBuilder.Append(tag.HtmlClose);
				htmlBuilder.Append(text.Substring(tag.CloseIndex + tag.Length));

				startIndex = tag.CloseIndex + tag.Length;
			}

			return htmlBuilder.ToString();
		}
	}
}