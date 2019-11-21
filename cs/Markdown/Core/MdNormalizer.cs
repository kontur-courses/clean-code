using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Tokens;

namespace Markdown.Core
{
    public class MdNormalizer
    {    
        public List<IToken> NormalizeTokens(List<IToken> tokens, string ignoredInsideMdTag)
        {
            var tagStack = new Stack<HTMLTagToken>();
            var inlineTokens = tokens
                .Where(token => token.TokenType == TokenType.HTMLTag)
                .Cast<HTMLTagToken>()
                .Where(tag => tag.TagType != HTMLTagType.Header);
            foreach (var tagToken in inlineTokens)
            {
                if (!TryPutTokenIntoRightTagsSequence(tagStack, tagToken, ignoredInsideMdTag))
                {
                    tagToken.TokenType = TokenType.Text;
                    ChangeTokenTypeToTextToAllTokensInStack(tagStack);
                };
            }
            ChangeTokenTypeToTextToAllTokensInStack(tagStack);
            return tokens;
        }

        private void ChangeTokenTypeToTextToAllTokensInStack(Stack<HTMLTagToken> tagStack)
        {
            while (tagStack.Count != 0)
            {
                tagStack.Pop().TokenType = TokenType.Text;
            }
        }
        
        private bool TryPutTokenIntoRightTagsSequence(
            Stack<HTMLTagToken> stack, HTMLTagToken token, string ignoredInsideMdTag)
        {
            var previousValueIsDifferent = stack.Count == 0 || stack.Peek().Value != token.Value;
            if (token.Value == ignoredInsideMdTag && stack.Count != 0 && previousValueIsDifferent)
            {
                token.TokenType = TokenType.Text;
                return true;
            }
                
            if (token.TagType == HTMLTagType.Opening)
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