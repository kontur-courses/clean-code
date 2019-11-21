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
            var stack = new Stack<PairToken>();
            var validTokens = new List<IToken>();
            foreach (var token in tokens)
                if (token is PairToken pairToken)
                    ProcessTokenOnPairMatching(pairToken, stack, validTokens);
                else
                    validTokens.Add(token);

            return validTokens.OrderBy(token => token.Position);
        }

        private void ProcessTokenOnPairMatching(PairToken pairToken, Stack<PairToken> openingTokens,
            List<IToken> listToAdd)
        {
            if (pairToken.IsClosing && openingTokens.Count > 0 && !openingTokens.Peek().IsClosing)
            {
                var openToken = openingTokens.Pop();
                if (openToken.Type == pairToken.Type)
                {
                    listToAdd.Add(openToken);
                    listToAdd.Add(pairToken);
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
            var resultTokens = new List<IToken>();
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
                        if (lastOpeningToken != null)
                        {
                            resultTokens.Add(lastOpeningToken);
                            resultTokens.Add(pairToken);
                            lastOpeningToken = null;
                            continue;
                        }

                        if (lastClosingToken == null)
                        {
                            if (IsClosingTokenPairForNonMergeableToken(nonMergeableOpeningTokens, openingTokens))
                            {
                                lastClosingToken = pairToken;
                            }
                            else
                            {
                                resultTokens.Add(nonMergeableOpeningTokens.Pop());
                                resultTokens.Add(pairToken);
                            }
                        }
                        else
                        {
                            lastClosingToken =
                                TryMergeClosingTokens(resultTokens, openingTokens, lastClosingToken, pairToken)
                                    ? null
                                    : pairToken;
                        }
                    }
                }
                else
                {
                    resultTokens.Add(token);
                }

            if (lastOpeningToken != null)
                resultTokens.Add(lastOpeningToken);
            if (lastClosingToken != null)
                resultTokens.Add(lastClosingToken);

            while (openingTokens.Count > 0)
                resultTokens.Add(openingTokens.Pop());

            return resultTokens.OrderBy(token => token.Position);
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
            List<IToken> listToAdd,
            Stack<PairToken> mergeableOpeningTokens,
            PairToken lastClosingToken,
            PairToken closingToken)
        {
            if (lastClosingToken.Position == closingToken.Position - 1)
            {
                mergeableOpeningTokens.Pop();
                listToAdd.Add(
                    new PairToken(AttributeType.Strong,
                        mergeableOpeningTokens.Pop().Position,
                        false,
                        2)
                );
                listToAdd.Add(
                    new PairToken(AttributeType.Strong,
                        lastClosingToken.Position,
                        true,
                        2)
                );
                return true;
            }

            listToAdd.Add(lastClosingToken);
            return false;
        }

        private IEnumerable<IToken> RemoveNestedPairTokensOfSameType(
            IEnumerable<IToken> tokens, params AttributeType[] typesToExclude)
        {
            var nestedTypeTokens = typesToExclude.ToDictionary(type => type, type => new Stack<PairToken>());
            var result = new List<IToken>();

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
                            result.Add(nestedTokenPair.Item1);
                            result.Add(nestedTokenPair.Item2);
                        }
                        else
                        {
                            result.Add(openingToken);
                            result.Add(token);
                        }
                    }
                }
                else
                {
                    result.Add(token);
                }

            return result.OrderBy(token => token.Position);
        }

        private bool TryToCreatePairOfNestedTokens(
            Dictionary<AttributeType, Stack<PairToken>> nestedTypeTokens,
            PairToken openingToken,
            PairToken closingToken,
            out (IToken, IToken) nestedTokenPair)
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
            PairToken openingHeader = null;
            PairToken closingHeader = null;
            PairToken openingDescription = null;
            var openingHeaders = new Stack<PairToken>();
            var result = new List<IToken>();
            foreach (var token in tokens)
                if (token is PairToken pairToken)
                {
                    if (openingHeader == null)
                        ProcessTokenToMatchOpeningLinkHeader(result, pairToken, ref openingHeader);
                    else if (closingHeader == null)
                        ProcessTokenToMatchClosingLinkHeader(
                            result,
                            openingHeaders,
                            pairToken,
                            ref openingHeader,
                            ref closingHeader);
                    else if (openingDescription == null)
                        ProcessTokenToMatchOpeningLinkDescription(
                            openingHeaders,
                            pairToken,
                            ref openingHeader,
                            ref closingHeader,
                            ref openingDescription);
                    else
                        ProcessTokenToMatchClosingLinkDescription(
                            result,
                            source,
                            pairToken,
                            ref openingHeader,
                            ref closingHeader,
                            ref openingDescription);
                }
                else
                {
                    if (openingDescription == null)
                        result.Add(token);
                }

            return result.OrderBy(token => token.Position);
        }

        private void ProcessTokenToMatchOpeningLinkHeader(
            List<IToken> listToAdd, PairToken currentToken, ref PairToken openingHeader)
        {
            if (currentToken.Type == AttributeType.LinkHeader && !currentToken.IsClosing)
            {
                openingHeader = currentToken;
            }
            else
            {
                if (currentToken.Type != AttributeType.LinkDescription && currentToken.Type != AttributeType.LinkHeader)
                    listToAdd.Add(currentToken);
            }
        }

        private void ProcessTokenToMatchClosingLinkHeader(
            List<IToken> listToAdd,
            Stack<PairToken> openingHeaders,
            PairToken currentToken,
            ref PairToken openingHeader,
            ref PairToken closingHeader)
        {
            if (currentToken.Type == AttributeType.LinkHeader)
            {
                if (currentToken.IsClosing)
                {
                    closingHeader = currentToken;
                }
                else
                {
                    openingHeaders.Push(openingHeader);
                    openingHeader = currentToken;
                }
            }
            else
            {
                if (currentToken.Type != AttributeType.LinkDescription && currentToken.Type != AttributeType.LinkHeader)
                    listToAdd.Add(currentToken);
            }
        }

        private void ProcessTokenToMatchOpeningLinkDescription(
            Stack<PairToken> openingHeaders,
            PairToken currentToken,
            ref PairToken openingHeader,
            ref PairToken closingHeader,
            ref PairToken openingDescription)
        {
            if (currentToken.Type == AttributeType.LinkHeader)
            {
                if (!currentToken.IsClosing)
                {
                    openingHeader = currentToken;
                    closingHeader = null;
                }
                else
                {
                    if (openingHeaders.Count <= 0)
                        return;
                    openingHeader = openingHeaders.Pop();
                    closingHeader = currentToken;
                }
            }
            else if (currentToken.Type == AttributeType.LinkDescription)
            {
                if (!currentToken.IsClosing)
                {
                    if (closingHeader.Position == currentToken.Position - 1)
                    {
                        openingDescription = currentToken;
                    }
                    else
                    {
                        openingHeader = openingHeaders.Count > 0 ? openingHeaders.Pop() : null;
                        closingHeader = null;
                    }
                }
            }
            else
            {
                openingHeader = openingHeaders.Count > 0 ? openingHeaders.Pop() : null;
                closingHeader = null;
            }
        }

        private void ProcessTokenToMatchClosingLinkDescription(
            List<IToken> listToAdd,
            string textSource,
            PairToken currentToken,
            ref PairToken openingHeader,
            ref PairToken closingHeader,
            ref PairToken openingDescription)
        {
            if (currentToken.Type == AttributeType.LinkDescription)
                if (currentToken.IsClosing)
                {
                    var linkTokens = CreatePairOfLinkTokens(
                        textSource, 
                        openingHeader, 
                        closingHeader,
                        openingDescription,
                        currentToken
                        );

                    listToAdd.Add(linkTokens.Item1);
                    listToAdd.Add(linkTokens.Item2);
                    openingHeader = null;
                    closingHeader = null;
                    openingDescription = null;
                }
        }

        private (LinkToken, LinkToken) CreatePairOfLinkTokens(
            string textSource,
            PairToken openingHeader,
            PairToken closingHeader,
            PairToken openingDescription,
            PairToken closingDescription)
        {
            return (new LinkToken(
                        openingHeader.Position,
                        1,
                        textSource
                            .Substring(
                                openingDescription.Position + 1,
                                closingDescription.Position - openingDescription.Position - 1
                            )
                    ),
                    new LinkToken(
                        closingHeader.Position,
                        closingDescription.Position - openingDescription.Position + 2,
                        null
                        )
                );
        }
    }
}