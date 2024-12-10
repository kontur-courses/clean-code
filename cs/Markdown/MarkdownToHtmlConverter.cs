using System.Text;
using Markdown.AstNodes;

namespace Markdown;

public class MarkdownToHtmlConverter(ILexer lexer, IParser parser)
{
    private ILexer Lexer { get; } = lexer;
    private IParser Parser { get; } = parser;

    public string Convert(string input)
    {
        var tokens = Lexer.Tokenize(input);
        var ast = Parser.Parse(tokens);
        return ConvertAstToHtml(ast);
    }

    private string ConvertAstToHtml(RootMarkdownNode ast)
    {
        var html = new StringBuilder();
        ConvertToHtml(ast, html);
        return html.ToString();
    }

    private void ConvertToHtml(MarkdownNode node, StringBuilder html)
    {
        switch (node)
        {
            case TextMarkdownNode textNode:
                html.Append(textNode.Content);
                break;
            case ItalicMarkdownNode italicNode:
                html.Append("<em>");
                foreach (var child in italicNode.Children)
                    ConvertToHtml(child, html);
                html.Append("</em>");
                break;
            case BoldMarkdownNode boldNode:
                html.Append("<strong>");
                foreach (var child in boldNode.Children)
                    ConvertToHtml(child, html);
                html.Append("</strong>");
                break;
            case HeadingMarkdownNode headingNode:
                html.Append("<h1>");
                foreach (var child in headingNode.Children)
                    ConvertToHtml(child, html);
                html.Append("</h1>");
                break;
            case RootMarkdownNode root:
            {
                foreach (var child in root.Children)
                    ConvertToHtml(child, html);
                break;
            }
        }
    }
}