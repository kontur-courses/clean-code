using System.Text;
using Markdown.Convert;
using Markdown.Token;

namespace Markdown.Render;

public class TokenRenderer : ITokenRenderer
{
    public string Render(TokenTree tree, ITagConverter converter)
    {
        string opening;
        string closing;
        
        if (tree.Tag == null)
        {
            opening = "";
            closing = "";
        }
        else if (tree.Closed)
        {
            var tag = tree.Valid ? converter.Convert(tree.Tag) : tree.Tag;
            opening = tag.Opening;
            closing = tag.Closing;
        }
        else
        {
            opening = tree.Tag.Opening;
            closing = "";
        }

        if (tree.IsLeaf) return $"{opening}{tree.Value}{closing}";
        
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(opening);
        foreach (var child in tree.Children)
        {
            stringBuilder.Append(Render(child, converter));
        }
        stringBuilder.Append(closing);

        return stringBuilder.ToString();
    }
}