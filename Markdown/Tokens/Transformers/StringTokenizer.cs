using System.Text;
using Markdown.Tokens.Lexing;

namespace Markdown.Tokens.Transformers;

internal sealed class TokenBuilder(IReadOnlyList<ISymbolInterpreter> interpreters)
{
    public IReadOnlyList<Token> Build(IReadOnlyList<RawSegment> segments)
    {
        var tokens = new List<Token>(segments.Count);
        tokens.AddRange(segments.Select((_, i) => BuildToken(segments, i)));

        return TextTokenMerger.Merge(tokens);
    }

    private Token BuildToken(IReadOnlyList<RawSegment> segments, int index) => segments[index] switch
    {
        TextSegment text => new TextToken(text.Value),
        LineBreakSegment => new ParagraphBreakToken(),
        SymbolSegment symbol => InterpretSymbol(segments, index, symbol),
        _ => throw new ArgumentOutOfRangeException(nameof(segments), $"Unknown segment at {index}")
    };

    private Token InterpretSymbol(IReadOnlyList<RawSegment> segments, int index, SymbolSegment symbol)
    {
        var interpreter = interpreters.FirstOrDefault(x => x.CanHandle(symbol));
        return interpreter?.Interpret(segments, index) ?? new TextToken(symbol.Symbol);
    }
}

public static class TextTokenMerger
{
    public static IReadOnlyList<Token> Merge(IReadOnlyList<Token> tokens)
    {
        var result = new List<Token>(tokens.Count);
        var buffer = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token is TextToken text)
            {
                buffer.Append(text.Value);
                continue;
            }

            FlushBuffer();
            result.Add(token);
        }

        FlushBuffer();
        return result;

        void FlushBuffer()
        {
            if (buffer.Length == 0) return;
            result.Add(new TextToken(buffer.ToString()));
            buffer.Clear();
        }
    }
}

public static class StringTokenizer
{
    private static readonly TokenBuilder Builder = new([
        new HeaderInterpreter(),
        new UnderscoreInterpreter()
    ]);

    public static IReadOnlyList<Token> Tokenize(string text)
        => Builder.Build(Lexer.Lexize(text));
}