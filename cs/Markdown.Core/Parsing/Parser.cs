using Markdown.Core.Lexing;
using Markdown.Core.Parsing.Blocks;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing;

public class Parser : IParser
{
    private readonly List<Token> _allTokens = [];
    private readonly InlineValidator _inlineValidator = new();
    private InlineParser _inlineParser;
    private ParserState _state;
    private readonly List<IBlockParser> _blockParsers;

    public Parser()
    {
        _blockParsers =
        [
            new HeadingBlockParser(),
            new ParagraphBlockParser()
        ];
    }
    
    public DocumentNode Parse(IEnumerable<Token> tokens)
    {
        _allTokens.Clear();
        _allTokens.AddRange(tokens);
        
        _state = new ParserState(_allTokens);
        _inlineParser = new InlineParser(
            _allTokens,
            _state.MoveNext,
            () => _state.CurrentToken,
            () => _state.CurrentIndex,
            _state.IsEndOfLine,
            _inlineValidator);
        _state.Start();

        var document = new DocumentNode();
        
        while (_state.CurrentToken.Kind != TokenKind.Eof)
        {
            _state.SkipEmptyLines();
            if (_state.CurrentToken.Kind == TokenKind.Eof)
                break;

            var block = ParseBlock();
            if (block != null) document.Children.Add(block);
        }
        return document;
    }

    private BlockNode? ParseBlock()
    {
        return (from blockParser in _blockParsers where blockParser
            .CanParse(_state) select blockParser
            .Parse(_state, _inlineParser))
            .FirstOrDefault();
    }
}
