using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
	public class MdConverterHelper
	{
		private readonly List<ITag> pairedTags;
		private readonly string text;

		public MdConverterHelper(List<ITag> pairedTags, string text)
		{
			this.pairedTags = pairedTags;
			this.text = text;
		}

		public string GetHtmlCode()
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();

			foreach (var tag in pairedTags)
			{
				htmlBuilder.Append(text.Substring(startIndex, tag.OpenIndex - startIndex));
				htmlBuilder.Append(tag.HtmlOpen);
				htmlBuilder.Append(GetInnerFormattedText(tag));
				htmlBuilder.Append(tag.HtmlClose);
				htmlBuilder.Append(text.Substring(tag.CloseIndex + tag.Length));

				startIndex = tag.CloseIndex + tag.Length;
			}

			return htmlBuilder.ToString();
		}

		private string GetInnerFormattedText(ITag tag)
		{
			


			var innerText = text.GetBodyInside(tag);
			var mdConverter = new MdConverter();
			var innerPairedTags = mdConverter.GetAllPairedTags(innerText);
			var helperForMdConverter = new MdConverterHelper(innerPairedTags, innerText);
			var htmlText = helperForMdConverter.GetHtmlCode();
			var isNeedToConvert = (innerPairedTags.Count == 0 || innerPairedTags.Last().Length <= tag.Length) &&
			                      innerPairedTags.Count != 0;

			return isNeedToConvert
			? htmlText
			: innerText;
		}
	}
}