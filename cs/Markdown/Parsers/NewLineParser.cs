using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class NewLineParser : ITokenParser
{
    private readonly TokenCollectionParser mainParser;

    public NewLineParser(TokenCollectionParser mainParser)
    {
        this.mainParser = mainParser;
    }

    public TagNode Parse()
    {
        var token = Tokens.NewLine;
        
        if (mainParser.TryFlushContextsUntil(out var context, TokenContext.IsHeader1))
        {
            var node = mainParser.ToNode(context);

            if (node.Tag.Type == TagType.Text)
            {
                return Tokens.Text(string.Join("", new[] { node.Tag.Value, token.Value })).ToTagNode();
            }

            mainParser.PushToBuffer(token);
            return new TagNode(node.Tag, node.Children);
        }

        return token.ToTextToken().ToTagNode();
    }
}