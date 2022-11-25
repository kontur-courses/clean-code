using System.Text;
using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Render;

public class TokenRenderer : ITokenRenderer
{
    public string Render(TokenTree tree, Dictionary<Tag, Tag> rules)
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
            var tag = tree.Valid ? rules[tree.Tag] : tree.Tag;
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
            stringBuilder.Append(Render(child, rules));
        }
        stringBuilder.Append(closing);

        return stringBuilder.ToString();
    }
}