using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenList : IEnumerable<Token>
    {
        private readonly Token rootToken;
        public string RootValue => rootToken.Value.ToString();

        public TokenList(Token rootToken)
        {
            this.rootToken = rootToken;
        }

        public bool TryAddNewToken(TagInfo tag, int position)
        {
            var currentToken = rootToken;
            while (currentToken.Child != null)
            {
                if (currentToken.Tag == tag)
                    return false;
                currentToken = currentToken.Child;
            }

            currentToken.Child = tag.GetNewToken(position);
            currentToken.Child.Parent = currentToken;

            return true;
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
