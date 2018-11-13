using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            station = TypeToken.SimpleText;
        }

        public List<Token> SplitToTokens()
        {
            var tokens = new List<Token>();
            var position = 0;
            while (position < Content.Length)
            {
                var token = new Token(0,0,"", TypeToken.SimpleText);
                switch (station)
                {
                    case TypeToken.SimpleText:
                        token = Reader.ReadUntil(x => x == '_', TypeToken.SimpleText);
                        station = TypeToken.Em;
                        break;
                    case TypeToken.Em:
                        token = Reader.ReadUntil(x => x == '_', TypeToken.Em);
                        station = token.Length == 0? TypeToken.Strong:TypeToken.SimpleText;
                        break;
                    case TypeToken.Strong:
                        token = Reader.ReadUntil(x => x == '_', TypeToken.Strong);
                        station = token.Length == 0 ? TypeToken.SimpleText : TypeToken.Strong;
                        break;
                }

                position = Reader.Position;
                if (token.Length != 0)
                tokens.Add(token);
            }
            return tokens;
        }
    }
}