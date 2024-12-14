using System.Text;
using Markdown.HtmlTags;
using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown.Converters;

public class HtmlConverter : IHtmlConverter
{
    public string Convert(List<MarkdownParagraph> markdownParagraphs)
    {
        var htmlParagraphs = new List<string>();

        for (var i = 0; i < markdownParagraphs.Count; i++)
        {
            var paragraph = markdownParagraphs[i];
            if (paragraph.Tags.Count == 0)
            {
                htmlParagraphs.Add(CollectStringOfOnlyCommonTokens(paragraph.Tokens, 0, paragraph.Tokens.Count));
                continue;
            }
            
            if (IsFirstParagraphOfMarkedList(markdownParagraphs, i))
                htmlParagraphs.Add(HtmlTagContentConstants.OpeningBoardMarkedList);
            
            htmlParagraphs.Add(CollectStringOfTokensWithTags(paragraph.Tokens, paragraph.Tags));
            
            if (IsLastParagraphOfMarkedList(markdownParagraphs, i))
                htmlParagraphs.Add(HtmlTagContentConstants.ClosingBoardMarkedList);
        }

        return string.Join(Environment.NewLine, htmlParagraphs);
    }

    private static string CollectStringOfTokensWithTags(
        List<IToken> tokens, 
        List<MarkdownTag> tags)
    {
        var htmlString = new StringBuilder();
        var tagsSortingByPosition = tags
            .OrderBy(tag => tag.Token.PositionInTokens)
            .ToArray();
        var firstTag = tagsSortingByPosition.First();
        var previousTag = firstTag;

        if (firstTag.Token.PositionInTokens > 0)
            htmlString.Append(CollectStringOfOnlyCommonTokens(tokens, endIndex: firstTag.Token.PositionInTokens));

        htmlString.Append(GetHtmlTag(firstTag) ?? "");
        
        for (var i = 1; i < tagsSortingByPosition.Length; i++)
        {
            var currentTag = tagsSortingByPosition[i];
            var commonTokensStartIndex = CalculateCommonTokensStartIndex(previousTag);
            var commonTokensOnString =
                CollectStringOfOnlyCommonTokens(tokens, commonTokensStartIndex, currentTag.Token.PositionInTokens);
            
            htmlString.Append(commonTokensOnString).Append(GetHtmlTag(currentTag) ?? "");
            previousTag = currentTag;
        }

        if (previousTag.Token.PositionInTokens < tokens.Count)
        {
            var commonTokensStartIndex = CalculateCommonTokensStartIndex(previousTag);
            htmlString.Append(CollectStringOfOnlyCommonTokens(tokens, commonTokensStartIndex));
        }
        if (firstTag is { TagType: MarkdownTagType.Heading or MarkdownTagType.MarkedList })
            htmlString.Append(GetHtmlTag(firstTag, true) ?? "");
        
        return htmlString.ToString();
    }
    
    private static string CollectStringOfOnlyCommonTokens(
        List<IToken> tokensInParagraph, 
        int? startIndex = null, 
        int? endIndex = null)
    {
        startIndex ??= 0;
        endIndex ??= tokensInParagraph.Count;
        
        var tokensOnString = new StringBuilder();

        for (var i = startIndex.Value; i < endIndex.Value; i++)
        {
            tokensOnString.Append(tokensInParagraph[i].Content);
        }

        return tokensOnString.ToString();
    }

    private static int CalculateCommonTokensStartIndex(MarkdownTag tagBeforeCommonTokens)
    {
        if (tagBeforeCommonTokens.TagType is MarkdownTagType.Heading or MarkdownTagType.MarkedList)
            return tagBeforeCommonTokens.Token.PositionInTokens + 2;
        
        return tagBeforeCommonTokens.Token.PositionInTokens + 1;
    }

    private static bool IsFirstParagraphOfMarkedList(List<MarkdownParagraph> tagParagraphs, int indexCurrentParagraph)
    {
        var currentTagParagraph = tagParagraphs[indexCurrentParagraph].Tags;

        if (currentTagParagraph.Count == 0 || currentTagParagraph[0].TagType != MarkdownTagType.MarkedList)
            return false;
        
        if (indexCurrentParagraph == 0)
            return true;

        var previousTagParagraph = tagParagraphs[indexCurrentParagraph - 1].Tags;

        if (previousTagParagraph.Count == 0)
            return true;

        return previousTagParagraph[0].TagType != MarkdownTagType.MarkedList;
    }

    private static bool IsLastParagraphOfMarkedList(List<MarkdownParagraph> tagParagraphs, int indexCurrentParagraph)
    {
        var currentTagParagraph = tagParagraphs[indexCurrentParagraph].Tags;

        if (currentTagParagraph.Count == 0 || currentTagParagraph[0].TagType != MarkdownTagType.MarkedList)
            return false;
        
        if (indexCurrentParagraph == tagParagraphs.Count - 1)
            return true;

        var nextTagParagraph = tagParagraphs[indexCurrentParagraph + 1].Tags;

        if (nextTagParagraph.Count == 0)
            return true;

        return nextTagParagraph[0].TagType != MarkdownTagType.MarkedList;
    }
        
    private static string? GetHtmlTag(MarkdownTag markdownTag, bool isNeedClosingTagForSingle = false)
    {
        return markdownTag.TagType switch
        {
            MarkdownTagType.Heading when isNeedClosingTagForSingle => HtmlTagContentConstants.HeadingClosing,
            MarkdownTagType.Heading => HtmlTagContentConstants.HeadingOpening,
            MarkdownTagType.Bold when markdownTag.IsClosedTag => HtmlTagContentConstants.BoldClosing,
            MarkdownTagType.Bold => HtmlTagContentConstants.BoldOpening,
            MarkdownTagType.Italics when markdownTag.IsClosedTag => HtmlTagContentConstants.ItalicsClosing,
            MarkdownTagType.Italics => HtmlTagContentConstants.ItalicsOpening,
            MarkdownTagType.MarkedList when isNeedClosingTagForSingle => HtmlTagContentConstants.MarkedListClosing,
            MarkdownTagType.MarkedList => HtmlTagContentConstants.MarkedListOpening,
            _ => null
        };
    }
}