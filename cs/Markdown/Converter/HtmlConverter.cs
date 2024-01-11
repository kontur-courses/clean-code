using System.Text;
using Markdown.Tags;

namespace Markdown.Converter
{
    public class HtmlConverter : IHtmlConverter
    {
        private Dictionary<TagType, string> tagsMarkup = MarkdownConfig.HtmlTags;

        public string ConvertFromMarkdownToHtml(string markdownText, List<Token> tokens)
        {
            var htmlResultText = new StringBuilder(markdownText);
            var htmlTags = ConvertToHtmlTags(tokens);
            var shift = 0;

            foreach (var tag in htmlTags)
            {
                var mdTaglength = 1;
                if (tag.Type == TagType.Bold)
                {
                    mdTaglength = 2;
                    shift--;
                }
                if (tag.IsClosing) 
                {
                    if (tag.Type == TagType.Header)
                    {
                        mdTaglength = 0;
                        shift++;
                    }
                    else if (tag.Type == TagType.EscapedSymbol)
                    {
                        mdTaglength = 0;
                        shift++;
                    }
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
                htmlTags.Add(new HtmlTag(token.TagType, token.StartIndex, false, tagsMarkup[token.TagType]));
                htmlTags.Add(new HtmlTag(token.TagType, token.EndIndex , true, tagsMarkup[token.TagType]));
            }
            return htmlTags.OrderBy(tag => tag.Index).ToList();
        }
    }
}
