using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenList : IEnumerable<Token>
    {
        private readonly Token rootToken;
        private Token lastToken;

        public TokenList(Token rootToken)
        {
            this.rootToken = rootToken;
            lastToken = rootToken;
        }

        public string GetValue()
        {
            lastToken.Abort(rootToken);
            return rootToken.Value;
        }

        public bool TryCloseTag(CollectionView<char> collectionView, out ITagInfo tag)
        {
            tag = null;
            foreach (var token in this)
            {
                if (!token.Tag.EndCondition(collectionView)) continue;
                lastToken.Abort(token);
                lastToken = token.Parent;
                tag = token.Tag;
                return true;
            }

            return false;
        }

        public bool TryOpenTag(ITagInfo tag, int position)
        {
            if (this.Any(token => token.Tag == tag))
                return false;
            var newToken = tag.GetNewToken(position);
            newToken.SetParent(lastToken);
            lastToken = newToken;

            return true;
        }

        public void AddCharacter(char c)
        {
            lastToken.AddCharacter(c);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            var currentToken = lastToken;
            do
            {
                yield return currentToken;
                currentToken = currentToken.Parent;
            } while (currentToken != null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
