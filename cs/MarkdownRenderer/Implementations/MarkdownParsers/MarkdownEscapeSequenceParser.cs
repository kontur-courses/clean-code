using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Implementations.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownEscapeSequenceParser : IEscapeSequenceElementParser
{
    public Type ParsingElementType { get; } = typeof(EscapeSequenceElement);

    public char EscapeCharacter => '\\';
    private IReadOnlySet<string> _escapingSequences = new HashSet<string> {"\\"};

    public void SetEscapingSequences(IEnumerable<string> escapingSequences)
    {
        _escapingSequences = escapingSequences.Concat(new[] {"\\"}).ToHashSet();
    }

    public bool TryGetEscapingSequenceToken(string content, int escapeCharacterPos, out Token? token)
    {
        token = default;
        if (content[escapeCharacterPos] != EscapeCharacter)
            return false;
        var escapingSequence = _escapingSequences
            .Where(sequence => escapeCharacterPos + sequence.Length < content.Length)
            .Where(
                sequence => sequence
                    .Where((c, i) => content[escapeCharacterPos + 1 + i] != c)
                    .Any() == false
            )
            .DefaultIfEmpty()
            .MaxBy(sequence => sequence?.Length ?? 0);

        if (escapingSequence is null)
            return false;

        token = new Token(escapeCharacterPos, escapeCharacterPos + escapingSequence.Length);
        return true;
    }

    public IElement ParseElement(string content, Token token)
    {
        if (content[token.Start] != EscapeCharacter)
            throw new ArgumentException("Unable to parse!");
        var rawContent = content.Substring(token.Start + 1, token.Length - 1);
        if (!_escapingSequences.Contains(rawContent))
            throw new ArgumentException("Unable to parse!");
        return new EscapeSequenceElement(rawContent);
    }
}