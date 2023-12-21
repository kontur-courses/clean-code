using Markdown.Extensions;
using Markdown.Tags.TagsContainers;
using Markdown.Tags.TagsContainers.ComponentTags;
using Markdown.Tags.TagsContainers.Rules.MarkdownRules;
using Markdown.Tags.TextTag;
using Markdown.Tokens;

namespace Markdown.Parsers.TagsParsers.Markdown;

public class MarkdownParser : ITextParser<Tag>
{
    private static readonly int _tagMaxLength = TagsContainer.GetMarkdownTags().Keys.Max(tag => tag.Length);
    private static readonly Dictionary<string, ITag> _tags = TagsContainer.GetMarkdownTags();
    private readonly HashSet<string> _escapeCharacters;

    public MarkdownParser(HashSet<string> escapeCharacters)
    {
        _escapeCharacters = escapeCharacters;
    }

    public List<IToken<Tag>> ParseText(string text)
    {
        var paragraphs = text.Split("\n");
        var tokens = new List<IToken<Tag>>();
        foreach (var paragraph in paragraphs)
        {
            var paragraphTokens = ParseParagraph(paragraph);
            var paragraphSortedTokens = SortTokens(paragraphTokens, paragraph.Length);
            var openBlockTag = GetOpeningBlockTag(tokens, paragraphSortedTokens);
            var closingBlockTag = GetClosingBlockTag(tokens, paragraphSortedTokens);
            if (openBlockTag != null) tokens.Add(openBlockTag);
            tokens.AddRange(paragraphSortedTokens);
            if (closingBlockTag != null) tokens.Add(closingBlockTag);
        }

        var lastClosingBlockTag = GetClosingBlockTag(tokens, new List<TagToken>());
        if (lastClosingBlockTag != null) tokens.Add(lastClosingBlockTag);

        return tokens;
    }

    private static TagToken GetOpeningBlockTag(List<IToken<Tag>> previousParagraphsTokens,
        List<TagToken> newParagraphTokens)
    {
        var isNextTokenNotComponentTag = newParagraphTokens.Count == 0
                                         || !IsTag(newParagraphTokens[0].ToString())
                                         || !_tags[newParagraphTokens[0].ToString()].IsTagComponent;
        if (isNextTokenNotComponentTag) return null;

        var nextTag = _tags[newParagraphTokens[0].ToString()];
        var blockTag = ((IComponentTag)nextTag).BlockTag;
        ITag previousTag = null;
        if (previousParagraphsTokens.Count != 0 && IsTag(previousParagraphsTokens.Last().ToString()))
            previousTag = _tags[previousParagraphsTokens.Last().ToString()];
        if (!((IMarkdownBlockTagRules)blockTag.MarkdownRules).IsBlockOpening(previousTag, nextTag))
            return null;

        return new TagToken(0, 0, new Tag(blockTag.HtmlOpenTag, TagStatus.Ignored));
    }

    private static TagToken GetClosingBlockTag(List<IToken<Tag>> previousParagraphsTokens,
        List<TagToken> newParagraphTokens)
    {
        var isPreviousTokenNotComponentTag = previousParagraphsTokens.Count == 0
                                             || !IsTag(previousParagraphsTokens.Last().ToString())
                                             || !_tags[previousParagraphsTokens.Last().ToString()].IsTagComponent;
        if (isPreviousTokenNotComponentTag) return null;

        var lastTag = _tags[previousParagraphsTokens.Last().ToString()];
        var blockTag = ((IComponentTag)lastTag).BlockTag;
        ITag nextTag = null;
        if (newParagraphTokens.Count != 0 && IsTag(newParagraphTokens[0].ToString()))
            nextTag = _tags[newParagraphTokens[0].ToString()];
        if (!((IMarkdownBlockTagRules)blockTag.MarkdownRules).IsBlockClosing(lastTag, nextTag))
            return null;

        return new TagToken(0, 0, new Tag(blockTag.HtmlClosingTag, TagStatus.Ignored));
    }

