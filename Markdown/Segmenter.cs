using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Segmenter
    {
        private readonly IEnumerable<TokenInfo> tokens;
        private readonly TagRules rules;
        
        private readonly Dictionary<int, List<TokenSegment>> allowToken = new();
        private readonly HashSet<TokenInfo> currentOpenedTag = new();
        private readonly HashSet<TokenInfo> ignoredTokens = new();
        private readonly HashSet<TokenInfo> provisionalContainRuleIgnoredTokens = new();
        private readonly Stack<TokenInfo> tokenStack = new();
        
        public Segmenter(IEnumerable<TokenInfo> tokens, TagRules rules)
        {
            this.tokens = tokens ?? throw new ArgumentNullException();
            this.rules = rules;
        }

        private bool IsThisTokenOpened(TokenInfo tokenInfo)
        {
            if (currentOpenedTag.RemoveWhere(x => x.Token == tokenInfo.Token) != 0)
            {
                // ignoredTokens.RemoveWhere(x => x.Token == tokenInfo.Token);
                return false;
            }
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
            if (!allowToken.ContainsKey(allowPosition)) allowToken[allowPosition] = new List<TokenSegment>();
            allowToken[allowPosition].Add(new TokenSegment(closeTokenToSearch.WithPosition(openedToken.Position), closeTokenToSearch));
        }

        private bool DoesMatchNestingRule(TokenSegment segment)
        {
            if (!tokenStack.Any()) return true;
            if (rules.CanBeNested(Tag.GetTagByChars(tokenStack.Peek().Token), segment.GetBaseTag())) return true;
            var allowPosition = tokenStack.Peek().Position;
            if (!allowToken.ContainsKey(allowPosition)) allowToken[allowPosition] = new List<TokenSegment>();
            allowToken[allowPosition].Add(segment);
            return false;
        }

        private bool ValidateByContainRule(TokenInfo tokenInfo)
        {
            var token = provisionalContainRuleIgnoredTokens.FirstOrDefault(x => x.Token == tokenInfo.Token);
            return token is null || !tokenInfo.WordPartPlaced;
        }

        private void HandlePairToken(TokenInfo tokenInfo, out TokenSegment validSegment)
        {
            var (index, token, closeValid, openValid, inWordPartPlaced, _) = tokenInfo;
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
            // segment = new TokenSegment(Tag.GetTagByChars(tokenInfo.Token), tokenInfo.Position, tokenInfo.Position);
            segment = new TokenSegment(tokenInfo);

            foreach (var openedTokenInfo in currentOpenedTag)
            {
                if (!rules.CanContain(Tag.GetTagByChars(openedTokenInfo.Token), segment.GetBaseTag()))
                {
                    if (openedTokenInfo.WordPartPlaced)
                        ignoredTokens.Add(openedTokenInfo);
                    else
                        provisionalContainRuleIgnoredTokens.Add(openedTokenInfo);
                }
            }

            if (!rules.DoesMatchInFrontRule(segment)) segment = null;
        }
        
        public IEnumerable<TokenSegment> ToTokenSegments()
        {
            foreach (var tokenInfo in tokens)
            {
                var (index, token, closeValid, openValid, inWordPartPlaced, _) = tokenInfo;
                TokenSegment segment;
                
                if (Tag.GetTagByChars(token).End is null) HandleSingleToken(tokenInfo, out segment);
                else HandlePairToken(tokenInfo, out segment);
                if (segment is not null) yield return segment;
            }

            foreach (var (key, value) in allowToken)
            {
                var allower = currentOpenedTag.FirstOrDefault(x => x.Position == key);
                
                if (allower is not null)
                    foreach (var segment in value)
                    {
                        yield return segment;
                    }
                if (allower is not null && Tag.GetTagByChars(allower.Token).End is null)
                    foreach (var segment in value)
                    {
                        if (!segment.InTextSegment) yield return segment;
                    }
            }
        }
    }
}