using System;
using System.Collections.Generic;

namespace Markdown.TokenizerLogic
{
    public class TokenPairer
    {
        private IEnumerable<Token> filtered;
        private Stack<PairedTokenInfo> toPair;
        private bool isWord;
        private bool isTagPairSkipped;
        private bool isLastPaired;

        private TokenPairer(IEnumerable<Token> filteredTokens)
        {
            filtered = filteredTokens;
            toPair = new Stack<PairedTokenInfo>();
        }

        public static IEnumerable<Token> PairFilteredTokens(IEnumerable<Token> filteredTokens)
        {
            var pairer = new TokenPairer(filteredTokens);
            pairer.Apply();
            return pairer.filtered;
        }

        private void Apply()
        {
            foreach (var token in filtered)
                HandleToken(token);

            DiscardUnpaired();
        }

        private void HandleToken(Token token)
        {
            switch (token)
            {
                case ItalicToken em:
                    {
                        if (!em.IsEscaped)
                            HandlePairedToken(em);
                        break;
                    }
                case BoldToken bold:
                    {
                        if (!bold.IsEscaped)
                            HandlePairedToken(bold);
                        break;
                    }
                case TagToken:
                    {
                        EndWord();
                        DiscardUnpaired();
                        break;
                    }
                case SpaceToken:
                    {
                        EndWord();
                        break;
                    }
                case SingleToken single:
                    {
                        HandleSingleToken(single);
                        break;
                    }
                default:
                    throw new ArgumentException("Unknown token received");
            }
        }

        private void HandlePairedToken(PairedToken token)
        {
            if (isWord)
            {
                if (CanPair(token))
                    PairTokens(token, token is BoldToken);
                else
                    AddPaired(token, true);
            }
            else
                AddPaired(token, false);
        }

        private void HandleSingleToken(SingleToken token)
        {
            StartWord();
            if (token is TextToken text)
                isTagPairSkipped = text.WithDigits ? true : isTagPairSkipped;
        }

        private bool CanPair(PairedToken token)
        {
            return toPair.Count > 0
                && toPair.Peek().CanOpen
                && toPair.Peek().IsSameType(token);
        }

        private void PairTokens(PairedToken token, bool isBold)
        {
            var pair = toPair.Pop();

            if (isTagPairSkipped)
            {
                pair.Token.Escape();
                token.Escape();
            }
            else
                HandlePairing(token, isBold, pair);

            isLastPaired = false;
        }

        private void HandlePairing(PairedToken token, bool isBold, PairedTokenInfo pair)
        {
            if (isBold
                && IsIncorrectNesting())
            {
                pair.Token.Escape();
                token.Escape();
            }
            else
            {
                token.Close();
                isWord = false;
            }
        }

        private bool IsIncorrectNesting()
        {
            return toPair.Count > 0
                && toPair.Peek().Token is ItalicToken
                && toPair.Peek().CanOpen;
        }

        private void AddPaired(PairedToken token, bool isClosing)
        {
            isLastPaired = true;
            var newPair = new PairedTokenInfo(token);
            if (isClosing)
                newPair.Close();
            else
                newPair.Open();
            toPair.Push(newPair);
        }

        private void StartWord()
        {
            if (isLastPaired)
            {
                toPair.Peek().Open();
                isLastPaired = false;
            }
            isWord = true;
        }

        private void EndWord()
        {
            if (toPair.Count > 0
                && toPair.Peek().CanOpen
                && toPair.Peek().CanClose)
                toPair.Peek().DisableOpen();
            isLastPaired = false;
            isWord = false;
            isTagPairSkipped = false;
        }

        private void DiscardUnpaired()
        {
            foreach (var unpaired in toPair)
                unpaired.Token.Escape();
            toPair.Clear();
        }
    }
}
