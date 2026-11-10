using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Renderers;

public class HtmlRenderer : IMarkedTextRenderer
{
    public string Render(MarkedText markedText)
    {
        var resultBuilder = new StringBuilder();

        foreach (var token in markedText.Tokens)
            switch (token.Kind)
            {
                case TokenKind.Text:
                    resultBuilder.Append(token.Text);
                    break;
                case TokenKind.EmphasisStart:
                    resultBuilder.Append("<em>");
                    break;
                case TokenKind.EmphasisEnd:
                    resultBuilder.Append("</em>");
                    break;
                case TokenKind.StrongStart:
                    resultBuilder.Append("<strong>");
                    break;
                case TokenKind.StrongEnd:
                    resultBuilder.Append("</strong>");
                    break;
                case TokenKind.HeadingStart:
                    resultBuilder.Append("<h1>");
                    break;
                case TokenKind.HeadingEnd:
                    resultBuilder.Append("</h1>");
                    break;

                default:
                    throw new ArgumentException($"Unknown token kind: {token.Kind}");
            }

        return resultBuilder.ToString();
    }
}