using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface IEscapeSequenceElementParser : IElementParser
{
    public char EscapeCharacter { get; }
    public void SetEscapingSequences(IEnumerable<string> escapingSequences);
    public bool TryGetEscapingSequenceToken(string content, int escapeCharacterPos, out ContentToken? token);
    IElement ParseElement(string content, ContentToken token);
}