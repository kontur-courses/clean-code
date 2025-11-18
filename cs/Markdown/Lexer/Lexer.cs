using System.Text;

namespace Markdown.Tokenizing;

public class Lexer
{
    private static readonly Dictionary<char, Func<CharIndexer, Token?>> SingleCharHandlers = new()
    {
        ['\n'] = c => CreateAndMove(TokenType.EndOfLine, "\n", c),
        [' '] = c => CreateAndMove(TokenType.Whitespace, " ", c),
        ['\t'] = c => CreateAndMove(TokenType.Whitespace, "\t", c),
        ['\\'] = c => CreateAndMove(TokenType.Escape, "\\", c),
        ['#'] = c => CreateAndMove(TokenType.Hash, "#", c),
        ['['] = c => CreateAndMove(TokenType.OpenBracket, "[", c),
        [']'] = c => CreateAndMove(TokenType.CloseBracket, "]", c),
        ['('] = c => CreateAndMove(TokenType.OpenParen, "(", c),
        [')'] = c => CreateAndMove(TokenType.CloseParen, ")", c),
        ['<'] = c => CreateAndMove(TokenType.AutoLinkOpen, "<", c),
        ['>'] = c => CreateAndMove(TokenType.AutoLinkClose, ">", c),
        ['_'] = HandleUnderscore
    };

    public List<Token> Tokenize(string text)
    {
        var cursor = new CharIndexer(text);
        var tokens = new List<Token>();

        while (!cursor.End)
        {
            if (SingleCharHandlers.TryGetValue(cursor.Current, out var handler))
            {
                var token = handler(cursor);
                if (token != null)
                    tokens.Add(token);
            }
            else
            {
                ReadText(tokens, cursor);
            }
        }

        tokens.Add(new Token(TokenType.EndOfFile, ""));
        return tokens;
    }

    private static Token? HandleUnderscore(CharIndexer cursor)
    {
        if (cursor.MatchNext('_'))
        {
            var token = new Token(TokenType.DoubleUnderscore, "__");
            cursor.MoveNext();
            cursor.MoveNext();
            return token;
        }
        else
        {
            var token = new Token(TokenType.Underscore, "_");
            cursor.MoveNext();
            return token;
        }
    }

    private static Token CreateAndMove(TokenType type, string value, CharIndexer cursor)
    {
        var token = new Token(type, value);
        cursor.MoveNext();
        return token;
    }

    private static void ReadText(List<Token> tokens, CharIndexer indexer)
    {
        var sb = new StringBuilder();

        while (!indexer.End && IsTextChar(indexer.Current))
        {
            sb.Append(indexer.Current);
            indexer.MoveNext();
        }

        if (sb.Length > 0)
            tokens.Add(new Token(TokenType.Text, sb.ToString()));
    }

    private static bool IsTextChar(char c)
    {
        return !SingleCharHandlers.ContainsKey(c) && 
               c != '\n' && 
               !char.IsWhiteSpace(c);
    }
}