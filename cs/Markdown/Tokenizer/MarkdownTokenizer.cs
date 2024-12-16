using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokenizer.Rules;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer;

/// <summary>
/// Токенайзер - переводит строку Markdown в токены. Учитывает правила
/// </summary>
/// <seealso cref="markdownRules" />
public class MarkdownTokenizer : ITokenizer
{
    private readonly MarkdownRules markdownRules = new();

    private readonly List<ITag> supportedTags = new()
    {
        new StrongTag(),
        new CursiveTag(),
        new HeaderTag(),
        new ImageTag(),
        new NewLineTag()
    };


    public List<IToken> Tokenize(string content)
    {
        var escapePositions = GetEscapePositions(content);
        var tagLines = IdentifyTags(content);

        foreach (var tagLine in tagLines) RemoveInvalidTags(tagLine, content);

        var escapedContent = RemoveEscapeCharacters(content, escapePositions, tagLines);

        var cleanTagTokens = tagLines.SelectMany(t => t).ToList();
        var tokenTree = MarkdownTokenTreeBuilder.BuildTokenTree(cleanTagTokens, escapedContent);

        return tokenTree;
    }

    private List<List<TagToken>> IdentifyTags(string content)
    {
        var lines = new List<List<TagToken>>();
        var tagTokens = new List<TagToken>();
        for (var i = 0; i < content.Length; i++)
            foreach (var tag in supportedTags)
            {
                // Try to get rule for tag
                if (markdownRules.TagRules.TryGetValue(tag.GetType(), out var rule))
                    // If rule determines that tag is not a tag, skip it
                    if (rule.IsTag != null && !rule.IsTag(tag, content, i))
                        continue;

                // If tag is not self-closing, and it's not a closing tag
                if (content.ContainsSubstringOnIndex(tag.MdTag, i) && !content.IsEscaped(i))
                {
                    if (tag is NewLineTag)
                    {
                        if (tagTokens.FirstOrDefault()?.Tag is not HeaderTag)
                            tagTokens.Add(new TagToken(tag) { Position = i });
                        lines.Add(tagTokens);
                        tagTokens = new List<TagToken>();
                        break;
                    }

                    tagTokens.Add(new TagToken(tag) { Position = i });
                    i += tag.MdTag.Length - 1;
                    break;
                }

                // If tag is self-closing or closing tag
                if (tag.SelfClosing || !content.ContainsSubstringOnIndex(tag.MdClosingTag, i) || content.IsEscaped(i))
                    continue;
                tagTokens.Add(new TagToken(tag) { Position = i });
                // If tag is closing tag for header, create new line
                if (tag is HeaderTag)
                {
                    lines.Add(tagTokens);
                    tagTokens = new List<TagToken>();
                    break;
                }
                i += tag.MdClosingTag.Length - 1;
                break;
            }

        lines.Add(tagTokens);

        return lines;
    }

    private List<int> GetEscapePositions(string content)
    {
        var escapePositions = new List<int>();

        for (var i = 0; i < content.Length; i++)
        {
            if (content[i] != '\\') continue;
            if (i + 1 >= content.Length || (content[i + 1] != '\\' && !supportedTags.Any(tag =>
                    content.ContainsSubstringOnIndex(tag.MdTag, i + 1) ||
                    (!tag.SelfClosing && content.ContainsSubstringOnIndex(tag.MdClosingTag, i + 1))))) continue;
            escapePositions.Add(i);
            i++;
        }

        return escapePositions;
    }


    private static string RemoveEscapeCharacters(string content, List<int> escapePositions, List<List<TagToken>> tags)
    {
        var sb = new StringBuilder(content);
        foreach (var pos in escapePositions.OrderByDescending(p => p)) sb.Remove(pos, 1);

        content = sb.ToString();

        foreach (var tagToken in tags.SelectMany(tagTokens => tagTokens))
            tagToken.Position -= escapePositions.Count(pos => pos < tagToken.Position);

        return content;
    }

