using System.Text;
using Markdown.Markdown.Attributes;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public class UnionImageHandler : ImageHandlerBase
{
    private readonly AttributeType[] attributeTypes = [AttributeType.Alt, AttributeType.Src];

    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var handledAttributes = new HashSet<IAttribute>();
        var startIndex = -1;
        var endIndex = -1;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (!IsImageTag(token)) continue;
            startIndex = i;
            handledAttributes = GetFilledAttributesSet(tokens, ref i);
            endIndex = i;
        }

        return handledAttributes.Count != attributeTypes.Length
            ? tokens
            : HandleTokensWithUnionImageTag(tokens,
                MarkdownTokenCreator.CreateTagWithAttributes(tokens[startIndex], handledAttributes),
                startIndex,
                endIndex);
    }

    private HashSet<IAttribute> GetFilledAttributesSet(IReadOnlyList<IToken> tokens, ref int i)
    {
        var handledAttributes = new HashSet<IAttribute>();
        foreach (var attrType in attributeTypes)
        {
            var content = GetAttributeContent(tokens, ref i);
            handledAttributes.Add(AttributeCreator.CreateImageAttribute(attrType, content));
        }

        return handledAttributes;
    }

    private static string GetAttributeContent(IReadOnlyList<IToken> tokens, ref int i)
    {
        var attributeContent = new StringBuilder();
        var lastToken = tokens[i];
        while (i < tokens.Count && tokens[++i].TagPair != lastToken)
            attributeContent.Append(tokens[i].Content);
        return attributeContent.ToString();
    }

    private static List<IToken> HandleTokensWithUnionImageTag(IReadOnlyList<IToken> tokens, IToken unionImageTag, int startIndex,
        int endIndex)
    {
        var handledTokens = new List<IToken>();
        for (var i = 0; i < startIndex; i++)
            handledTokens.Add(tokens[i]);
        handledTokens.Add(unionImageTag);
        for (var i = endIndex + 1; i < tokens.Count; i++)
            handledTokens.Add(tokens[i]);
        return handledTokens;
    }
}