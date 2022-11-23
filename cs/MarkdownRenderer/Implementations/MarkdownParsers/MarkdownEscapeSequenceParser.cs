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

    public bool TryGetEscapingSequenceToken(string content, int escapeCharacterPos, out ContentToken? token)
    {
        token = default;
        if (content[escapeCharacterPos] != EscapeCharacter)
            return false;

        var escapingSequence = _escapingSequences
            .Where(sequence => escapeCharacterPos + sequence.Length < content.Length)
            .Where(
                sequence => sequence
                    .Where((c, i) => content[escapeCharacterPos + 1 + i] != c)
                    .Any() is false
            )
            .DefaultIfEmpty()
            .MinBy(sequence => sequence?.Length ?? 0);

        if (escapingSequence is null)
            return false;

        token = new ContentToken(escapeCharacterPos, escapeCharacterPos + escapingSequence.Length, 1, 0);
        return true;
    }

    public IElement ParseElement(string content, ContentToken token)
    {
        if (content[token.Start] != EscapeCharacter)
            throw new ArgumentException("Incorrect escaping sequence. Unable to parse!");

        var rawContent = content.Substring(token);
        if (!_escapingSequences.Contains(rawContent))
            throw new ArgumentException("Incorrect escaping sequence. Unable to parse!");

        return new EscapeSequenceElement();
    }
}