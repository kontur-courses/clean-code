using System.Collections.Immutable;
using Markdown.Parsers.Markdown.Rules;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers.Markdown;

public class MarkdownParser : ITextParser<Tag>
{
    private readonly HashSet<string> _escapeCharacters;
    private readonly int _tagMaxLength;
    private readonly HashSet<string> _tags;

    public MarkdownParser(HashSet<string> escapeCharacters, ImmutableDictionary<TagDefinition, string> tagsDefenitions)
    {
        _escapeCharacters = escapeCharacters;
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
            var paragraphSortedTokens = SortTokens(paragraphTokens, startIndex);
            tokens.AddRange(paragraphSortedTokens);
            startIndex = tokens.Last().EndIndex + 1;
        }

        return tokens;
    }

    private Dictionary<int, TagToken> ParseParagraph(int startIndexInText, string sequence)
    {
        var previousTags = new Stack<TagToken>();
        var parsedTokens = new Dictionary<int, TagToken>();
        var textStartIndex = startIndexInText;
        for (var currentIndexInParagraph = 0; currentIndexInParagraph < sequence.Length; currentIndexInParagraph++)
            for (var tagLength = _tagMaxLength; tagLength > 0; tagLength--)
            {
                if (tagLength + currentIndexInParagraph > sequence.Length) continue;

                var currentText = sequence.Substring(currentIndexInParagraph, tagLength);
                if (IsTag(currentText) || IsEscapeCharacter(currentText))
                {
                    var textTokenBeforeTag = GetTextToken(textStartIndex, startIndexInText, currentIndexInParagraph, sequence);
                    AddTextToken(textTokenBeforeTag, parsedTokens);
                    var currentTag = GetTag(currentIndexInParagraph, tagLength, sequence, startIndexInText);
                    AddTag(currentTag, previousTags, parsedTokens);
                    textStartIndex = startIndexInText + tagLength + currentIndexInParagraph;
                    currentIndexInParagraph = currentIndexInParagraph + tagLength - 1;
                    break;
                }
            }

        AddTextToken(GetTextToken(textStartIndex, startIndexInText, sequence.Length, sequence), parsedTokens);
        AddNotPairedTags(previousTags, parsedTokens);

        return parsedTokens;
    }

    private bool IsTag(string tag) =>
        _tags.Contains(tag);

    private static void AddTextToken(TagToken textToken, Dictionary<int, TagToken> parsedTokens)
    {
        if (string.IsNullOrEmpty(textToken.ToString()))
            return;

        parsedTokens.Add(textToken.StartIndex, textToken);
    }

    private static TagToken GetTextToken(int textStartIndex, int paragraphStartIndex, int currentIndexInParagrapgh, string paragraph)
    {
        var tokenStartIndexInParagraph = textStartIndex - paragraphStartIndex;
        var tokenEndIndexInParagraph = currentIndexInParagrapgh + paragraphStartIndex - textStartIndex;
        var tokenText = paragraph.Substring(tokenStartIndexInParagraph, tokenEndIndexInParagraph);
        var token = new TagToken(
            textStartIndex,
            currentIndexInParagrapgh + paragraphStartIndex - 1,
            new Tag(tokenText, TagType.Ignored));

        return token;
    }

    private static TagToken GetTag(int startIndex, int tagLength, string text, int paragraphStartIndex)
    {
        var endIndex = startIndex + tagLength - 1;
        var tagValue = text.Substring(startIndex, tagLength);
        var tagType = GetTagType(tagValue, startIndex, endIndex, text);
        var tag = new Tag(tagValue, tagType);
        var token = new TagToken(startIndex + paragraphStartIndex, endIndex + paragraphStartIndex, tag);

        return token;
    }

    private static TagType GetTagType(string tag, int startIndex, int endIndex, string text)
    {
        var tagType = TagType.Undefined;
        var isPreviousIndexInParagraphRange = startIndex - 1 >= 0;
        var isNextIndexInParagraphRange = endIndex + 2 <= text.Length;
        var nextSymbol = isNextIndexInParagraphRange ? text[endIndex + 1] : MarkdownRules.EmptyChar;
        var previousSymbol = isPreviousIndexInParagraphRange ? text[startIndex - 1] : MarkdownRules.EmptyChar;

        if (MarkdownRules.IsTagIgnoredBySymbol(tag, previousSymbol) || MarkdownRules.IsTagIgnoredBySymbol(tag, nextSymbol))
            tagType = TagType.Ignored;
        else if (MarkdownRules.IsTagOpen(tag, previousSymbol, nextSymbol))
            tagType = TagType.OpenTag;
        else if (MarkdownRules.IsTagClosing(tag, previousSymbol, nextSymbol))
            tagType = TagType.ClosingTag;

        return tagType;
    }

    private static bool IsTagsPared(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
    {
        var isTagsNotIgnored = firstTag.Value.TagType != TagType.Ignored && secondTag.Value.TagType != TagType.Ignored;
        if (isTagsNotIgnored && MarkdownRules.IsTagsPaired(firstTag, secondTag, parsedTokens))
        {
            firstTag.Value.TagType = TagType.OpenTag;
            secondTag.Value.TagType = TagType.ClosingTag;
            return true;
        }

        return false;
    }

    private bool IsEscapeCharacter(string character) =>
        _escapeCharacters.Contains(character);

    private void AddTag(TagToken currentTag, Stack<TagToken> previousTags, Dictionary<int, TagToken> parsedTokens)
    {
        if (!previousTags.TryPeek(out var previousTag))
            previousTags.Push(currentTag);
        else if (IsEscapeCharacter(previousTag.ToString()))
            AddEscapeCharacter(currentTag, previousTags, parsedTokens);
        else if (currentTag.Value.TagType == TagType.Ignored)
            parsedTokens.Add(currentTag.StartIndex, currentTag);
        else if (IsTagsPared(previousTag, currentTag, parsedTokens))
        {
            UpdateTagsBeforeAdding(previousTag, currentTag);
            previousTags.Pop();
            parsedTokens.Add(previousTag.StartIndex, previousTag);
            parsedTokens.Add(currentTag.StartIndex, currentTag);
            UpdateNestedTags(currentTag, previousTag, parsedTokens);
        }
        else
            previousTags.Push(currentTag);
    }

    private void AddEscapeCharacter(TagToken currentTag, Stack<TagToken> previousTags, Dictionary<int, TagToken> parsedTokens)
    {
        var previousTag = previousTags.Pop();
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
    }

    private static void UpdateTagsBeforeAdding(TagToken firstTag, TagToken secondTag)
    {
        if (MarkdownRules.IsTagsIgnored(firstTag, secondTag))
        {
            firstTag.Value.TagType = TagType.Ignored;
            secondTag.Value.TagType = TagType.Ignored;
        }
    }

    private static void UpdateNestedTags(TagToken currentTag, TagToken previousTag, Dictionary<int, TagToken> parsedTokens)
    {
        var nestedTagsToIgnore = MarkdownRules.GetIgnoredNestedTags(currentTag.ToString());
        if (nestedTagsToIgnore.Count == 0)
            return;

        var nextTag = parsedTokens[previousTag.EndIndex + 1];
        while (nextTag != currentTag)
        {
            if (nestedTagsToIgnore.Contains(nextTag.ToString()))
                nextTag.Value.TagType = TagType.Ignored;
            nextTag = parsedTokens[nextTag.EndIndex + 1];
        }
    }

    private static void AddNotPairedTags(Stack<TagToken> tags, Dictionary<int, TagToken> parsedTokens)
    {
        var lastIndex = tags.Count > 0 ? Math.Max(tags.Peek().EndIndex, parsedTokens.Max(t => t.Value.EndIndex)) : 0;
        while (tags.Count > 0)
        {
            var tag = tags.Pop();
            var tagValue = tag.ToString();
            tag.Value.TagType = TagType.Ignored;

            if (MarkdownRules.IsTagSingle(tagValue))
            {
                tag.Value.TagType = TagType.OpenTag;
                var closingTag = new Tag(tagValue, TagType.ClosingTag);
                var closingTagToken = new TagToken(lastIndex + 1, lastIndex + 1 + tagValue.Length, closingTag);
                parsedTokens.Add(closingTagToken.StartIndex, closingTagToken);
                lastIndex = lastIndex + 1 + tagValue.Length;
            }

            parsedTokens.Add(tag.StartIndex, tag);
        }
    }

    private static List<TagToken> SortTokens(Dictionary<int, TagToken> tokens, int startIndex)
    {
        var sortedTokens = new List<TagToken>(tokens.Count);
        while (sortedTokens.Count < tokens.Count)
        {
            var token = tokens[startIndex];
            sortedTokens.Add(token);
            startIndex = token.EndIndex + 1;
        }

        return sortedTokens;
    }
}