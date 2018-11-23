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
        private readonly Token rootToken = new Token(0, new PTagInfo());
        private Window Window => GetWindow();


        public TagReader(string source, params TagInfo[] tags)
        {
            Source = source;
            Tags = tags;
        }

        public string Evaluate()
        {
            while (Position < Source.Length)
            {
                if (TrySkipEscapeSymbol()) continue;
                if (TryCloseToken()) continue;
                if (TryOpenToken()) continue;
                
                AddCharacterToTokens(Window[0]);

                Position++;
            }
            return rootToken.Value.ToString();
        }

        public void SkipAndAdd(int amount)
        {
            for (var startPosition = Position; Position < startPosition + amount; Position++)
            {
                AddCharacterToTokens(Source[Position]);
            }
        }

        public void Skip(int amount)
        {
            for (var startPosition = Position; Position < startPosition + amount; Position++) ;
        }

        private void AddCharacterToTokens(char c)
        {
            foreach (var token in GetTokens())
            {
                token.AddCharacter(c);
            }
        }

        private Window GetWindow() => new Window
        {
            [2] = Position >= Source.Length - 2 ? (char) 0 : Source[Position + 2],
            [1] = Position >= Source.Length - 1 ? (char) 0 : Source[Position + 1],
            [0] = Source[Position],
            [-1] = Position == 0 ? (char) 0 : Source[Position - 1]
        };

        private bool TryAddNewToken(TagInfo tag)
        {
            var currentToken = rootToken;
            while (currentToken.Child != null)
            {
                if (currentToken.Tag == tag)
                    return false;
                currentToken = currentToken.Child;
            }

            if (currentToken.Tag is EmTagInfo && tag is StrongTagInfo)
            {
                SkipAndAdd(tag.TagLength);
                return false;
            }

            currentToken.Child = tag.GetNewToken(Position);
            currentToken.Child.Parent = currentToken;

            return true;
        }

        private bool TryOpenToken()
        {
            foreach (var tag in Tags)
            {
                if (!tag.StartCondition(Window)) continue;
                if (!TryAddNewToken(tag)) continue;
                tag.OnTagStart(this);
                return true;
            }

            return false;
        }

        private bool TryCloseToken()
        {
            foreach (var token in GetTokens().Reverse())
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

        private IEnumerable<Token> GetTokens()
        {
            var currentToken = rootToken;
            do
            {
                yield return currentToken;
                currentToken = currentToken.Child;
            } while (currentToken != null);
        }
    }
}
