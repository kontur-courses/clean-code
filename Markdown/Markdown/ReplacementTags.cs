using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
	public class ReplacementTags
	{
		private readonly List<ITag> pairedTags;
		private readonly string text;

		public ReplacementTags(List<ITag> pairedTags, string text)
		{
			this.pairedTags = pairedTags;
			this.text = text;
		}

		public string ReplaceToHtml()
		{
			var startIndex = 0;
			var htmlBuilder = new StringBuilder();

			for (var i = 0; i < pairedTags.Count; i++)
			{
				var tag = pairedTags[i];
				var nextTag = i + 1 < pairedTags.Count ? pairedTags[i + 1] : null;
				htmlBuilder.Append(text.Substring(startIndex, tag.OpenIndex - startIndex));
				htmlBuilder.Append(tag.HtmlOpen);
				htmlBuilder.Append(GetInnerFormattedText(tag));
				htmlBuilder.Append(tag.HtmlClose);

				htmlBuilder.Append(nextTag != null
					? text.Substring(tag.CloseIndex + tag.Length, nextTag.OpenIndex - tag.CloseIndex - tag.Length)
					: text.Substring(tag.CloseIndex + tag.Length));

				startIndex = nextTag?.OpenIndex ?? tag.CloseIndex + tag.Length;
			}

			return htmlBuilder.ToString();
		}

		private string GetInnerFormattedText(ITag tag)
		{
			var innerText = text.GetBodyInside(tag);
			var mdConverter = new MdConverter();
			var innerPairedTags = mdConverter.GetAllPairedTags(innerText).Where(t => t.Length <= tag.Length).ToList();

			if (innerPairedTags.Count == 0)
				return innerText;

			var replacementTags = new ReplacementTags(innerPairedTags, innerText);
			var htmlText = replacementTags.ReplaceToHtml();
			return htmlText;
		}
	}
}