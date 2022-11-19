using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface IEscapeSequenceElementParser : IInlineElementParser
{
    public char EscapeCharacter { get; }
    public void SetEscapingSequences(IEnumerable<string> escapingSequences);
    public bool TryGetEscapingSequenceToken(string content, int escapeCharacterPos, out Token? token);
}