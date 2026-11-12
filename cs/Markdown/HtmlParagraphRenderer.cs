using System.Text;

namespace Markdown
{
    public class HtmlParagraphRenderer
    {
        private const char ParagraphSeparator = '\n';
        private const int EscapeSymbolLength = 1;

        private readonly StringBuilder html = new ();
        private bool hasParagraphs;
        private bool isInsideList;

        public static string Render(IEnumerable<string> paragraphs)
        {
            var renderer = new HtmlParagraphRenderer();
            return renderer.RenderToHtml(paragraphs);
        }

        private string RenderToHtml(IEnumerable<string> paragraphs)
        {
            foreach (var paragraph in paragraphs)
            {
                RenderParagraph(paragraph);
            }

            CloseList();
            return html.ToString();
        }

        private void RenderParagraph(string paragraph)
        {
            if (MarkdownSyntax.TryGetBlockTag(paragraph, out var tagType))
                RenderBlock(tagType, paragraph[MarkdownSyntax.GetMarker(tagType).Length..]);
            else
                AppendOutsideList(HtmlInlineRenderer.Render(RemoveBlockMarkerEscape(paragraph)));
        }

        private void RenderBlock(TagType tagType, string content)
        {
            var blockHtml = HtmlConverter.Wrap(tagType, HtmlInlineRenderer.Render(content));
            if (tagType == TagType.ListItem)
                AppendInsideList(blockHtml);
            else
                AppendOutsideList(blockHtml);
        }

        private void AppendInsideList(string paragraphHtml)
        {
            AppendSeparator();
            OpenList();
            html.Append(paragraphHtml);
        }

        private void AppendOutsideList(string paragraphHtml)
        {
            CloseList();
            AppendSeparator();
            html.Append(paragraphHtml);
        }

        private void AppendSeparator()
        {
            if (hasParagraphs)
                html.Append(ParagraphSeparator);

            hasParagraphs = true;
        }

        private void OpenList()
        {
            if (isInsideList)
                return;

            html.Append(HtmlConverter.Open(TagType.UnorderedList));
            isInsideList = true;
        }

        private void CloseList()
        {
            if (isInsideList)
                html.Append(HtmlConverter.Close(TagType.UnorderedList));

            isInsideList = false;
        }

        private static string RemoveBlockMarkerEscape(string paragraph)
        {
            return IsBlockMarkerEscaped(paragraph) ? paragraph[EscapeSymbolLength..] : paragraph;
        }

        private static bool IsBlockMarkerEscaped(string paragraph)
        {
            var startsWithEscape = paragraph.StartsWith(MarkdownSyntax.EscapeSymbol);
            return startsWithEscape && MarkdownSyntax.TryGetBlockTag(paragraph[EscapeSymbolLength..], out _);
        }
    }
}
