using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverter
	{
		private readonly TextStream stream;
		private List<ITag> existedPairedTags;
		readonly Dictionary<string, ITag> dictionaryTags;

		public MdConverter(TextStream stream)
		{
			this.stream = stream;
			existedPairedTags = new List<ITag>();
			dictionaryTags = new Dictionary<string, ITag>
			{
				{"_", new UnderLineTag()},
				{"__", new DoubleUnderLineTag()}
			};
		}

		public string ConvertToHtml()
		{
			existedPairedTags = GetPairedTags();

			if (existedPairedTags.Count == 0)
				return stream.Text();

			var sortedTagIndexes = GetSortedTagIndexes();
			var textInHtml = GetHtmlCode(sortedTagIndexes);


			return textInHtml;
		}

		private List<ITag> GetPairedTags()
		{
			while (stream.Position() < stream.Length())
			{
				var symbol = stream.Current();

				if (IsOpenTag(symbol))
				{
					var tag = GetTag(symbol);
					tag.OpenIndex = stream.Position();
					tag.CloseIndex = tag.FindCloseIndex(stream.Text());

					if (tag.CloseIndex != -1)
						existedPairedTags.Add(tag);
				}

				stream.MoveNext();
			}

			return existedPairedTags;
		}

		private ITag GetTag(char symbol) => 
			IsDoubleUnderLineTag(symbol) ? new DoubleUnderLineTag() : dictionaryTags[symbol.ToString()];

		private bool IsDoubleUnderLineTag(char symbol) =>
			symbol == '_' && stream.Lookahead(1) == '_' && !char.IsWhiteSpace(stream.Lookahead(2))
			&& (char.IsWhiteSpace(stream.Lookahead(-1)) || stream.Position() == 0);

		private string GetHtmlCode(List<int> sortedTagIndexes)
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();
			foreach (var tagIndex in sortedTagIndexes)
			{
				htmlBuilder.Append(stream.Text().Substring(startIndex, tagIndex - startIndex));
				var isOpen = existedPairedTags.Any(t => t.OpenIndex == tagIndex);
				int tagLength;

				if (isOpen)
				{
					var tag = existedPairedTags.Single(t => t.OpenIndex == tagIndex);
					tagLength = tag.Length;
					htmlBuilder.Append(tag.HtmlOpen);
				}
				else
				{
					var tag = existedPairedTags.Single(t => t.CloseIndex == tagIndex);
					tagLength = tag.Length;
					htmlBuilder.Append(tag.HtmlClose);
				}

				startIndex = tagIndex + tagLength;
			}

			return htmlBuilder.ToString();
		}

		private List<int> GetSortedTagIndexes()
		{
			var tagIndexes = existedPairedTags.Select(t => t.OpenIndex).ToList();
			tagIndexes.AddRange(existedPairedTags.Select(t => t.CloseIndex));
			tagIndexes.Sort();
			return tagIndexes;
		}

		private bool IsOpenTag(char symbol)
		{
			var prevSymbol = stream.Lookahead(-1);
			var nextSymbol = stream.Lookahead(1);

			return dictionaryTags.ContainsKey(symbol.ToString()) && !char.IsWhiteSpace(nextSymbol)
			                                                     && (char.IsWhiteSpace(prevSymbol) || stream.Position() == 0);
		}
	}
}
