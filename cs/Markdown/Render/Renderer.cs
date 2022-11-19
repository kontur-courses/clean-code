using System.Text;
using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Render;

public class Renderer : IRenderer
{
    public string Render(TokenTree tree, Dictionary<Tag, Tag> rules)
    {
        if (tree.IsLeaf) return $"{rules[tree.Tag].Opening}{tree.Value}{rules[tree.Tag].Closing}";
        
        var stringBuilder = new StringBuilder();
        foreach (var child in tree.Children)
        {
            stringBuilder.Append(Render(child, rules));
        }

        return stringBuilder.ToString();
    }
}