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
        var lineTags = settings.GetTokenSeparators(true);
        var tokenSeparators = settings.GetTokenSeparators();
        foreach (var line in text.Split('\n'))
        {
            var lineTag = lineTags.FirstOrDefault(x => line.StartsWith(x));
            var builder = new StringBuilder(line);
            var root = new TokenBuilder(this, builder, 0, null)
            .WithMdTag(lineTag);
            root.AddToken(ParseLine(builder, tokenSeparators, root));
            root.WithEnd(builder.Length);
            yield return root.Build();
        }
    }

    public Token WrapToken(string token, int start, string? mdTag)
    {
        if (mdTag == null)
            return new(token, start, null);

        return settings.TryGetSetting(mdTag!, out var setting) ? (new(token, start, setting)) : (new(token, start, null));
    }

    internal TagSetting? GetSetting(string? mdTag)
    {
        if (mdTag != null && settings.TryGetSetting(mdTag!, out var setting))
            return setting;
        return null;
    }

    private TokenBuilder ParseLine(StringBuilder line, string[] separators, TokenBuilder? root)
    {
        return ParseToken(line, 0, null, separators, root);
    }

    private TokenBuilder ParseToken(StringBuilder line, int start, string? openingSeparator, string[] separators, TokenBuilder? root)
    {
        var openingSeparatorLength = openingSeparator?.Length ?? 0;
        var token = new TokenBuilder(this, line, start - openingSeparatorLength, root)
            .WithMdTag(openingSeparator);
        var lastKnownTokenEnd = start;

        void TryAddPlainWord(int wordEnd)
        {
            if (lastKnownTokenEnd != wordEnd)
            {
                var plainToken = CreateTokenBuilder(line, lastKnownTokenEnd, wordEnd, null, token);
                token!.AddToken(plainToken);
            }
        }

        for (var i = start; i < line.Length; i++)
        {
            if (TryGetCurrentSeparator(i, line.ToString(), separators, openingSeparator, out var currentSeparator))
            {
                if (IsEscaped(line.ToString(), i))
                {
                    line.Remove(i - 1, 1);

                    if (!IsEscaped(line.ToString(), i - 1))
                    {
                        i += currentSeparator.Length;
                        continue;
                    }
                    else
                    {
                        i -= 1;
                    }
                }

                if (openingSeparator != currentSeparator)
                {
                    TryAddPlainWord(i);

                    var childToken = ParseToken(line, i + currentSeparator.Length, currentSeparator, separators, token);
                    token.AddToken(childToken);
                    i = childToken.End - 1;
                    lastKnownTokenEnd = i + 1;
                }
                else
                {
                    token.WithEnd(i + currentSeparator.Length);
                    return token;
                }
            }
        }

        TryAddPlainWord(line.Length);

        token.WithEnd(line.Length);
        return token;
    }

    private TokenBuilder CreateTokenBuilder(StringBuilder source, int start, int end, string? mdTag, TokenBuilder? root)
    {
        return new TokenBuilder(this, source, start, root)
                  .WithMdTag(mdTag)
                  .WithEnd(end);
    }

    private bool TryGetCurrentSeparator(int i, string line, string[] separators, string? lastSeparator, out string currentSeparator)
    {
        IEnumerable<string> finalSeparators = separators;
        if (lastSeparator != null)
            finalSeparators = finalSeparators.Where(x => x != lastSeparator).Prepend(lastSeparator);
        foreach (var separator in finalSeparators)
        {
            if (i + separator.Length > line.Length)
                continue;
            var separatorEnd = i + separator.Length;
            if (line[i..separatorEnd] == separator)
            {
                currentSeparator = separator;
                return true;
            }
        }

        currentSeparator = null!;
        return false;
    }

    private static bool IsEscaped(string source, int position)
    {
        if (position < 1)
            return false;

        return source[position - 1] == '\\';
    }
}
