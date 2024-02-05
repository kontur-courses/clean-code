using System.Text;
using Markdown.Tags;
using Microsoft.VisualStudio.CodeCoverage;

namespace Markdown.Converter
{
    public class HtmlConverter : IHtmlConverter
    {
        private Dictionary<Tag, string> htmlTagsMarkupDict = MarkdownConfig.HtmlTags;
        private int shift;

        public string ConvertFromMarkdownToHtml(string markdownText, List<Token> tokens)
        {
            var htmlResultText = new StringBuilder(markdownText);
            var htmlTags = ConvertToHtmlTags(tokens);
            shift = 0;

            foreach (var tag in htmlTags)
            {
                ReplaceMarkdownWithHtml(htmlResultText, tag);
                shift = htmlResultText.Length - markdownText.Length;
            }

            return htmlResultText.ToString();
        }

        private void ReplaceMarkdownWithHtml(StringBuilder htmlResultText, HtmlTag tag)
        {
            var mdTagLength = GetMdTagLength(tag);
            htmlResultText.Remove(tag.Index + shift, mdTagLength);
            htmlResultText.Insert(tag.Index + shift, tag.GetMarkup());
        }

        private int GetMdTagLength(HtmlTag tag)
        {
            if (tag.Tag == Tag.Bold)
            {
                shift--;
                return 2;
            }

            if (tag.IsClosing && (tag.Tag == Tag.Header || tag.Tag == Tag.EscapedSymbol))
            {
                shift++;
                return 0;
            }

            return 1;
        }

        private List<HtmlTag> ConvertToHtmlTags(List<Token> tokens)
        {
            var htmlTags = new List<HtmlTag>();

            foreach (var token in tokens)
            {
                AddHtmlTag(htmlTags, token.TagType, token.StartIndex, false, htmlTagsMarkupDict[token.TagType]);
                AddHtmlTag(htmlTags, token.TagType, token.EndIndex , true, htmlTagsMarkupDict[token.TagType]);
            }

            return htmlTags.OrderBy(tag => tag.Index).ToList();
        }

        private void AddHtmlTag(List<HtmlTag> htmlTags, Tag tagType, int index, bool isClosing, string htmlMarkup)
        {
            htmlTags.Add(new HtmlTag(tagType, index, isClosing, htmlMarkup));
        }
    }
}
