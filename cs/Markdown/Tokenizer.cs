using System.Text;

namespace Markdown;

internal class Tokenizer
{
    private readonly WrapperSettingsProvider settings;
    private readonly TagSetting[] plainTokenTags;
    private readonly TagSetting[] lineOnlyTags;
    public IReadOnlySet<string> SpecialParts { get; private set; }

    public Tokenizer(WrapperSettingsProvider settings)
    {
        this.settings = settings;
        lineOnlyTags = settings.GetSettings(true);
        plainTokenTags = settings.GetSettings(false);
        var allTags = lineOnlyTags.Concat(plainTokenTags);
        SpecialParts = allTags.SelectMany(x => x.SpecialParts)
            .Append("\\")
            //.Distinct()
            //.OrderBy(x => x.Length)
            .ToHashSet();
    }

    public IEnumerable<Token> ParseLines(string text)
    {

        foreach (var line in text.Split('\n'))
        {
            var lineTag = lineOnlyTags.FirstOrDefault(x => line.StartsWith(x.OpeningTag));
            var builder = new StringBuilder(line);
            var root = new TokenBuilder(this, builder, 0, null)
            .WithMdTag(lineTag);
            root.AddToken(ParseLine(builder, root, lineTag?.OpeningTag.Length ?? 0));
            root.WithEnd(builder.Length);
            yield return root.Build();
        }
    }

    public IReadOnlySet<string> GetExcludedParts(TagSetting? tag)
    {
        var excludedParts = new HashSet<string>(SpecialParts);
        excludedParts.ExceptWith(tag?.SpecialParts ?? new HashSet<string>());
        return excludedParts;
    }

    public Token WrapToken(string token, int start, string? mdTag)
    {

        return TryGetSetting(mdTag, out var setting) ? new(token, start, setting, GetExcludedParts(setting)) : new(token, start, null, SpecialParts);
    }

    public Token WrapToken(string token, int start, TagSetting? mdTag)
    {
        return new(token, start, mdTag, GetExcludedParts(mdTag));
    }

    internal bool TryGetSetting(string? mdTag, out TagSetting setting)
    {
        setting = null!;
        return mdTag != null && settings.TryGetSetting(mdTag!, out setting);
    }

    private TokenBuilder ParseLine(StringBuilder line, TokenBuilder? root, int start)
    {
        return ParseToken(line, start, null, root);
    }

    private TokenBuilder ParseToken(StringBuilder line, int start, TagDescriptor? openingDescriptor, TokenBuilder? root, HashSet<string>? usedOpeners = null)
    {
        usedOpeners ??= new HashSet<string>();
        TagSetting? openingTag = null;
        if (openingDescriptor != null)
            openingTag = openingDescriptor.Tag;

        var openingSeparatorLength = openingTag?.OpeningTag?.Length ?? 0;
        var token = new TokenBuilder(this, line, start, root);
        start += openingSeparatorLength;
        var lastKnownTokenEnd = start;

        for (var i = start; i < line.Length; i++)
        {
            var ignoreEscape = false;
            if (line[i] == '\\')
                ignoreEscape = TryEscape(line, i, null);
            if (openingDescriptor != null && CanCloseTag(i, line.ToString(), openingDescriptor, usedOpeners, out var closingTag))
            {
                if (!ignoreEscape && TryEscape(line, i, closingTag))
                {
                    i += closingTag.EndTag.Length - 1;
                    continue;
                }

                usedOpeners.Remove(closingTag.EndTag);
                token.WithEnd(i + openingTag!.EndTag.Length);
                if (openingDescriptor.IsValid)
                    token.WithMdTag(closingTag);
                else
                    token.WithMdTag(null);

                return token;
            }

            if (CanOpenTag(i, line.ToString(), start, openingDescriptor, usedOpeners, out var openedTag))
            {
                if (!ignoreEscape && TryEscape(line, i, openedTag.Tag))
                {
                    i += openedTag.Tag.EndTag.Length - 1;
                    continue;
                }

                TryAddPlainWord(i);
                usedOpeners.Add(openedTag.Tag.OpeningTag);
                var childToken = ParseToken(line, i, openedTag, token, usedOpeners);
                token.AddToken(childToken);
                lastKnownTokenEnd = childToken.End;
                i = lastKnownTokenEnd - 1;
            }
        }

        TryAddPlainWord(line.Length);

        token.WithEnd(line.Length).WithMdTag(null);
        return token;

        void TryAddPlainWord(int wordEnd)
        {
            if (lastKnownTokenEnd != wordEnd)
            {
                var plainToken = CreateTokenBuilder(line, lastKnownTokenEnd, wordEnd, null, token);
                token!.AddToken(plainToken);
            }
        }
    }

    private bool TryEscape(StringBuilder line, int position, TagSetting? tag)
    {
        if (IsEscaped(line, position))
        {
            line.Remove(position - 1, 1);
            return true;
        }

        return false;
    }

    public static bool IsEscaped(StringBuilder line, int position)
    {
        if (line.Length < position || position < 1)
            return false;
        return line[position - 1] == '\\' && !IsEscaped(line, position - 1);
    }

