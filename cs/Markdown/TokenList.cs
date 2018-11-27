using System.Collections;
using System.Collections.Generic;

namespace Markdown
{
    public class TokenList : IEnumerable<Token>
    {
        private readonly Token rootToken;

        public TokenList(Token rootToken)
        {
            this.rootToken = rootToken;
        }

        public string GetValue()
        {
            rootToken.Child?.Abort();
            return rootToken.Value;
        }

        public bool TryAddNewToken(ITagInfo tag, int position)
        {
            var currentToken = rootToken;
            while (currentToken.Child != null)
            {
                if (currentToken.Tag == tag)
                    return false;
                currentToken = currentToken.Child;
            }

            currentToken.SetChild(tag.GetNewToken(position));
            currentToken.Child.SetParent(currentToken);

            return true;
        }

        public void AddCharacter(char c)
        {
            rootToken.AddCharacter(c);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            var currentToken = rootToken;
            do
            {
                yield return currentToken;
                currentToken = currentToken.Child;
            } while (currentToken != null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