    private Dictionary<int, TagToken> ParseParagraph(string paragraph)
    {
        var previousTags = new Stack<TagToken>();
        var parsedTokens = new Dictionary<int, TagToken>();
        var textStartIndex = 0;
        for (var currentIndexInParagraph = 0; currentIndexInParagraph < paragraph.Length; currentIndexInParagraph++)
        for (var tagLength = _tagMaxLength; tagLength > 0; tagLength--)
        {
            if (tagLength + currentIndexInParagraph > paragraph.Length) continue;

            var currentText = paragraph.Substring(currentIndexInParagraph, tagLength);
            if (IsTag(currentText) || IsEscapeCharacter(currentText))
            {
                var textTokenBeforeTag = new TagToken(textStartIndex, currentIndexInParagraph - 1,
                    new Tag(paragraph[textStartIndex..currentIndexInParagraph], TagStatus.Ignored));
                AddTextToken(textTokenBeforeTag, parsedTokens);
                var currentTag = GetTag(currentIndexInParagraph, tagLength, paragraph);
                AddTag(currentTag, previousTags, parsedTokens);
                textStartIndex = tagLength + currentIndexInParagraph;
                currentIndexInParagraph = currentIndexInParagraph + tagLength - 1;
                break;
            }
        }

        var lastTextToken = new TagToken(textStartIndex, paragraph.Length - 1,
            new Tag(paragraph[textStartIndex..paragraph.Length], TagStatus.Ignored));
        AddTextToken(lastTextToken, parsedTokens);
        AddNotPairedTags(previousTags, parsedTokens);

        return parsedTokens;
    }

    private static bool IsTag(string tag)
    {
        return _tags.ContainsKey(tag);
    }

    private static void AddTextToken(TagToken textToken, Dictionary<int, TagToken> parsedTokens)
    {
        if (string.IsNullOrEmpty(textToken.ToString()))
            return;

        parsedTokens.Add(textToken.StartIndex, textToken);
    }

    private static TagToken GetTag(int startIndex, int tagLength, string text)
    {
        var endIndex = startIndex + tagLength - 1;
        var tagValue = text.Substring(startIndex, tagLength);
        var tagStatus = GetTagStatus(tagValue, startIndex, endIndex, text);
        var tag = new Tag(tagValue, tagStatus);
        var token = new TagToken(startIndex, endIndex, tag);

        return token;
    }

    private static TagStatus GetTagStatus(string tag, int startIndex, int endIndex, string text)
    {
        var tagStatus = TagStatus.Undefined;
        var isPreviousIndexInParagraphRange = startIndex - 1 >= 0;
        var isNextIndexInParagraphRange = endIndex + 2 <= text.Length;
        var nextSymbol = isNextIndexInParagraphRange ? text[endIndex + 1] : CharExtension.EmptyChar;
        var previousSymbol = isPreviousIndexInParagraphRange ? text[startIndex - 1] : CharExtension.EmptyChar;
        var tagRules = IsTag(tag) ? _tags[tag].MarkdownRules : null;

        if (!IsTag(tag))
            return TagStatus.Ignored;
        if (tagRules.IsTagOpen(previousSymbol, nextSymbol))
            tagStatus = TagStatus.OpenTag;
        else if (tagRules.IsTagClosing(previousSymbol, nextSymbol))
            tagStatus = TagStatus.ClosingTag;
        else if (tagRules.IsTagIgnoredBySymbol(previousSymbol, tagStatus) ||
                 tagRules.IsTagIgnoredBySymbol(nextSymbol, tagStatus))
            tagStatus = TagStatus.Ignored;

        return tagStatus;
    }

