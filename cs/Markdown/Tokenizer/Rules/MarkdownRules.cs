using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Rules;

/// <summary>
/// Правила парсинга для тегов Markdown
/// </summary>
public class MarkdownRules
{
    public readonly Dictionary<Type, Rule> TagRules = new()
    {
        [typeof(HeaderTag)] = new Rule
        {
            IsTag = (tag, content, position) =>
            {
                var isClosingTag = content.ContainsSubstringOnIndex(tag.MdClosingTag, position);
                var isOpeningTag = content.ContainsSubstringOnIndex(tag.MdTag, position);

                if (isClosingTag)
                {
                    for (var i = position - 1; i >= 0; i--)
                    {
                        if (content[i] == '#')
                            return true;
                        if (content[i] == '\n')
                            break;
                    }
                }

                if (isOpeningTag)
                {
                    return position == 0 || content[position - 1] == '\n';
                }

                return false;
            },
        },
    };

    public readonly Dictionary<Type, Rule> Rules = new()
    {
        [typeof(CursiveTag)] = new Rule
        {
            IsValid = (tagToken, content, isClosingTag, orderedTags) =>
            {
                return !IsTagInWordWithDigits(tagToken, content) &&
                       !IsStartOrEndWhitespace(tagToken, content, isClosingTag) &&
                       !IsEmptyContent(tagToken, orderedTags) &&
                       !IsTagBorderInWord(tagToken, content, orderedTags);
            }
        },
        [typeof(StrongTag)] = new Rule
        {
            IsValid = (tagToken, content, isClosingTag, orderedTags) =>
            {
                return !IsTagInWordWithDigits(tagToken, content) &&
                       !IsStartOrEndWhitespace(tagToken, content, isClosingTag) &&
                       !IsEmptyContent(tagToken, orderedTags) &&
                       !IsTagBorderInWord(tagToken, content, orderedTags);
            }
        },
    };

    private static bool IsEmptyContent(TagToken openingTag, List<TagToken> orderedTags)
    {
        var tagIndex = orderedTags.IndexOf(openingTag);
        if (tagIndex + 1 < orderedTags.Count)
        {
            var closingTag = orderedTags[tagIndex + 1];
            if (openingTag.Tag.Matches(closingTag.Tag))
            {
                var contentLength = closingTag.Position - (openingTag.Position + openingTag.Tag.MdTag.Length);
                if (contentLength <= 0)
                    Console.WriteLine(
                        $"Invalid tag: {openingTag.Tag.MdTag.GetType()} at {openingTag.Position} (empty content)");
                return contentLength <= 0;
            }
        }

        return false;
    }


    private static bool IsStartOrEndWhitespace(TagToken tagToken, string content, bool isClosingTag)
    {
        var pos = tagToken.Position;
        var tagLen = (isClosingTag ? tagToken.Tag.MdClosingTag : tagToken.Tag.MdTag).Length;
        var tagEnd = pos + tagLen - 1;

        var charBefore = pos > 0 ? content[pos - 1] : '\0';
        var charAfter = tagEnd + 1 < content.Length ? content[tagEnd + 1] : '\0';

        var invalid = char.IsWhiteSpace(isClosingTag ? charBefore : charAfter);
        if (invalid)
            Console.WriteLine(
                $"Invalid tag: {tagToken.Tag.GetType()} at {tagToken.Position} (start or end whitespace)");
        return invalid;
    }

    private static bool IsTagBorderInWord(TagToken tag, string content, List<TagToken> orderedTags)
    {
        var isClosing = content.ContainsSubstringOnIndex(tag.Tag.MdClosingTag, tag.Position);
        var tagLength = (isClosing ? tag.Tag.MdClosingTag : tag.Tag.MdTag).Length;
        var tagEnd = tag.Position + tagLength - 1;
        //next symbol
        if (tagEnd + 1 == content.Length || char.IsWhiteSpace(content[tagEnd + 1]))
            return false;
        //previous symbol
        var previousTag = orderedTags.LastOrDefault(t => t.Position < tag.Position);
        if (tag.Position == 0 || previousTag?.Position + previousTag?.Tag.MdTag.Length == tag.Position)
            return false;

        //escape at the beginning of the word
        for (var i = tag.Position - 1; i >= 0; i--)
        {
            if (content.IsEscaped(i))
                continue;
            if (i == 0 || char.IsWhiteSpace(content[i]))
                return false;
            if (char.IsLetter(content[i]))
                break;
        }

        var found = FindInAdjacentWord(tag, content,
            (i, s) => s.ContainsSubstringOnIndex(isClosing ? tag.Tag.MdClosingTag : tag.Tag.MdTag, i));
        if (!found)
            Console.WriteLine($"Invalid tag: {tag.Tag.GetType()} at {tag.Position} (tag border in word)");
        return !found;
    }

    private static bool IsTagInWordWithDigits(TagToken tagToken, string content)
    {
        var isClosing = content.ContainsSubstringOnIndex(tagToken.Tag.MdClosingTag, tagToken.Position);
        var tagLength = (isClosing ? tagToken.Tag.MdClosingTag : tagToken.Tag.MdTag).Length;
        var tagEnd = tagToken.Position + tagLength - 1;
        //next symbol
        if (tagEnd + 1 == content.Length || char.IsWhiteSpace(content[tagEnd + 1]))
            return false;
        //previous symbol
        if (tagToken.Position == 0 || char.IsWhiteSpace(content[tagToken.Position - 1]))
            return false;

        var found = FindInAdjacentWord(tagToken, content, (i, s) => char.IsDigit(s[i]));
        if (found)
            Console.WriteLine($"Invalid tag: {tagToken.Tag.MdTag.GetType()} at {tagToken.Position} (word with digits)");
        return found;
    }

    private static bool FindInAdjacentWord(TagToken tagToken, string content, Func<int, string, bool> predicate)
    {
        var isClosing = content.ContainsSubstringOnIndex(tagToken.Tag.MdClosingTag, tagToken.Position);
        var tagLength = (isClosing ? tagToken.Tag.MdClosingTag : tagToken.Tag.MdTag).Length;
        var tagEnd = tagToken.Position + tagLength - 1;

        // Scan forward
        for (var i = tagEnd + 1; i < content.Length && !char.IsWhiteSpace(content[i]); i++)
        {
            if (predicate(i, content))
            {
                return true;
            }
        }

        // Scan backward
        for (var i = tagToken.Position - 1; i >= 0 && !char.IsWhiteSpace(content[i]); i--)
        {
            if (predicate(i, content))
            {
                return true;
            }
        }

        return false;
    }
}