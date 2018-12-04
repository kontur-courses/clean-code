using System.Collections.Generic;

namespace Markdown.ASTNodes.StyleElement
{
    public interface IStyleElement : IElement
    {
        IEnumerable<Token> InnerTokens { get; set; }

        string GetRawValue(string value);

        string TransformToHtml(string value);
    }
}
