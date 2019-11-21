using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Tokens;

namespace Markdown.Core.Normalizer
{
    public class MdNormalizer
    {
        public List<Token> NormalizeTokens(List<Token> tokens, List<IgnoreInsideRule> ignoreRules)
        {
            var normalizedWithoutIgnoreRulesTokens = DoNormalizeIteration(tokens, new List<IgnoreInsideRule>());
            return DoNormalizeIteration(normalizedWithoutIgnoreRulesTokens, ignoreRules);
        }

        private List<Token> DoNormalizeIteration(List<Token> tokens, List<IgnoreInsideRule> ignoreRules)
        {
            var tagStack = new Stack<HTMLTagToken>();
            var inlineTokens = tokens
                .Where(token => token.TokenType == TokenType.HTMLTag)
                .Cast<HTMLTagToken>()
                .Where(tag => tag.TagType != HTMLTagType.Header);
            foreach (var tagToken in inlineTokens)
            {
                if (!TryPutTokenIntoRightTagsSequence(tagStack, tagToken, ignoreRules))
                {
                    tagToken.TokenType = TokenType.Text;
                    ChangeTokenTypeToTextToAllTokensInStack(tagStack);
                }
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
            Stack<HTMLTagToken> stack, HTMLTagToken token, List<IgnoreInsideRule> ignoreInsideRules)
        {
            var previousValueIsDifferent = stack.Count == 0 || stack.Peek().Value != token.Value;
            foreach (var ignoreInsideRule in ignoreInsideRules)
            {
                if (IsNeedIgnoreInside(stack, token, ignoreInsideRule))
                {
                    token.TokenType = TokenType.Text;
                    return true;
                }
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

        private static bool IsNeedIgnoreInside(Stack<HTMLTagToken> stack, HTMLTagToken token,
            IgnoreInsideRule ignoreInsideRule)
        {
            return token.Value == ignoreInsideRule.IgnoredInsideTag.MdTag &&
                   stack.Count != 0 &&
                   ignoreInsideRule.OuterTags.Any(tagInfo => stack.Peek().Value == tagInfo.MdTag);
        }
    }
}