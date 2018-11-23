using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TagReader
    {
        public int Position { get; private set; }
        public string Source { get; }
        public TagInfo[] Tags { get; }
        private TokenList TokenList { get; }
        private Window Window { get; }


        public TagReader(string source, params TagInfo[] tags)
        {
            Source = source;
            Tags = tags;
            TokenList = new TokenList(new PTagInfo().GetNewToken(0));
            Window = new Window(Source, 0);
        }

        public string Evaluate()
        {
            while (Position < Source.Length)
            {
                Window.Position = Position;
                if (TrySkipEscapeSymbol()) continue;
                if (TryCloseTag()) continue;
                if (TryOpenTag()) continue;
                
                AddCharacterToTokens(Window[0]);

                Position++;
            }
            return TokenList.RootValue;
        }

        public void SkipAndAdd(int amount)
        {
            for (var startPosition = Position; Position < startPosition + amount; Position++)
            {
                AddCharacterToTokens(Source[Position]);
            }
        }

        public void Skip(int amount) => Position += amount;

        private void AddCharacterToTokens(char c)
        {
            foreach (var token in TokenList)
            {
                token.AddCharacter(c);
            }
        }

        private bool TryOpenTag()
        {
            foreach (var tag in Tags)
            {
                if (!tag.StartCondition(Window)) continue;
                if (!TokenList.TryAddNewToken(tag, Position)) continue;
                tag.OnTagStart(this);
                return true;
            }

            return false;
        }

        private bool TryCloseTag()
        {
            foreach (var token in TokenList.Reverse())
            {
                if (!token.Tag.EndCondition(Window)) continue;
                token.Tag.OnTagEnd(this);
                token.Close();
                return true;
            }

            return false;
        }

        private bool TrySkipEscapeSymbol()
        {
            if (Window[0] != '\\') return false;
            Skip(1);
            SkipAndAdd(1);
            return true;
        }
    }
}
