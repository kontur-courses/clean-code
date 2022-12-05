using System.Text;
using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class TokenCollectionParser : ITokenCollectionParser
{
    public IEnumerable<TagNode> Parse(IEnumerable<Token> tokens)
    {
        if (tokens == null)
        {
            throw new ArgumentNullException(nameof(tokens));
        }

        var enumerator = tokens.GetEnumerator();
        var parser = new InnerParser(enumerator);

        return CombineTextTagNodes(parser.Parse());
    }
    

    private IEnumerable<TagNode> CombineTextTagNodes(IEnumerable<TagNode> nodes)
    {
        var sb = new StringBuilder();
        foreach (var node in nodes)
        {
            if (node.Tag.Type == TagType.Text)
            {
                sb.Append(node.Tag.Value);
            }
            else
            {
                if (sb.Length > 0)
                {
                    yield return Tags.Text(sb.ToString()).ToTagNode();
                    sb.Clear();
                }

                yield return new TagNode(node.Tag, CombineTextTagNodes(node.Children).ToArray());
            }
        }

        if (sb.Length > 0)
        {
            yield return Tags.Text(sb.ToString()).ToTagNode();
        }
    }
}