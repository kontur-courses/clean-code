using Markdown.Domain.Tags;
using Markdown.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Extensions;

namespace Markdown.Builder
{
    public class HtmlBuilder : IHtmlBuilder
    {
        private readonly Dictionary<Tag, string> _htmlTagsMarkupDict;

        private int shift;

        public HtmlBuilder(Dictionary<Tag, string> htmlTagsMarkupDict)
        {
            _htmlTagsMarkupDict = htmlTagsMarkupDict;
        }

        public string BuildHtmlFromMarkdown(string markdownText, List<Token> tokens)
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
                var htmlMarkup = _htmlTagsMarkupDict[token.TagType];
                var tag = token.TagType;

                htmlTags.Add(new HtmlTag(tag, token.StartIndex, false, htmlMarkup));
                htmlTags.Add(new HtmlTag(tag, token.EndIndex, true, htmlMarkup));
            }

            return htmlTags.OrderBy(tag => tag.Index).ToList();
        }
    }
}
