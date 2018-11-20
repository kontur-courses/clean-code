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

        public List<Token> SplitToTokens()
        {
            var tokens = new List<Token>();
            var position = 0;
            while (position < Content.Length)
            {
                ITokenHandler tokenHandler = new SimpleTokenHandler();
                switch (station)
                {
                    case TypeToken.Simple:
                        break;
                    case TypeToken.Em:
                        tokenHandler = new EmTokenHandler();
                        break;
                    case TypeToken.Strong:
                        tokenHandler = new StrongTokenHandler();
                        break;
                }
                var token = Reader.ReadToken(tokenHandler.IsStopChar, tokenHandler);
                position = Reader.Position;
                station = tokenHandler.GetNextTypeToken(Content, position - 1);
                if (station == TypeToken.Strong)
                    Reader.Position++;
                tokens.Add(token);
            }
            return tokens;
        }
    }
}