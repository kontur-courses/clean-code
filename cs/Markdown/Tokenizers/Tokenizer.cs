using System.Text;

namespace Markdown.Parsers
{
    public class Tokenizer : ITokenizer
    {
        public static readonly (string symbols, TokenType type)[] Translations;
        public static readonly Dictionary<TokenType, string> TypeToSymbols;

        static Tokenizer()
        {
            var _translations = new (string symbols, TokenType type)[]
            {
                ("_", TokenType.Italic),
                ("__", TokenType.Bold),
                ("# ", TokenType.Header),
                ("\\", TokenType.Escape),
                ("* ", TokenType.BulletList),
            };

            TypeToSymbols = _translations.ToDictionary(x => x.type, x=> x.symbols);
            

            Translations = _translations.OrderByDescending(x => x).ToArray();
        }

        public (Token[] tokens, string text) Tokenize(string text)
        {
            var rawTokens = TokenizeRaw(text);
            return ApplyEscapes(rawTokens, text);
        }

        private Token[] TokenizeRaw(string text)
        {
            var tokens = new List<Token>();
            for (int i = 0; i < text.Length; i++)
            {
                var translation = Translations.FirstOrDefault(x => text[i..]
                                              .StartsWith(x.symbols));

                if (translation.symbols != null)
                {
                    tokens.Add(new Token() { Index = i, Type = translation.type });
                    i += translation.symbols.Length - 1;
                }
            }

            return tokens.ToArray();
        }

        private (Token[] tokens, string text) ApplyEscapes(Token[] tokens, string text)
        {
            if (tokens.Length == 0)
                return (tokens, text);

            var sb = new StringBuilder();
            var prev = 0;

            var newTokens = new List<Token>();
            var curOffset = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                sb.Append(text[prev..tokens[i].Index]);

                if (i != tokens.Length - 1 &&
                    tokens[i].Type == TokenType.Escape &&
                    tokens[i + 1].Index == tokens[i].Index + tokens[i].Offset)
                {
                    curOffset += tokens[i].Offset;
                    i++;
                    prev = tokens[i].Index;
                }
                else
                {
                    prev = tokens[i].Index;
                    tokens[i].Index -= curOffset;
                    newTokens.Add(tokens[i]);
                }
            }
            sb.Append(text[prev..]);

            return (newTokens.ToArray(), sb.ToString());
        }
    }
}
