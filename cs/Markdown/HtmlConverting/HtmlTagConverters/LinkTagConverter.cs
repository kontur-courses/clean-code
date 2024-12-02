using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class LinkTagConverter() : BaseHtmlTagConverter(TagType.LinkText)
{
    public override string ConvertTokensToHtmlText(
        Dictionary<TagType, IHtmlTagConverter> converters,
        IReadOnlyList<Token> tokens,
        int start,
        out int readTokens)
    {
        var linkTextBuilder = new StringBuilder();
        var currentPosition = start;

        while (!IsCloseTagOfNeededTagType(tokens, currentPosition, TagType.LinkText))
        {
            if (TokenUtilities.TryGetTagTypeByOpenTag(tokens[currentPosition], out var tagType))
            {
                var convertedString = ConvertTokensToHtmlTextInTag(converters, tokens, tagType!.Value, ref currentPosition);
                linkTextBuilder.Append(convertedString);
            }
            else
                linkTextBuilder.Append(tokens[currentPosition].Content);
            currentPosition++;
        }

        readTokens = ++currentPosition - start;
        if (currentPosition >= tokens.Count ||
            tokens[currentPosition].Type != TokenType.Tag ||
            !TokenUtilities.TryGetTagTypeByOpenTag(tokens[currentPosition], out var nextTagType) ||
            nextTagType != TagType.LinkValue)
        {
            return linkTextBuilder.ToString();
        }

        var linkValueBuilder = new StringBuilder();
        currentPosition++;
        while (!IsCloseTagOfNeededTagType(tokens, currentPosition, TagType.LinkValue))
            linkValueBuilder.Append(tokens[currentPosition++].Content);

        readTokens = currentPosition - start + 1;
        return HtmlTagsCreator.CreateOpenTag(TagType.LinkText, ("href", linkValueBuilder.ToString())) +
               linkTextBuilder +
               HtmlTagsCreator.CreateCloseTag(TagType.LinkText);
    }

    private static bool IsCloseTagOfNeededTagType(IReadOnlyList<Token> tokens, int position, TagType neededTagType)
    {
        var token = tokens[position];
        return token.Type == TokenType.Tag &&
               TokenUtilities.TryGetTagTypeByCloseTag(token, out var tagType) &&
               tagType == neededTagType;
    }
}