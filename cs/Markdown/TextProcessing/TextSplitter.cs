using System.Collections.Generic;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TextSplitter
    {
        public string Content { get; set; }
        public TokenReader Reader { get; set; }
        private TypeToken station;

        public TextSplitter(string content)
        {
            Content = content;
            Reader = new TokenReader(content);
            station = TypeToken.Simple;
        }

        public List<IToken> SplitToTokens()
        {
            var tokens = new List<IToken>();
            var position = 0;
            while (position < Content.Length)
            {
                IToken token = new SimpleToken();
                switch (station)
                {
                    case TypeToken.Simple:
                        break;
                    case TypeToken.Em:
                        token = new EmToken();
                        break;
                    case TypeToken.Strong:
                        token = new StrongToken();
                        break;
                }
                token = Reader.ReadToken(token.IsStopChar, token);
                position = Reader.Position;
                station = token.GetNextTypeToken(Content, position - 1);
                if (station == TypeToken.Strong)
                    Reader.Position++;
                if (token.Length != 0)
                    tokens.Add(token);
            }
            return tokens;
        }
    }
}