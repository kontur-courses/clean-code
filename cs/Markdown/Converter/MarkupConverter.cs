using System.Text;
using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Converter;

public class MarkupConverter : IConverter
{
    private ISyntax syntax;

    public MarkupConverter(ISyntax syntax)
    {
        this.syntax = syntax;
    }

    public string ConvertTags(IList<IToken> tokens, string source)
    {
        var result = new StringBuilder();
        var prevIndex = 0;

        foreach (var token in tokens)
        {
            result.Append(source.AsSpan(prevIndex, token.Position - prevIndex));
            var tag = syntax.ConvertTag(token);
            if (!token.IsClosed)
            {
                if (token.IsParametrized)
                    tag.RenderParameters(token.Parameters, syntax.GetSupportedTagParameters(token.Separator));
                result.Append(tag.OpeningSeparator);
            }
            else
            {
                result.Append(tag.CloseSeparator);
            }

            prevIndex = token.Position + token.Length + token.Shift;
        }

        if (prevIndex <= source.Length - 1)
            result.Append(source.AsSpan(prevIndex, source.Length - prevIndex));

        return result.ToString();
    }
}