    private void RemoveInvalidTags(List<TagToken> foundTags, string content)
    {
        var invalidTags = new HashSet<TagToken>();
        var tagStack = new Stack<TagToken>();

        var overlaps = FindOverlappingTags(foundTags);
        foundTags.RemoveAll(t => overlaps.Contains(t));

        foreach (var tagToken in foundTags)
        {
            var tag = tagToken.Tag;
            var isClosingTag = IsClosingTag(tag, tagToken.Position, content, tagStack);

            if (IsTagInvalid(tagToken, content, isClosingTag, foundTags))
            {
                invalidTags.Add(tagToken);
                continue;
            }

            if (IsOpeningTag(tagStack, tagToken))
            {
                if (IsDisallowedChild(tagStack, tagToken))
                {
                    invalidTags.Add(tagToken);
                    continue;
                }

                if (!tagToken.Tag.SelfClosing)
                    tagStack.Push(tagToken);
            }
            else
            {
                HandleClosingTag(tagStack, tagToken, invalidTags);
            }
        }

        while (tagStack.Count > 0)
            invalidTags.Add(tagStack.Pop());

        foundTags.RemoveAll(t => invalidTags.Contains(t));
    }

    private static HashSet<TagToken> FindOverlappingTags(List<TagToken> tagTokens)
    {
        var overlaps = new HashSet<ITag>();
        var tagStack = new Stack<TagToken>();
        var invalidTags = new HashSet<TagToken>();

        foreach (var tagToken in tagTokens)
        {
            if (overlaps.Contains(tagToken.Tag))
            {
                invalidTags.Add(tagToken);
                overlaps.Remove(tagToken.Tag);
                continue;
            }
            if (IsOpeningTag(tagStack, tagToken))
                tagStack.Push(tagToken);
            else
            {
                if (tagStack.Count > 0 && tagStack.Peek().Tag.Matches(tagToken.Tag))
                {
                    tagStack.Pop();
                    continue;
                }
                invalidTags.Add(tagToken);
                overlaps.Add(tagToken.Tag);
                TagToken token;
                while (tagStack.Count > 0 && !tagStack.Peek().Tag.Matches(tagToken.Tag))
                {
                    token = tagStack.Pop();
                    invalidTags.Add(token);
                    overlaps.Add(token.Tag);
                }
                token = tagStack.Pop();
                invalidTags.Add(token);
                overlaps.Add(token.Tag);
            }
        }

        return invalidTags;
    }

    private static bool IsClosingTag(ITag tag, int position, string content, Stack<TagToken> tagStack)
    {
        return !tag.SelfClosing && content.ContainsSubstringOnIndex(tag.MdClosingTag, position)
                                && tagStack.Any(tagToken => tagToken.Tag.GetType() == tag.GetType());
    }

    private bool IsTagInvalid(TagToken tagToken, string content, bool isClosingTag, List<TagToken> orderedTags)
    {
        var tagType = tagToken.Tag.GetType();
        if (!markdownRules.Rules.TryGetValue(tagType, out var rule)) return false;
        return rule.IsValid != null && !rule.IsValid(tagToken, content, isClosingTag, orderedTags);
    }

    private static bool IsOpeningTag(Stack<TagToken> tagStack, TagToken tagToken)
    {
        return tagStack.Count == 0 || (!tagToken.Tag.Matches(tagStack.Peek().Tag) &&
                                       !tagStack.Any(t => t.Tag.Matches(tagToken.Tag)));
    }

    private static bool IsDisallowedChild(Stack<TagToken> tagStack, TagToken tagToken)
    {
        return tagStack.Count > 0 && tagStack.Peek().Tag.DisallowedChildren
            .Any(t => t.GetType() == tagToken.Tag.GetType());
    }

    private static void HandleClosingTag(Stack<TagToken> tagStack, TagToken tagToken, HashSet<TagToken> invalidTags)
    {
        var openingTag = tagStack.Pop();

        var contentLength = tagToken.Position - (openingTag.Position + openingTag.Tag.MdTag.Length);
        if (contentLength > 0) return;
        invalidTags.Add(openingTag);
        invalidTags.Add(tagToken);
    }
}