using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Tokens;

namespace Markdown.Core
{
    public class MdNormalizer
    {    
        public List<IToken> NormalizeTokens(List<IToken> tokens)
        {
            var tagStack = new Stack<HTMLTagToken>();
            var tagTokens = tokens.Where(token => token.TokenType == TokenType.HTMLTag).Cast<HTMLTagToken>();
            foreach (var tagToken in tagTokens)
            {
                if (!TryPutTokenIntoRightTagsSequence(tagStack, tagToken))
                {
                    tagToken.TokenType = TokenType.Text;
                    ChangeTypeToAllTokensInStack(tagStack);
                };
            }
            ChangeTypeToAllTokensInStack(tagStack);
            return tokens;
        }

        private void ChangeTypeToAllTokensInStack(Stack<HTMLTagToken> tagStack)
        {
            while (tagStack.Count != 0)
            {
                tagStack.Pop().TokenType = TokenType.Text;
            }
        }
        
        private bool TryPutTokenIntoRightTagsSequence(Stack<HTMLTagToken> stack, HTMLTagToken token)
        {
            var previousValueIsDifferent = stack.Count == 0 || stack.Peek().Value != token.Value;
            if (token.IsOpen)
            {
                if (previousValueIsDifferent)
                    stack.Push(token);
                else
                    return false;
            }
            else
            {
                if (previousValueIsDifferent)
                    return false;   
                stack.Pop();
            }
            return true;
        }
    }
}