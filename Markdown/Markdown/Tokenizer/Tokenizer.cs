using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Tokenizer : ITokenizer
    {
        private readonly ISyntax syntax;

        public Tokenizer(ISyntax syntax)
        {
            this.syntax = syntax;
        }

        public TokenText ParseText(string source)
        {
            var tokens = GetRawTokens(source);
            tokens = GetNonEscapedTokens(tokens);
            tokens = RemoveNonPairDelimiters(tokens);
            tokens = MergeAdjacentEmphasisDelimiters(tokens);
            tokens = RemoveNestedPairTokensOfSameType(tokens, AttributeType.Emphasis, AttributeType.Strong);
            tokens = AddValidLinkTokens(tokens, source);

            return new TokenText(source, tokens);
        }

        private IEnumerable<IToken> GetRawTokens(string source)
        {
            for (var i = 0; i < source.Length; i++)
                if (syntax.TryGetCharAttribute(source, i, out var type))
                    yield return CreateToken(type, source, i);
        }


        public IToken CreateToken(Attribute attribute, string source, int charPosition)
        {
            if (attribute.Type == AttributeType.Escape)
                return new SingleToken(attribute.Type, charPosition);

            var pairAttribute = attribute as PairAttribute;

            if (attribute.Type == AttributeType.Emphasis)
                return new PairToken(
                    attribute.Type,
                    charPosition,
                    pairAttribute.IsCharClosing(source, charPosition)
                );

            if (attribute.Type == AttributeType.LinkHeader || attribute.Type == AttributeType.LinkDescription)
                return new PairToken(
                    attribute.Type,
                    charPosition,
                    pairAttribute.IsCharClosing(source, charPosition)
                );

            throw new Exception("Couldn't create token of given type");
        }

        private IEnumerable<IToken> GetNonEscapedTokens(IEnumerable<IToken> tokens)
        {
            IToken previous = null;
            foreach (var token in tokens)
                if (previous == null || previous.Type != AttributeType.Escape ||
                    previous.Position != token.Position - 1)
                {
                    yield return token;
                    previous = token;
                }
        }

        private IEnumerable<IToken> RemoveNonPairDelimiters(IEnumerable<IToken> tokens)
        {
            var openingTokens = new Stack<PairToken>();
            var validTokens = new LinkedList<IToken>();

            foreach (var token in tokens)
                if (token is PairToken pairToken)
                    ProcessTokenOnPairMatching(pairToken, openingTokens, validTokens);
                else
                    AddPairTokenToLinkedList(validTokens, token);

            return validTokens;
        }

        private void ProcessTokenOnPairMatching(PairToken pairToken, Stack<PairToken> openingTokens,
            LinkedList<IToken> listToAdd)
        {
            if (pairToken.IsClosing && openingTokens.Count > 0 && !openingTokens.Peek().IsClosing)
            {
                var openToken = openingTokens.Pop();
                if (openToken.Type == pairToken.Type)
                {
                    AddPairTokenToLinkedList(listToAdd, openToken);
                    AddPairTokenToLinkedList(listToAdd, pairToken);
                }
                else
                {
                    if (openToken.Type != AttributeType.Emphasis)
                        openingTokens.Push(openToken);
                    else
                        ProcessTokenOnPairMatching(pairToken, openingTokens, listToAdd);
                }
            }
            else
            {
                openingTokens.Push(pairToken);
            }
        }

        private IEnumerable<IToken> MergeAdjacentEmphasisDelimiters(IEnumerable<IToken> tokens)
        {
            var resultTokens = new LinkedList<IToken>();
            var openingTokens = new Stack<PairToken>();
            var nonMergeableOpeningTokens = new Stack<PairToken>();
            PairToken lastOpeningToken = null;
            PairToken lastClosingToken = null;

            foreach (var token in tokens)
                if (token is PairToken pairToken && token.Type == AttributeType.Emphasis)
                {
                    if (!pairToken.IsClosing)
                    {
                        if (lastOpeningToken == null)
                            lastOpeningToken = pairToken;
                        else
                            lastOpeningToken = TryMergeOpeningTokens(
                                openingTokens,
                                nonMergeableOpeningTokens,
                                pairToken,
                                lastOpeningToken)
                                ? null
                                : pairToken;
                    }
                    else
                    {
                        ProcessClosingTokenForMerging(
                            resultTokens, 
                            nonMergeableOpeningTokens, 
                            openingTokens,
                            pairToken, 
                            ref lastOpeningToken, 
                            ref lastClosingToken);
                    }
                }
                else
                {
                    AddPairTokenToLinkedList(resultTokens, token);
                }

            if (lastOpeningToken != null)
                AddPairTokenToLinkedList(resultTokens, lastOpeningToken);
            if (lastClosingToken != null)
                AddPairTokenToLinkedList(resultTokens, lastClosingToken);
            foreach (var leftToken in openingTokens)
                AddPairTokenToLinkedList(resultTokens, leftToken);

            return resultTokens;
        }

        private void ProcessClosingTokenForMerging(
            LinkedList<IToken> listToAdd, 
            Stack<PairToken> nonMergeableOpeningTokens,
            Stack<PairToken> mergeableOpeningTokens,
            PairToken currentClosingToken,
            ref PairToken lastOpeningToken, 
            ref PairToken lastClosingToken)
        {
            if (lastOpeningToken != null)
            {
                AddPairTokenToLinkedList(listToAdd, lastOpeningToken);
                AddPairTokenToLinkedList(listToAdd, currentClosingToken);

                lastOpeningToken = null;
                return;
            }
            if (lastClosingToken == null)
            {
                if (IsClosingTokenPairForNonMergeableToken(nonMergeableOpeningTokens, mergeableOpeningTokens))
                {
                    lastClosingToken = currentClosingToken;
                }
                else
                {
                    AddPairTokenToLinkedList(listToAdd, nonMergeableOpeningTokens.Pop());
                    AddPairTokenToLinkedList(listToAdd, currentClosingToken);
                }
            }
            else
            {
                lastClosingToken =
                    TryMergeClosingTokens(listToAdd, mergeableOpeningTokens, lastClosingToken, currentClosingToken)
                        ? null
                        : currentClosingToken;
            }
        }

        private bool IsClosingTokenPairForNonMergeableToken(
            Stack<PairToken> nonMergeableOpeningTokens, Stack<PairToken> mergeableOpeningTokens)
        {
            return nonMergeableOpeningTokens.Count == 0
                   || mergeableOpeningTokens.Count > 0
                   && nonMergeableOpeningTokens.Peek().Position < mergeableOpeningTokens.Peek().Position;
        }

        private bool TryMergeOpeningTokens(
            Stack<PairToken> mergeableOpeningTokens,
            Stack<PairToken> nonMergeableOpeningTokens,
            PairToken openingToken,
            PairToken lastOpeningToken)
        {
            if (lastOpeningToken.Position == openingToken.Position - 1)
            {
                mergeableOpeningTokens.Push(lastOpeningToken);
                mergeableOpeningTokens.Push(openingToken);
                return true;
            }

            nonMergeableOpeningTokens.Push(lastOpeningToken);
            return false;
        }

        private bool TryMergeClosingTokens(
            LinkedList<IToken> listToAdd,
            Stack<PairToken> mergeableOpeningTokens,
            PairToken lastClosingToken,
            PairToken closingToken)
        {
            if (lastClosingToken.Position == closingToken.Position - 1)
            {
                mergeableOpeningTokens.Pop();
                var opening = new PairToken(AttributeType.Strong, mergeableOpeningTokens.Pop().Position, false, 2);
                var closing = new PairToken(AttributeType.Strong, lastClosingToken.Position, true, 2);

                AddPairTokenToLinkedList(listToAdd, opening);
                AddPairTokenToLinkedList(listToAdd, closing);
                return true;
            }

            AddPairTokenToLinkedList(listToAdd, lastClosingToken);
            return false;
        }

        private IEnumerable<IToken> RemoveNestedPairTokensOfSameType(
            IEnumerable<IToken> tokens, params AttributeType[] typesToExclude)
        {
            var nestedTypeTokens = typesToExclude.ToDictionary(type => type, type => new Stack<PairToken>());
            var result = new LinkedList<IToken>();

            foreach (var token in tokens)
                if (token is PairToken pairToken && nestedTypeTokens.ContainsKey(token.Type))
                {
                    if (!pairToken.IsClosing)
                    {
                        nestedTypeTokens[pairToken.Type].Push(pairToken);
                    }
                    else
                    {
                        var openingToken = nestedTypeTokens[token.Type].Pop();
                        if (TryToCreatePairOfNestedTokens(
                            nestedTypeTokens,
                            openingToken,
                            pairToken,
                            out var nestedTokenPair))
                        {
                            AddPairTokenToLinkedList(result, nestedTokenPair.OpeningToken);
                            AddPairTokenToLinkedList(result, nestedTokenPair.ClosingToken);
                        }
                        else
                        {
                            AddPairTokenToLinkedList(result, openingToken);
                            AddPairTokenToLinkedList(result, token);
                        }
                    }
                }
                else
                {
                    AddPairTokenToLinkedList(result, token);
                }

            return result;
        }

        private bool TryToCreatePairOfNestedTokens(
            Dictionary<AttributeType, Stack<PairToken>> nestedTypeTokens,
            PairToken openingToken,
            PairToken closingToken,
            out (IToken OpeningToken, IToken ClosingToken) nestedTokenPair)
        {
            if (nestedTypeTokens[closingToken.Type].Count > 0)
            {
                nestedTokenPair =
                    (new SingleToken(
                            AttributeType.None,
                            openingToken.Position,
                            openingToken.AttributeLength
                        ),
                        new SingleToken(
                            AttributeType.None,
                            closingToken.Position,
                            closingToken.AttributeLength
                        )
                    );
                return true;
            }
            nestedTokenPair = (null, null);
            return false;
        }

        private IEnumerable<IToken> AddValidLinkTokens(IEnumerable<IToken> tokens, string source)
        {
            var linkComponents = new LinkComponents();
            var openingHeaders = new Stack<PairToken>();
            var result = new LinkedList<IToken>();

            foreach (var token in tokens)
                if (token is PairToken pairToken)
                {
                    if (linkComponents.OpeningHeader == null)
                        ProcessTokenToMatchOpeningLinkHeader(result, pairToken, linkComponents);
                    else if (linkComponents.ClosingHeader == null)
                        ProcessTokenToMatchClosingLinkHeader(result, openingHeaders, pairToken, linkComponents);
                    else if (linkComponents.OpeningDescription == null)
                        ProcessTokenToMatchOpeningLinkDescription(openingHeaders, pairToken, linkComponents);
                    else
                        ProcessTokenToMatchClosingLinkDescription(result,source, pairToken, linkComponents);
                }
                else
                {
                    if (linkComponents.OpeningDescription == null)
                        AddPairTokenToLinkedList(result, token);
                }

            return result;
        }

        private void ProcessTokenToMatchOpeningLinkHeader(
            LinkedList<IToken> listToAdd, PairToken currentToken, LinkComponents linkComponents)
        {
            if (currentToken.Type == AttributeType.LinkHeader && !currentToken.IsClosing)
            {
                linkComponents.OpeningHeader = currentToken;
            }
            else
            {
                if (currentToken.Type != AttributeType.LinkDescription && currentToken.Type != AttributeType.LinkHeader)
                    AddPairTokenToLinkedList(listToAdd, currentToken);
            }
        }

        private void ProcessTokenToMatchClosingLinkHeader(
            LinkedList<IToken> listToAdd, Stack<PairToken> openingHeaders, PairToken currentToken, LinkComponents linkComponents)
        {
            if (currentToken.Type == AttributeType.LinkHeader)
            {
                if (currentToken.IsClosing)
                {
                    linkComponents.ClosingHeader = currentToken;
                }
                else
                {
                    openingHeaders.Push(linkComponents.OpeningHeader);
                    linkComponents.OpeningHeader = currentToken;
                }
            }
            else
            {
                if (currentToken.Type != AttributeType.LinkDescription && currentToken.Type != AttributeType.LinkHeader)
                    AddPairTokenToLinkedList(listToAdd, currentToken);
            }
        }

        private void ProcessTokenToMatchOpeningLinkDescription(
            Stack<PairToken> openingHeaders, PairToken currentToken, LinkComponents linkComponents)
        {
            if (currentToken.Type == AttributeType.LinkHeader)
            {
                if (!currentToken.IsClosing)
                {
                    linkComponents.OpeningHeader = currentToken;
                    linkComponents.ClosingHeader = null;
                }
                else
                {
                    if (openingHeaders.Count <= 0)
                        return;
                    linkComponents.OpeningHeader = openingHeaders.Pop();
                    linkComponents.ClosingHeader = currentToken;
                }
            }
            else if (currentToken.Type == AttributeType.LinkDescription)
            {
                if (!currentToken.IsClosing)
                {
                    if (linkComponents.ClosingHeader.Position == currentToken.Position - 1)
                    {
                        linkComponents.OpeningDescription = currentToken;
                    }
                    else
                    {
                        linkComponents.OpeningHeader = openingHeaders.Count > 0 ? openingHeaders.Pop() : null;
                        linkComponents.ClosingHeader = null;
                    }
                }
            }
            else
            {
                linkComponents.OpeningHeader = openingHeaders.Count > 0 ? openingHeaders.Pop() : null;
                linkComponents.ClosingHeader = null;
            }
        }

        private void ProcessTokenToMatchClosingLinkDescription(
            LinkedList<IToken> listToAdd, string textSource, PairToken currentToken, LinkComponents linkComponents)
        {
            if (currentToken.Type == AttributeType.LinkDescription)
                if (currentToken.IsClosing)
                {
                    linkComponents.ClosingDescription = currentToken;
                    var linkTokens = CreatePairOfLinkTokens(textSource, linkComponents);
                    AddPairTokenToLinkedList(listToAdd, linkTokens.OpeningLink);
                    AddPairTokenToLinkedList(listToAdd, linkTokens.ClosingLink);
                    linkComponents.OpeningHeader = null;
                    linkComponents.ClosingHeader = null;
                    linkComponents.OpeningDescription = null;
                    linkComponents.ClosingDescription = null;
                }
        }

        private (LinkToken OpeningLink, LinkToken ClosingLink) CreatePairOfLinkTokens(
            string textSource, LinkComponents linkComponents)
        {
            if (linkComponents.OpeningHeader == null || linkComponents.ClosingHeader == null 
                || linkComponents.OpeningDescription == null || linkComponents.ClosingDescription == null)
                throw new NullReferenceException("One of the link components is null");

            return (new LinkToken(
                        linkComponents.OpeningHeader.Position,
                        1,
                        textSource
                            .Substring(
                                linkComponents.OpeningDescription.Position + 1,
                                linkComponents.ClosingDescription.Position - linkComponents.OpeningDescription.Position - 1
                            )
                    ),
                    new LinkToken(
                        linkComponents.ClosingHeader.Position,
                        linkComponents.ClosingDescription.Position - linkComponents.OpeningDescription.Position + 2,
                        null
                        )
                );
        }

        private void AddPairTokenToLinkedList(LinkedList<IToken> list, IToken token)
        {
            if (list.Count == 0 || token.Position > list.Last.Value.Position)
                list.AddLast(token);
            else
                list.AddFirst(token);
        }

        private class LinkComponents
        {
            public PairToken OpeningHeader;
            public PairToken ClosingHeader;
            public PairToken OpeningDescription;
            public PairToken ClosingDescription;
        }
    }
}