    private static bool IsTagsPared(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
    {
        var isTagsNotIgnored = firstTag.Value.TagStatus != TagStatus.Ignored &&
                               secondTag.Value.TagStatus != TagStatus.Ignored;
        if (isTagsNotIgnored &&
            _tags[firstTag.ToString()].MarkdownRules.IsTagsPaired(firstTag, secondTag, parsedTokens))
        {
            firstTag.Value.TagStatus = TagStatus.OpenTag;
            secondTag.Value.TagStatus = TagStatus.ClosingTag;
            return true;
        }

        return false;
    }

    private bool IsEscapeCharacter(string character)
    {
        return _escapeCharacters.Contains(character);
    }

    private void AddTag(TagToken currentTag, Stack<TagToken> previousTags, Dictionary<int, TagToken> parsedTokens)
    {
        if (!previousTags.TryPeek(out var previousTag))
        {
            previousTags.Push(currentTag);
        }
        else if (IsEscapeCharacter(previousTag.ToString()))
        {
            AddEscapeCharacter(currentTag, previousTags, parsedTokens);
        }
        else if (currentTag.Value.TagStatus == TagStatus.Ignored)
        {
            parsedTokens.Add(currentTag.StartIndex, currentTag);
        }
        else if (IsTagsPared(previousTag, currentTag, parsedTokens))
        {
            UpdateTagsBeforeAdding(previousTag, currentTag);
            previousTags.Pop();
            parsedTokens.Add(previousTag.StartIndex, previousTag);
            parsedTokens.Add(currentTag.StartIndex, currentTag);
            UpdateNestedTags(currentTag, previousTag, parsedTokens);
        }
        else
        {
            previousTags.Push(currentTag);
        }
    }

    private void AddEscapeCharacter(TagToken currentTag, Stack<TagToken> previousTags,
        Dictionary<int, TagToken> parsedTokens)
    {
        var previousTag = previousTags.Pop();
        if (previousTag.EndIndex + 1 == currentTag.StartIndex)
        {
            currentTag.Value.TagStatus = TagStatus.Ignored;
            parsedTokens.Add(previousTag.StartIndex, currentTag);
        }
        else
        {
            if (IsEscapeCharacter(currentTag.ToString()))
                previousTags.Push(currentTag);
            parsedTokens.Add(previousTag.StartIndex, previousTag);
        }
    }

    private static void UpdateTagsBeforeAdding(TagToken firstTag, TagToken secondTag)
    {
        if (_tags[firstTag.ToString()].MarkdownRules.IsTagsIgnored(firstTag, secondTag))
        {
            firstTag.Value.TagStatus = TagStatus.Ignored;
            secondTag.Value.TagStatus = TagStatus.Ignored;
        }
    }

    private static void UpdateNestedTags(TagToken currentTag, TagToken previousTag,
        Dictionary<int, TagToken> parsedTokens)
    {
        var nestedTagsToIgnore = _tags[currentTag.ToString()].AllowedNestedTags;
        var nextTag = parsedTokens[previousTag.EndIndex + 1];
        while (nextTag != currentTag)
        {
            var nextTagValue = nextTag.ToString();
            if (IsTag(nextTagValue) && !nestedTagsToIgnore.Contains(_tags[nextTagValue].Type))
                nextTag.Value.TagStatus = TagStatus.Ignored;
            nextTag = parsedTokens[nextTag.EndIndex + 1];
        }
    }

    private static void AddNotPairedTags(Stack<TagToken> tags, Dictionary<int, TagToken> parsedTokens)
    {
        while (tags.Count > 0)
        {
            var tag = tags.Pop();
            if (!IsTag(tag.ToString()) || !_tags[tag.ToString()].IsMarkdownTagSingle)
                tag.Value.TagStatus = TagStatus.Ignored;

            parsedTokens.Add(tag.StartIndex, tag);
        }
    }

    private static List<TagToken> SortTokens(Dictionary<int, TagToken> parsedTokens, int paragraphLength)
    {
        var sortedTokens = new List<TagToken>(parsedTokens.Count);
        var closingTokens = new Dictionary<int, TagToken>();
        var startIndex = 0;

        while (true)
        {
            if (closingTokens.ContainsKey(startIndex))
                sortedTokens.Add(closingTokens[startIndex]);

            if (!parsedTokens.ContainsKey(startIndex))
                break;
            var token = parsedTokens[startIndex];
            sortedTokens.Add(token);
            startIndex = token.EndIndex + 1;

            var closingTag = GetClosingTagForSingleTag(token, paragraphLength);
            if (closingTag != null)
                closingTokens.Add(closingTag.StartIndex, closingTag);
        }

        return sortedTokens;
    }

    private static TagToken GetClosingTagForSingleTag(TagToken token, int paragraphLength)
    {
        var tokenText = token.ToString();
        if (!IsTag(tokenText))
            return null;

        var tag = _tags[tokenText];
        if (!tag.IsMarkdownTagSingle || token.Value.TagStatus != TagStatus.OpenTag)
            return null;
        return ((IMarkdownSingleTagRules)tag.MarkdownRules).GetClosingTag(tag, paragraphLength);
    }
}