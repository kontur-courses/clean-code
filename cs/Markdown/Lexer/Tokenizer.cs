using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Lexer
{
    internal class Tokenizer
    {
        private const char EscapeCharacter = '\\';

        private readonly IEnumerable<Lexeme> lexemes;

        internal Tokenizer() : this(LexemeDefinitions.Lexemes)
        {
        }

        internal Tokenizer(IEnumerable<Lexeme> lexemes)
        {
            this.lexemes = lexemes;
        }

        public IEnumerable<Token> GetTokens(string input)
        {
            var tokens = new List<Token>();
            var tags = new Stack<Token>();
            var position = 0;
            while (InBounds(input, position))
            {
                Token token;
                if (IsStartOfLexeme(input[position]))
                {
                    token = ReadTag(input, position, tags);
                    if (token != null)
                    {
                        position = AddToken(tokens, token, position);
                        continue;
                    }
                }

                token = ReadText(input, position);
                position = AddToken(tokens, token, position);
            }

            return tokens;
        }

        private Token ReadTag(string input, int position, Stack<Token> tags)
        {
            var lexeme = FindLexeme(input, position);
            if (lexeme == null)
                return null;
            var type = GetTagType(lexeme, tags);
            var token = new Token(position, lexeme.Representation, type, lexeme);
            if (!CheckTagConditions(input, token))
                return ReadText(input, position, token.Value);
            UpdateTagStack(tags, token);
            return token;
        }

        private Token ReadText(string input, int position, string readPart = null)
        {
            var builder = new StringBuilder(readPart);
            var escaped = false;
            var escapedCount = 0;
            for (var i = position + builder.Length; InBounds(input, i); i++)
            {
                var symbol = input[i];
                if (!escaped && IsStartOfLexeme(symbol))
                    break;
                if (escaped)
                {
                    builder.Append(symbol);
                    escaped = false;
                }
                else if (symbol == EscapeCharacter)
                {
                    escaped = true;
                    escapedCount++;
                }
                else
                    builder.Append(symbol);
            }

            var value = builder.ToString();
            return new Token(position, value.Length + escapedCount, value, TokenType.Text);
        }

        private bool IsStartOfLexeme(char symbol)
        {
            return lexemes.Any(l => l.Representation[0] == symbol);
        }

        private Lexeme FindLexeme(string input, int position)
        {
            foreach (var lexeme in lexemes)
            {
                var representation = lexeme.Representation;
                if (InBounds(input, position + representation.Length - 1) &&
                    input.Substring(position, representation.Length) == representation)
                    return lexeme;
            }

            return null;
        }

        private static TokenType GetTagType(Lexeme lexeme, IEnumerable<Token> tags)
        {
            var openingTag = tags.FirstOrDefault(tag => tag.Lexeme == lexeme && tag.Type == TokenType.OpeningTag);
            return openingTag == null ? TokenType.OpeningTag : TokenType.ClosingTag;
        }

        private static void UpdateTagStack(Stack<Token> tags, Token tag)
        {
            switch (tag.Type)
            {
                case TokenType.OpeningTag:
                    tags.Push(tag);
                    break;
                case TokenType.ClosingTag:
                    tags.Pop();
                    break;
            }
        }

        private static bool CheckTagConditions(string input, Token tag)
        {
            return !CheckWhitespaceAroundTag(input, tag) && !CheckDigitAroundTag(input, tag);
        }

        private static bool CheckDigitAroundTag(string input, Token tag)
        {
            var prev = tag.Position - 1;
            var next = tag.Position + tag.Length;
            return InBounds(input, prev) && InBounds(input, next) &&
                   (char.IsLetterOrDigit(input[prev]) && char.IsDigit(input[next]) ||
                    char.IsLetterOrDigit(input[next]) && char.IsDigit(input[prev]));
        }

        private static bool CheckWhitespaceAroundTag(string input, Token tag)
        {
            var index = -1;
            switch (tag.Type)
            {
                case TokenType.OpeningTag:
                    index = tag.Position + tag.Length;
                    break;
                case TokenType.ClosingTag:
                    index = tag.Position - 1;
                    break;
            }

            return InBounds(input, index) && char.IsWhiteSpace(input[index]);
        }

        private static int AddToken(IList<Token> tokens, Token token, int position)
        {
            if (tokens.Count > 0 && token.Type == TokenType.Text && tokens[tokens.Count - 1].Type == TokenType.Text)
                tokens[tokens.Count - 1] = MergeTokens(tokens[tokens.Count - 1], token);
            else
                tokens.Add(token);
            return position + token.Length;
        }

        private static Token MergeTokens(Token first, Token second)
        {
            return new Token(first.Position, first.Length + second.Length,
                first.Value + second.Value,first.Type | second.Type);
        }

        private static bool InBounds(string input, int index) => 0 <= index && index < input.Length;
    }
}