    private bool CanOpenTag(int position, string source, int start, TagDescriptor? openingDescriptor, HashSet<string> usedOpeners, out TagDescriptor tagData)
    {
        tagData = new(null!, false, 0, false);
        if (position < 0 || position >= source.Length)
            return false;
        if (TryGetCurrentTag(position, source, plainTokenTags, null, out var openedTag))
        {
            var openingTag = openingDescriptor != null ? openingDescriptor.Tag : null;
            var isValid = openedTag.CanBeNestedIn(openingTag);
            var tagEndPosition = position + openedTag.OpeningTag.Length;
            if (tagEndPosition <= source.Length)
            {
                if (tagEndPosition != source.Length && IsFreeSpace(source, tagEndPosition, 0, openingDescriptor))
                    return false;
            }
            else
            {
                return false;
            }

            var isInWord = IsInWord(source[start..], position - start, openedTag.OpeningTag.Length, openingDescriptor, start);
            tagData = new(openedTag, isInWord, position, isValid);
            return true;
        }

        return false;
    }

    private bool CanCloseTag(int position, string source, TagDescriptor openedDescriptor, HashSet<string> usedOpeners, out TagSetting closingTag)
    {
        closingTag = null!;
        if (position < 1 || position >= source.Length)
            return false;
        if (TryGetCurrentTag(position, source, plainTokenTags, openedDescriptor.Tag, out closingTag))
        {
            if (openedDescriptor.Tag != closingTag)
                return false;
            if (IsFreeSpace(source, position - 1, 0, null))
                return false;
            var wordEnd = position + closingTag.EndTag.Length;
            var word = source[openedDescriptor.Start..wordEnd];
            var isInWord = IsInWord(word, position - openedDescriptor.Start, closingTag.EndTag.Length, null, openedDescriptor.Start);
            if (openedDescriptor.IsInWord || isInWord)
            {
                if (word.Any(x => char.IsWhiteSpace(x) || char.IsNumber(x)))
                    return false;
            }

            if (TryGetCurrentTag(position, source, plainTokenTags, null, out var intersectingTag))
            {
                if (intersectingTag != closingTag)
                {
                    if (!TryGetCurrentTag(position + intersectingTag.EndTag.Length, source, plainTokenTags, null, out _))
                    {
                        openedDescriptor.Invalidate();
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    private static bool IsFreeSpace(string source, int position, int sourceStart, TagDescriptor? previousTag)
    {
        if (previousTag != null)
        {
            if (position < previousTag.Start + previousTag.Tag.OpeningTag.Length - sourceStart)
                return false;
        }

        return char.IsWhiteSpace(source[position]);
    }

    private static bool IsInWord(string source, int position, int length, TagDescriptor? openingDescriptor, int sourceStart)
    {

        if (position < 1 || position + length >= source.Length)
            return false;
        var beforeChar = source[position - 1];
        if (IsFreeSpace(source, position - 1, sourceStart, openingDescriptor) || beforeChar == '\\')
            return false;
        var afterChar = source[position + length];
        if (IsFreeSpace(source, position + length, sourceStart, openingDescriptor) || afterChar == '\\')
            return false;
        return true;
    }

    private TokenBuilder CreateTokenBuilder(StringBuilder source, int start, int end, TagSetting? mdTag, TokenBuilder? root)
    {
        return new TokenBuilder(this, source, start, root)
                  .WithMdTag(mdTag)
                  .WithEnd(end);
    }

    private bool TryGetCurrentTag(int i, string line, TagSetting[] tags, TagSetting? openedTag, out TagSetting currentTag)
    {
        IEnumerable<TagSetting> finalSeparators = tags;
        if (openedTag != null)
            finalSeparators = finalSeparators.Where(x => x != openedTag).Prepend(openedTag);
        foreach (var tag in finalSeparators)
        {
            var separator = GetSeparator(tag);
            if (i + separator.Length > line.Length)
                continue;
            var separatorEnd = i + separator.Length;
            if (line[i..separatorEnd] == separator)
            {
                currentTag = tag;
                return true;
            }
        }

        currentTag = null!;
        return false;

        string GetSeparator(TagSetting tag) => openedTag != null ? tag.EndTag : tag.OpeningTag;
    }

    private IEnumerable<TagSetting> GetCurrentTags(int i, string line, TagSetting[] tags, TagSetting? openedTag)
    {
        IEnumerable<TagSetting> finalSeparators = tags;
        if (openedTag != null)
            finalSeparators = finalSeparators.Where(x => x != openedTag).Prepend(openedTag);
        foreach (var tag in finalSeparators)
        {
            var separator = GetSeparator(tag);
            if (i + separator.Length > line.Length)
                continue;
            var separatorEnd = i + separator.Length;
            if (line[i..separatorEnd] == separator)
            {
                yield return tag;
            }
        }

        string GetSeparator(TagSetting tag) => openedTag != null ? tag.EndTag : tag.OpeningTag;
    }

    internal record TagDescriptor(TagSetting Tag, bool IsInWord, int Start, bool IsValid)
    {
        public bool IsValid { get; private set; } = IsValid;

        public void Invalidate()
        {
            IsValid = false;
        }

        public void Deconstruct(out TagSetting tag, out bool isInWord, out int start, out bool isValid)
        {
            tag = Tag;
            isInWord = IsInWord;
            start = Start;
            isValid = IsValid;
        }
    }
}