using Markdown.Parsers;
using Markdown.Tokens.Tags;

namespace Markdown.Tokens;

public class Tokenizer
{
    private readonly Dictionary<char, List<IParser>> _parsers = [];
    private readonly List<Token> _tokens = [];
    public Tokenizer(bool allowBold)
    {
        var underscoreParsers = new List<IParser>();
        
        if (allowBold)
            underscoreParsers.Add(new BoldParser());
        underscoreParsers.Add(new ItalicsParser());
        
        _parsers.Add('_', underscoreParsers);
        _parsers.Add('#', [new HeadingParser()]);
        _parsers.Add('\\', [new EscapeParser()]);
        _parsers.Add('*', [new MarkedListParser('*')]);
        _parsers.Add('-', [new MarkedListParser('-')]);
        _parsers.Add('+', [new MarkedListParser('+')]);
    }
    
    public List<Token> Tokenize(string markdown)
    {
        var lines = markdown.Split("\n");
        for (var i = 0; i < lines.Length; i++)
            _tokens.AddRange(ParseLine(lines[i], i, lines.Length));

        return _tokens;
    }

    private List<Token> ParseLine(string line, int lineIndex, int linesLength)
    {
        var result = new List<Token>();
        
        for (var i = 0; i < line.Length; i++)
        {
            var matched = false;
            if (_parsers.TryGetValue(line[i], out var parsers))
            {
                foreach (var parser in parsers)
                {
                    if (!parser.CanParse(line[i], line, i)) 
                        continue;
                    var token = parser.Parse(line, i);
                    _tokens.Add(token);
                    i = token.EndIndex;
                    matched = true;
                    break;
                }
            }

            if (!matched)
                _tokens.Add(new Token(new TextTag(), line[i].ToString(), i));
        }

        if (linesLength > 1 && lineIndex < linesLength - 1)
            _tokens.Add(new Token(new TextTag(), "\n", 0));
        return result;
    }
}