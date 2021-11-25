using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Segmenter
    {
        private readonly IEnumerable<TokenInfo> tokens;
        private readonly TagRules rules;

        private int newLineStartIndex;
        private readonly Dictionary<int, List<TokenSegment>> allowWaitTokens = new();
        private readonly List<TokenSegment> allowTokens = new();
        private readonly HashSet<TokenInfo> currentOpenedTag = new();
        private readonly HashSet<TokenInfo> ignoredTokens = new();
        private readonly HashSet<TokenInfo> provisionalContainRuleIgnoredTokens = new();
        private readonly Stack<TokenInfo> tokenStack = new();

        public Segmenter(IEnumerable<TokenInfo> tokens, TagRules rules)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(tokens), tokens),
                new ExceptionCheckObject(nameof(rules), rules));
            
            this.tokens = tokens;
            this.rules = rules;
        }

        private void GoToNextLine()
        {
            TryAllowTokens();
            
            allowWaitTokens.Clear();
            currentOpenedTag.Clear();
            ignoredTokens.Clear();
            provisionalContainRuleIgnoredTokens.Clear();
            tokenStack.Clear();
        }

        private bool IsThisTokenOpened(TokenInfo tokenInfo)
        {
            if (currentOpenedTag.RemoveWhere(x => x.Token == tokenInfo.Token) != 0) return false;
            tokenStack.Push(tokenInfo);
            if (tokenInfo.OpenValid) currentOpenedTag.Add(tokenInfo);
            return true;
        }

        private void SearchOpenTokenDueToIntersecting(TokenInfo lastOpenedToken, TokenInfo closeTokenToSearch)
        {
            var openedToken = lastOpenedToken;
            var tempStack = new Stack<TokenInfo>();
            var allowPosition = 0;
            
            while (openedToken.Token != closeTokenToSearch.Token)
            {
                allowPosition = openedToken.Position;
                ignoredTokens.Add(openedToken);
                tempStack.Push(openedToken);
                openedToken = tokenStack.Pop();
            }
            
            tempStack.Pop();
            while (tempStack.Any()) tokenStack.Push(tempStack.Pop());
            if (!allowWaitTokens.ContainsKey(allowPosition)) allowWaitTokens[allowPosition] = new List<TokenSegment>();
            allowWaitTokens[allowPosition].Add(new TokenSegment(closeTokenToSearch.WithPosition(openedToken.Position), closeTokenToSearch));
        }

        private bool DoesMatchNestingRule(TokenSegment segment)
        {
            if (!tokenStack.Any()) return true;
            if (rules.CanBeNested(tokenStack.Peek().Tag, segment.GetBaseTag())) return true;
            var allowPosition = tokenStack.Peek().Position;
            if (!allowWaitTokens.ContainsKey(allowPosition)) allowWaitTokens[allowPosition] = new List<TokenSegment>();
            allowWaitTokens[allowPosition].Add(segment);
            return false;
        }

        private bool ValidateByContainRule(TokenInfo tokenInfo)
        {
            var token = provisionalContainRuleIgnoredTokens.FirstOrDefault(x => x.Token == tokenInfo.Token);
            return token is null || !tokenInfo.WordPartPlaced;
        }

        private void HandlePairToken(TokenInfo tokenInfo, out TokenSegment validSegment)
        {
            var (_, token, closeValid, _, _, _) = tokenInfo;
            validSegment = null;

            if (IsThisTokenOpened(tokenInfo) 
                || !closeValid 
                || ignoredTokens.RemoveWhere(x => x.Token == token) == 1
                || !tokenStack.Any() 
                || !ValidateByContainRule(tokenInfo)) return;

            var openedToken = tokenStack.Pop();
            if (openedToken.Token == token)
            {
                validSegment = new TokenSegment(tokenInfo.WithPosition(openedToken.Position), tokenInfo);
                if (!DoesMatchNestingRule(validSegment))
                    validSegment = null;
                return;
            }
            
            SearchOpenTokenDueToIntersecting(openedToken, tokenInfo);
        }

        private void HandleSingleToken(TokenInfo tokenInfo, out TokenSegment segment)
        {
            if (rules.IsInterruptTag(tokenInfo.Tag))
            {
                newLineStartIndex = tokenInfo.Position + tokenInfo.Token.Length;
                GoToNextLine();
            }
            segment = new TokenSegment(tokenInfo);

            foreach (var openedTokenInfo in currentOpenedTag)
            {
                if (!rules.CanContain(openedTokenInfo.Tag, segment.GetBaseTag()))
                {
                    if (openedTokenInfo.WordPartPlaced)
                        ignoredTokens.Add(openedTokenInfo);
                    else
                        provisionalContainRuleIgnoredTokens.Add(openedTokenInfo);
                }
            }

            if (!rules.DoesMatchInFrontRule(segment, segment.StartPosition - newLineStartIndex)) segment = null;
        }

        private void TryAllowTokens()
        {
            foreach (var value in allowWaitTokens.Where(y => currentOpenedTag.Any(x => x.Position == y.Key))
                                                 .Select(x => x.Value))
            {
                foreach (var segment in value)
                    allowTokens.Add(segment);
            }
        }
        
        public IEnumerable<TokenSegment> ToTokenSegments()
        {
            foreach (var tokenInfo in tokens)
            {
                TokenSegment segment;
                
                if (tokenInfo.Tag.End is null) HandleSingleToken(tokenInfo, out segment);
                else HandlePairToken(tokenInfo, out segment);
                if (segment is not null) yield return segment;
            }

            TryAllowTokens();
            foreach (var tokenSegment in allowTokens)
            {
                yield return tokenSegment;
            }
        }
    }
}