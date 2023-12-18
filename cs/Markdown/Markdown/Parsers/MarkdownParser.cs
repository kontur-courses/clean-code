using System.Collections.Immutable;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class MarkdownParser : ITextParser<Tag>
{
    private readonly HashSet<string> _escapeCharacters;
    private readonly int _tagMaxLength;
    private readonly HashSet<string> _tags;
    private readonly ImmutableDictionary<TagDefinition, string> _tagsDefenitions;

    public MarkdownParser(HashSet<string> escapeCharacters, ImmutableDictionary<TagDefinition, string> tagsDefenitions)
    {
        _escapeCharacters = escapeCharacters;
        _tagsDefenitions = tagsDefenitions;
        _tags = tagsDefenitions.Values.ToHashSet();
        _tagMaxLength = _tags.Max(tag => tag.Length);
    }

    public List<IToken<Tag>> ParseText(string text)
    {
        var paragraphs = text.Split("\n");
        var tokens = new List<IToken<Tag>>();
        var startIndex = 0;
        foreach (var paragraph in paragraphs)
        {
            var paragraphTokens = ParseParagraph(startIndex, paragraph);
            tokens.AddRange(paragraphTokens);
            startIndex = tokens.Last().EndIndex + 1;
        }

        return tokens;
    }

    private List<TagToken> ParseParagraph(int startIndex, string paragraph)
    {
        var headerTokens = new Dictionary<int, TagToken>();
        var currentIndexInParagraph = 0;
        if (IsHeader(paragraph))
        {
            AddHeaderTokens(headerTokens, startIndex, paragraph.Length);
            currentIndexInParagraph += _tagsDefenitions[TagDefinition.Header].Length;
        }

        var sequenceTokens = ParseSequence(startIndex, currentIndexInParagraph, paragraph);
        foreach (var parsedToken in headerTokens)
            sequenceTokens.Add(parsedToken.Key, parsedToken.Value);

        return SortTokens(sequenceTokens, startIndex);
    }

    private bool IsHeader(string paragraph) => 
        paragraph.StartsWith(_tagsDefenitions[TagDefinition.Header]);

    private void AddHeaderTokens(Dictionary<int, TagToken> parsedTokens, int startIndex, int textLength)
    {
        var tag = _tagsDefenitions[TagDefinition.Header];
        var openHeaderToken = new TagToken(startIndex, startIndex + tag.Length - 1, new Tag(tag, TagType.OpenTag));
        var closingHeaderToken = new TagToken(
            startIndex + textLength,
            startIndex + textLength + tag.Length - 1,
            new Tag(tag, TagType.ClosingTag));

        parsedTokens.Add(openHeaderToken.StartIndex, openHeaderToken);
        parsedTokens.Add(closingHeaderToken.StartIndex, closingHeaderToken);
    }

    private Dictionary<int, TagToken> ParseSequence(int startIndexInText, int currentIndexInParagraph, string sequence)
    {
        var previousTags = new Stack<TagToken>();
        var parsedTokens = new Dictionary<int, TagToken>();
        var textStartIndex = startIndexInText + currentIndexInParagraph;
        for (; currentIndexInParagraph < sequence.Length; currentIndexInParagraph++)
        for (var tagLength = _tagMaxLength; tagLength > 0; tagLength--)
        {
            if (tagLength + currentIndexInParagraph > sequence.Length)
                continue;
            var currentText = sequence.Substring(currentIndexInParagraph, tagLength);
            if (IsTag(currentText) || IsEscapeCharacter(currentText))
            {
                var textToken = GetTextToken(textStartIndex, startIndexInText, currentIndexInParagraph, sequence);
                if (!string.IsNullOrEmpty(textToken.ToString()))
                    parsedTokens.Add(textToken.StartIndex, textToken);
                var currentTag = GetTag(currentIndexInParagraph, tagLength, sequence, startIndexInText);
                AddTag(currentTag, previousTags, parsedTokens);
                textStartIndex = startIndexInText + tagLength + currentIndexInParagraph;
                currentIndexInParagraph = currentIndexInParagraph + tagLength - 1;
                break;
            }
        }

        var lastTextToken = GetTextToken(textStartIndex, startIndexInText, currentIndexInParagraph, sequence);
        if (!string.IsNullOrEmpty(lastTextToken.ToString()))
            parsedTokens.Add(lastTextToken.StartIndex, lastTextToken);
        AddNotPairedTags(previousTags, parsedTokens);

        return parsedTokens;
    }

    private bool IsTag(string tag) =>
        _tags.Contains(tag);

    private static TagToken GetTextToken(int textStartIndex, int paragraphStartIndex, int currentIndexInParagrapgh,
        string text)
    {
        var tokenText = text.Substring(textStartIndex - paragraphStartIndex,
            currentIndexInParagrapgh + paragraphStartIndex - textStartIndex);
        var token = new TagToken(textStartIndex, currentIndexInParagrapgh + paragraphStartIndex - 1,
            new Tag(tokenText, TagType.Ignored));

        return token;
    }

    private static TagToken GetTag(int startIndex, int tagLength, string text, int paragraphStartIndex)
    {
        var endIndex = startIndex + tagLength - 1;
        var tagType = GetTagType(startIndex, endIndex, text);
        var tag = new Tag(text.Substring(startIndex, tagLength), tagType);
        var token = new TagToken(startIndex + paragraphStartIndex, endIndex + paragraphStartIndex, tag);

        return token;
    }

    private static TagType GetTagType(int startIndex, int endIndex, string text)
    {
        var tagType = TagType.Undefined;
        var isPreviousIndexInParagraphRange = startIndex - 1 >= 0;
        var isNextIndexInParagraphRange = endIndex + 2 <= text.Length;

        if ((isPreviousIndexInParagraphRange && char.IsDigit(text[startIndex - 1]))
            || (isNextIndexInParagraphRange && char.IsDigit(text[endIndex + 1])))
            tagType = TagType.Ignored;
        else if (isPreviousIndexInParagraphRange && char.IsWhiteSpace(text[startIndex - 1])
                                                 && isNextIndexInParagraphRange &&
                                                 !char.IsWhiteSpace(text[endIndex + 1]))
            tagType = TagType.OpenTag;
        else if (!isPreviousIndexInParagraphRange)
            tagType = TagType.OpenTag;
        else if (!isNextIndexInParagraphRange)
            tagType = TagType.ClosingTag;
        else if (isNextIndexInParagraphRange && char.IsWhiteSpace(text[endIndex + 1])
                                             && isPreviousIndexInParagraphRange &&
                                             !char.IsWhiteSpace(text[startIndex - 1]))
            tagType = TagType.ClosingTag;

        return tagType;
    }

    private static bool IsTagsPared(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
    {
        if (firstTag.ToString() == secondTag.ToString())
        {
            if (firstTag.Value.TagType == TagType.Undefined && secondTag.Value.TagType == TagType.ClosingTag)
            {
                firstTag.Value.TagType = TagType.OpenTag;
                return true;
            }

            if (firstTag.Value.TagType == TagType.OpenTag && secondTag.Value.TagType == TagType.Undefined)
            {
                secondTag.Value.TagType = TagType.ClosingTag;
                return true;
            }

            if (firstTag.Value.TagType == TagType.Undefined 
                && secondTag.Value.TagType == TagType.Undefined
                && IsTagsInsideOneWord(firstTag, secondTag, parsedTokens))
            {
                firstTag.Value.TagType = TagType.OpenTag;
                secondTag.Value.TagType = TagType.ClosingTag;
                return true;
            }

            if (firstTag.Value.TagType != secondTag.Value.TagType
                && firstTag.Value.TagType != TagType.Ignored
                && secondTag.Value.TagType != TagType.Ignored)
                return true;
        }

        return false;
    }

    private static bool IsTagsInsideOneWord(TagToken firstTag, TagToken secondTag,
        Dictionary<int, TagToken> parsedTokens)
    {
        var nextTokenAfterFirstTag = parsedTokens[firstTag.EndIndex + 1];
        return nextTokenAfterFirstTag.EndIndex == secondTag.StartIndex - 1
               && !nextTokenAfterFirstTag.ToString().Contains(" ");
    }

    private bool IsEscapeCharacter(string character)
    {
        return _escapeCharacters.Contains(character);
    }

    private void AddTag(TagToken currentTag, Stack<TagToken> previousTags, Dictionary<int, TagToken> parsedTokens)
    {
        if (previousTags.Count == 0)
        {
            previousTags.Push(currentTag);
            return;
        }

        if (TryAddEscapeCharacter(currentTag, previousTags, parsedTokens))
            return;

        var previousTag = previousTags.Peek();
        if (currentTag.Value.TagType == TagType.Ignored)
        {
            parsedTokens.Add(currentTag.StartIndex, currentTag);
        }
        else if (IsTagsPared(previousTag, currentTag, parsedTokens))
        {
            previousTags.Pop();
            if (previousTag.EndIndex + 1 == currentTag.StartIndex)
            {
                previousTag.Value.TagType = TagType.Ignored;
                currentTag.Value.TagType = TagType.Ignored;
            }

            parsedTokens.Add(previousTag.StartIndex, previousTag);
            parsedTokens.Add(currentTag.StartIndex, currentTag);
            UpdateNestedTags(currentTag, previousTag, parsedTokens);
        }
        else
        {
            previousTags.Push(currentTag);
        }
    }

    public bool TryAddEscapeCharacter(TagToken currentTag, Stack<TagToken> previousTags,
        Dictionary<int, TagToken> parsedTokens)
    {
        var previousTag = previousTags.Peek();
        if (!IsEscapeCharacter(previousTag.ToString()))
            return false;

        previousTags.Pop();
        if (previousTag.EndIndex + 1 == currentTag.StartIndex)
        {
            currentTag.Value.TagType = TagType.Ignored;
            parsedTokens.Add(previousTag.StartIndex, currentTag);
        }
        else
        {
            if (IsEscapeCharacter(currentTag.ToString()))
                previousTags.Push(currentTag);
            parsedTokens.Add(previousTag.StartIndex, previousTag);
        }

        return true;
    }

    private void UpdateNestedTags(TagToken currentTag, TagToken previousTag, Dictionary<int, TagToken> parsedTokens)
    {
        if (_tagsDefenitions[TagDefinition.Italic] == currentTag.ToString())
        {
            var nextTag = parsedTokens[previousTag.EndIndex + 1];
            while (nextTag != currentTag)
            {
                if (_tagsDefenitions[TagDefinition.Strong] == nextTag.ToString())
                    nextTag.Value.TagType = TagType.Ignored;
                nextTag = parsedTokens[nextTag.EndIndex + 1];
            }
        }
    }

    private void AddNotPairedTags(Stack<TagToken> tags, Dictionary<int, TagToken> parsedTokens)
    {
        while (tags.Count > 0)
        {
            var tag = tags.Pop();
            tag.Value.TagType = TagType.Ignored;
            parsedTokens.Add(tag.StartIndex, tag);
        }
    }

    private List<TagToken> SortTokens(Dictionary<int, TagToken> tokens, int startIndex)
    {
        var startedIndex = tokens.Keys.Min();
        startedIndex = startIndex;
        var sortedTokens = new List<TagToken>(tokens.Count);
        while (sortedTokens.Count < tokens.Count)
        {
            var token = tokens[startedIndex];
            sortedTokens.Add(token);
            startedIndex = token.EndIndex + 1;
        }

        return sortedTokens;
    }
}