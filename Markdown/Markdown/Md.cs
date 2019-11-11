namespace Markdown
{
    public class Md
    {
        public static string Render(string mdText)
        {
            var lexemes = Lexer.ExtractLexemes(mdText);
            var line = Parser.ParseLine(lexemes);
            var html = HtmlRenderer.RenderParagraph(line);

            return html;
        }
    }
}