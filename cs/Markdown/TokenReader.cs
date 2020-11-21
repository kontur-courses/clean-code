using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenReader
    {
        public readonly string Text;
        public int CurrentPosition = 0;

        public TokenReader(string text)
        {
            Text = text;
        }

        private readonly List<TokenType> tokenTypes = new List<TokenType>();
        private readonly Stack<ReaderState> states = new Stack<ReaderState>();

        public Token ReadToken(bool notRawText = false)
        {
            foreach (var tokenType in tokenTypes)
            {
                Token token;
                if (tokenType is BasicTokenType basicTokenType) token = ReadTokenType(basicTokenType);
                else if (tokenType is CustomTokenType customTokenType) token = ReadTokenType(customTokenType);
                else token = null;
                if (token != null) return token;
            }

            return notRawText ? null : ReadRawTextUntil(() => HasToken(true) || GetClosingState(0) != null);
        }

        public TToken ReadTokenType<TToken>(BasicTokenType<TToken> type) where TToken : BasicToken, new()
            => (TToken) ReadTokenType((BasicTokenType) type);

        private Token ReadTokenType(CustomTokenType type)
        {
            var state = new ReaderState(type, CurrentPosition);
            states.Push(state);
            var token = type.ReadFunc(this);

            if (states.Peek() == state) states.Pop();
            if (token == null) CurrentPosition = state.Position;
            return token;
        }

        private Token ReadTokenType(BasicTokenType type)
        {
            if (type.StartWithNewLine && !IsAtNewLine(-1)) return null;
            if (!TryGet(type.Start)) return null;

            var token = type.CreateInstance(CurrentPosition, type.Start.Length);
            if (IsAtSpace(type.Start.Length)) return null;

            var state = new ReaderState(type, CurrentPosition, token);
            states.Push(state);
            var allowSpaces = IsAtSpace(-1);

            var position = CurrentPosition;
            CurrentPosition += type.Start.Length;

            var result = ReadToken(token, allowSpaces);
            if (states.Peek() == state) states.Pop();

            result ??= new RawTextToken(position, CurrentPosition - position);

            return result;
        }

        public BasicTokenType<TToken> AddBasicTokenType<TToken>(string start, string end)
            where TToken : BasicToken, new()
        {
            var type = new BasicTokenType<TToken>(start, end);
            tokenTypes.Add(type);
            return type;
        }

        public CustomTokenType AddCustomTokenType<TToken>(Func<TokenReader, TToken> readFunc) where TToken : Token
        {
            var type = new CustomTokenType(readFunc);
            tokenTypes.Add(type);
            return type;
        }

        public IEnumerable<TokenType> GetTokenTypes() => tokenTypes;

        private Token ReadToken(BasicToken token, bool allowSpaces)
        {
            var type = (BasicTokenType) states.Peek().TokenType;

            while (!(!IsAtSpace(-1) && TryGet(type.End, type.EndWithNewLine)))
            {
                if (CurrentPosition >= Text.Length) return null;
                if (!allowSpaces && IsAtSpace()) return null;

                var failState = GetClosingState();
                if (failState != null)
                {
                    while (states.Peek() != failState) states.Pop();
                    CurrentPosition += ((BasicTokenType) failState.TokenType).End.Length;

                    return null;
                }

                var subtoken = ReadToken();
                if (subtoken == null) return null;
                token.AddSubtoken(subtoken);
            }

            var failType = tokenTypes.FirstOrDefault(t => t is BasicTokenType b
                                                          && TryGet(b.End, b.EndWithNewLine));
            if (failType != null && states.All(s => s.TokenType != failType))
            {
                CurrentPosition += ((BasicTokenType) failType).End.Length;
                return null;
            }

            CurrentPosition += type.End.Length;
            token.Length += type.End.Length;
            if (states.Skip(1).Select(s => s.TokenType).Any(type.DisallowedTokenTypes.Contains)) return null;
            if (token.Length == type.Start.Length + type.End.Length) return null;

            return token;
        }

        public RawTextToken ReadRawTextUntil(Func<bool> stopWhen)
        {
            if (CurrentPosition >= Text.Length) return null;
            var result = new RawTextToken(CurrentPosition);
            while (!stopWhen() && CurrentPosition < Text.Length)
            {
                result.Length++;
                CurrentPosition++;
            }

            return result;
        }

        public bool HasToken(bool notRawText)
        {
            var state = new ReaderState(null, CurrentPosition, null);
            states.Push(state);
            var ok = ReadToken(notRawText) != null;
            states.Pop();
            CurrentPosition = state.Position;
            return ok;
        }

        private ReaderState GetClosingState(int skipStages = 1)
        {
            var state = states.Skip(skipStages)
                .TakeWhile(s => s.TokenType is BasicTokenType)
                .FirstOrDefault(s => s.TokenType is BasicTokenType t && TryGet(t.End, t.EndWithNewLine));
            var type = (BasicTokenType) state?.TokenType;
            var shouldEndWithNewLine = type?.EndWithNewLine ?? false;
            var endWithNewLine = shouldEndWithNewLine && IsAtNewLine(type.End.Length);
            if (IsAtSpace() && !endWithNewLine) state = null;
            return state;
        }

        public IEnumerable<Token> ReadAll()
        {
            for (var token = ReadToken(); token != null; token = ReadToken()) yield return token;
        }

        public bool TryGet(string text, bool endWithNewLine = false, bool endWithSpace = false)
            => GetNextChars(text.Length) == text
               && (!endWithSpace || IsAtSpace(text.Length))
               && (!endWithNewLine || IsAtNewLine());

        public bool IsAtSpace(int offset = 0) => IsAtNewLine(offset) || Text[CurrentPosition + offset] == ' ';

        public bool IsAtNewLine(int offset = 0)
        {
            if (CurrentPosition + offset < -1 || CurrentPosition + offset > Text.Length) return false;
            if (CurrentPosition + offset == -1 || CurrentPosition + offset == Text.Length) return true;
            return Text[CurrentPosition + offset] == '\n';
        }

        public string GetNextChars(int count)
        {
            if (count > Text.Length - CurrentPosition) count = Text.Length - CurrentPosition;
            return Text.Substring(CurrentPosition, count);
        }

        private class ReaderState
        {
            public readonly TokenType TokenType;
            public readonly int Position;

            public ReaderState(TokenType tokenType, int startIndex, Token token = null)
            {
                TokenType = tokenType;
                Position = startIndex;
            }
        }
    }
}