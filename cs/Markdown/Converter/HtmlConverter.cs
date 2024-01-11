using System.Text;
using Markdown.Tags;

namespace Markdown.Converter
{
    public class HtmlConverter : IHtmlConverter
    {
        private Dictionary<Tag, string> htmlTagsMarkupDict = MarkdownConfig.HtmlTags;

        public string ConvertFromMarkdownToHtml(string markdownText, List<Token> tokens)
        {
            var htmlResultText = new StringBuilder(markdownText);
            var htmlTags = ConvertToHtmlTags(tokens);
            var shift = 0;

            foreach (var tag in htmlTags)
            {
                var mdTaglength = 1;
                if (tag.Tag == Tag.Bold)
                {
                    mdTaglength = 2;
                    shift--;
                }
                if (tag.IsClosing && (tag.Tag == Tag.Header || tag.Tag == Tag.EscapedSymbol)) 
                {
                    mdTaglength = 0;
                    shift++;
                }
                htmlResultText.Remove(tag.Index + shift, mdTaglength);
                htmlResultText.Insert(tag.Index + shift, tag.GetMarkup());
                shift = htmlResultText.Length - markdownText.Length;
            }

            return htmlResultText.ToString();
        }

        private List<HtmlTag> ConvertToHtmlTags(List<Token> tokens)
        {
            var htmlTags = new List<HtmlTag>();

            foreach (var token in tokens)
            {
                htmlTags.Add(new HtmlTag(token.TagType, token.StartIndex, false, htmlTagsMarkupDict[token.TagType]));
                htmlTags.Add(new HtmlTag(token.TagType, token.EndIndex , true, htmlTagsMarkupDict[token.TagType]));
            }

            return htmlTags.OrderBy(tag => tag.Index).ToList();
        }
    }
}
