using System.Text;
using Markdown.Dto;
using Markdown.TokensUtils.Abstractions;

namespace Markdown.TokensUtils.Implementations;

public class Tokenizer : ITokenizer
{
    private static readonly HashSet<char> SpecialSymbols = ['\\', '_', '#', ')', '['];

    public IEnumerable<Token> Tokenize(string? line)
    {
        ArgumentNullException.ThrowIfNull(line);

        var sb = new StringBuilder();
        var i = 0;

        while (i < line.Length)
        {
            var c = line[i];

            if (SpecialSymbols.Contains(c) && sb.Length > 0)
            {
                yield return new Token(sb.ToString(), TokenType.Text, false, false);
                sb.Clear();
            }

            var token = MapSpecialSymbol.Specialize(c, line, i);
            if (token != null)
            {
                yield return token;

                if (token.Type is TokenType.Header or TokenType.Strong)
                {
                    i++;
                }
            }
            else
            {
                sb.Append(c);
            }

            i++;
        }

        if (sb.Length > 0)
        {
            yield return new Token(sb.ToString(), TokenType.Text, false, false);
        }

        yield return new Token(string.Empty, TokenType.End, false, false);
    }
}