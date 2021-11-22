using System.Text;

namespace Markdown;

internal class Tokenizer
{
    private readonly WrapperSettingsProvider settings;

    public Tokenizer(WrapperSettingsProvider settings)
    {
        this.settings = settings;
    }

    public IEnumerable<Token> ParseLines(string text)
    {
        var lineTags = settings.GetSettings(true);
        var openers = settings.GetOpeningTags(false);
        var closers = settings.GetClosingTags(false);
        foreach (var line in text.Split('\n'))
        {
            var lineTag = lineTags.FirstOrDefault(x => line.StartsWith(x.OpeningTag));
            var builder = new StringBuilder(line);
            var root = new TokenBuilder(builder, 0, null)
            .WithMdTag(lineTag);
            root.AddToken(ParseLine(builder, openers, closers, root));
            root.WithEnd(builder.Length);
            yield return root.Build();
        }
    }

    public Token WrapToken(string token, int start, string? mdTag)
    {
        return TryGetSetting(mdTag, out var setting) ? new(token, start, setting) : new(token, start, null);
    }

    public static Token WrapToken(string token, int start, TagSetting? mdTag)
    {
        return new(token, start, mdTag);
    }

    internal bool TryGetSetting(string? mdTag, out TagSetting setting)
    {
        setting = null!;
        return mdTag != null && settings.TryGetSetting(mdTag!, out setting);
    }

    private TokenBuilder ParseLine(StringBuilder line, string[] openers, string[] closers, TokenBuilder? root)
    {
        return ParseToken(line, 0, null, openers, closers, root);
    }

    private TokenBuilder ParseToken(StringBuilder line, int start, TagDescriptor? openingDescriptor, string[] openers, string[] closers, TokenBuilder? root)
    {
        TagSetting? openingTag = null;
        var isInWord = false;
        if (openingDescriptor != null)
        {
            (openingTag, isInWord,_) = openingDescriptor.Value;
        }

        var openingSeparatorLength = openingTag?.OpeningTag?.Length ?? 0;
        var token = new TokenBuilder(line, start, root)
            .WithMdTag(openingTag);
        start += openingSeparatorLength;
        var lastKnownTokenEnd = start;

        for (var i = start; i < line.Length; i++)
        {
            if (CanOpenTag(i, line.ToString(), out var openedTag))
            {
                TryAddPlainWord(i);
                var childToken = ParseToken(line, i, openedTag, openers, closers, token);
                token.AddToken(childToken);
                lastKnownTokenEnd = childToken.End;
                i = lastKnownTokenEnd - 1;
            }

            if (openingDescriptor.HasValue && CanCloseTag(i, line.ToString(), openingDescriptor.Value))
            {
                token.WithMdTag(openingTag).WithEnd(i + openingTag!.EndTag.Length);
                return token;
            }

        }

        TryAddPlainWord(line.Length);

        token.WithEnd(line.Length);
        return token;

        bool CanOpenTag(int position, string source, out TagDescriptor tagData)
        {
            tagData = new(null!, false,0);
            if (position < 0 || position >= source.Length)
                return false;
            if (TryGetCurrentTag(position, source, openers, null, out var openedTag))
            {
                var tagEndPosition = position + openedTag.OpeningTag.Length;
                if (tagEndPosition < source.Length)
                {
                    if (char.IsWhiteSpace(source[tagEndPosition]))
                        return false;
                }
                else
                {
                    return false;
                }

                var isInWord = IsInWord(source, position, openedTag.OpeningTag.Length);
                tagData = new(openedTag, isInWord,position);
                return true;
            }

            return false;
        }

        bool CanCloseTag(int position, string source, TagDescriptor openingTag)
        {
            if (position < 1 || position >= source.Length)
                return false;
            if (TryGetCurrentTag(position, source, closers, openingTag.Tag.EndTag, out var closingTag))
            {
                if (char.IsWhiteSpace(source[position - 1]))
                    return false;
                var isInWord = IsInWord(source, position, closingTag.EndTag.Length);
                if (openingTag.IsInWord || isInWord)
                {
                    if (source[openingTag.Start..position].Any(x => char.IsWhiteSpace(x) || char.IsNumber(x)))
                        return false;
                }

                return true;
            }

            return false;
        }

        void TryAddPlainWord(int wordEnd)
        {
            if (lastKnownTokenEnd != wordEnd)
            {
                var plainToken = CreateTokenBuilder(line, lastKnownTokenEnd, wordEnd, null, token);
                token!.AddToken(plainToken);
            }
        }
    }

    private static bool IsInWord(string source, int position, int length)
    {
        if (position < 1 || position + length >= source.Length)
            return false;
        var beforeChar = source[position - 1];
        if (char.IsWhiteSpace(beforeChar))
            return false;
        var afterChar = source[position + length];
        if (char.IsWhiteSpace(afterChar))
            return false;
        return true;
    }

    private TokenBuilder CreateTokenBuilder(StringBuilder source, int start, int end, TagSetting? mdTag, TokenBuilder? root)
    {
        return new TokenBuilder(source, start, root)
                  .WithMdTag(mdTag)
                  .WithEnd(end);
    }

    private bool TryGetCurrentTag(int i, string line, string[] separators, string? prioritizedSeparator, out TagSetting currentTag)
    {
        IEnumerable<string> finalSeparators = separators;
        if (prioritizedSeparator != null)
            finalSeparators = finalSeparators.Where(x => x != prioritizedSeparator).Prepend(prioritizedSeparator);
        foreach (var separator in finalSeparators)
        {
            if (i + separator.Length > line.Length)
                continue;
            var separatorEnd = i + separator.Length;
            if (line[i..separatorEnd] == separator)
                return TryGetSetting(separator, out currentTag);
        }

        currentTag = null!;
        return false;
    }

    private static bool IsEscaped(StringBuilder source, int position)
    {
        if (position < 1)
            return false;

        return source[position - 1] == '\\' && !IsEscaped(source, position - 1);
    }
}

internal record struct TagDescriptor(TagSetting Tag, bool IsInWord, int Start)
{
    public void Deconstruct(out TagSetting tag, out bool isInWord, out int start)
    {
        tag = Tag;
        isInWord = IsInWord;
        start = Start;
    }
}
