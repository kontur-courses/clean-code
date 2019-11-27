using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programm
{
    public class UnderScoresHandler
    {
        public static Token[] HandleUnderScores(Token[] tokens)
        {
            var tokensEscapeUnderScores = MakeUnderScoresEscapeIfNeededAndGetTokens(tokens);
            var tokensAdjacentUnderScoresInOneToken = MakeAdjacentUnderScoresInOneTokenAndGetTokens(
                tokensEscapeUnderScores);
            var tokensUnderScoresDividedIntoPairs = DivideUnderScoresIntoPairsAndGetTokens(
                tokensAdjacentUnderScoresInOneToken);
            var tokensDoubleUnderScoresInInsertionMuted = MuteDoubleUnderScoresInInsertAndGetTokens(
                tokensUnderScoresDividedIntoPairs);
            return tokensDoubleUnderScoresInInsertionMuted;
        }

        private static Token[] MakeUnderScoresEscapeIfNeededAndGetTokens(Token[] tokens)
        {
            var len = tokens.Length;
            for (var i = 0; i < len; i++)
            {
                if (tokens[i].OriginalValue == "_" && i > 0 &&
                    char.IsDigit(tokens[i - 1].OriginalValue[0]))
                    tokens[i].IsEscapeChar = true;
                if (tokens[i].OriginalValue == "_" && i != len - 1 &&
                    char.IsDigit(tokens[i + 1].OriginalValue[0]))
                    tokens[i].IsEscapeChar = true;
            }
            return tokens;
        }

        private static Token[] MakeAdjacentUnderScoresInOneTokenAndGetTokens(Token[] tokens)
        {
            var len = tokens.Length;
            var tokensList = new List<Token>();
            for (var i = 0; i < len;)
            {
                if (tokens[i].OriginalValue != "_" || tokens[i].IsEscapeChar)
                {
                    tokensList.Add(tokens[i]);
                    i++;
                    continue;
                }
                var underScoresCnt = 0;
                while (i < len && tokens[i].OriginalValue == "_" &&
                       !tokens[i].IsEscapeChar)
                {
                    underScoresCnt++;
                    i++;
                }
                tokensList.Add(new Token(new string('_', underScoresCnt), new string('_', underScoresCnt), false));
            }
            return tokensList.ToArray();
        }

        private static Token[] DivideUnderScoresIntoPairsAndGetTokens(Token[] tokens)
        {
            var indexAndTokenstack = new Stack<Tuple<int, Token>>();
            var len = tokens.Length;
            var tokensInIndex = new List<ThreePartList<Token>>();
            for (var i = 0; i < len; i++)
                tokensInIndex.Add(new ThreePartList<Token>());
            for (var i = 0; i < len; i++)
            {
                if (tokens[i].OriginalValue[0] != '_' || tokens[i].IsEscapeChar)
                {
                    tokensInIndex[i].Beginning.Add(tokens[i]);
                    continue;
                }
                if (UnderScoreCanBeClosing(tokens, i) && indexAndTokenstack.Count != 0)
                    HandleNewUnderScores(tokens, indexAndTokenstack, tokensInIndex, i);
                if (tokens[i].OriginalValue != "")
                    indexAndTokenstack.Push(new Tuple<int, Token>(i, tokens[i]));
            }
            while (indexAndTokenstack.Count != 0)
            {
                for (var j = 0; j < indexAndTokenstack.Peek().Item2.OriginalValue.Length; j++)
                    tokensInIndex[indexAndTokenstack.Peek().Item1].Middle.Add(new Token("_", "_", false));
                indexAndTokenstack.Pop();
            }
            var finalTokens = new List<Token>();
            for (var i = 0; i < tokens.Length; i++)
            {
                foreach (var curToken in tokensInIndex[i])
                    finalTokens.Add(curToken);
            }
            return finalTokens.ToArray();   
        }

        private static bool UnderScoreCanBeClosing(Token[] tokens, int indexInString) => 
            indexInString != 0 && tokens[indexInString - 1].OriginalValue != " ";

        private static bool UnderScoreCanBeOpening(Token[] tokens, int indexInString) => 
            indexInString != tokens.Length - 1 && tokens[indexInString + 1].OriginalValue != " ";

        private static void HandleNewUnderScores(Token[] tokens, Stack<Tuple<int, Token>> underScoresStack,
            List<ThreePartList<Token>> tokensInIndex, int indexInTokens)
        {
            var tokenLen = tokens[indexInTokens].OriginalValue.Length;
            while (underScoresStack.Count != 0 && tokenLen != 0)
            {
                var prevToken = underScoresStack.Pop();
                var prevTokenLen = prevToken.Item2.OriginalValue.Length;
                var prevTokenIndexInIndex = prevToken.Item1;
                if (!UnderScoreCanBeOpening(tokens, prevTokenIndexInIndex))
                {

                    tokensInIndex[prevTokenIndexInIndex].
                        Ending.Add(new Token("_", "_", false));
                    prevTokenLen--;
                }
                if (tokenLen % 2 != 0 && prevTokenLen % 2 != 0 || 
                    tokenLen == 1 && prevTokenLen >= 1 ||
                    tokenLen >= 1 && prevTokenLen == 1)
                {
                    tokensInIndex[prevTokenIndexInIndex].Ending.Add(new Token("<em>", "_", false));
                    tokensInIndex[indexInTokens].Beginning.Add(new Token("</em>", "_", false));
                    prevTokenLen--;
                    tokenLen--;
                }
                while (prevTokenLen != 0 && tokenLen != 0)
                {
                    tokensInIndex[prevTokenIndexInIndex].Ending.Add(new Token("<strong>", "__", false));
                    tokensInIndex[indexInTokens].Beginning.Add(new Token("</strong>", "__", false));
                    tokenLen -= 2;
                    prevTokenLen -= 2;
                }
                if (prevTokenLen != 0)
                    underScoresStack.Push(new Tuple<int, Token>(prevTokenIndexInIndex, new Token(new string('_', prevTokenLen),
                        new string('_', prevTokenLen), false)));
                else
                    tokensInIndex[prevTokenIndexInIndex].Ending.Reverse();
            }
            tokens[indexInTokens].OriginalValue = new string('_', tokenLen);
            tokens[indexInTokens].RenderedValue = new string('_', tokenLen);
        }

        private static Token[] MuteDoubleUnderScoresInInsertAndGetTokens(Token[] tokens)
        {
            var insertionNestingCnt = 0;
            foreach (var curToken in tokens)
            {
                if (curToken.RenderedValue == "<em>")
                    insertionNestingCnt++;
                else if (curToken.RenderedValue == "</em>")
                    insertionNestingCnt--;
                else if ((curToken.RenderedValue == "<strong>" || curToken.RenderedValue == "</strong>") &&
                         insertionNestingCnt != 0)
                    curToken.RenderedValue = "__";
            }
            return tokens;
        }
    }
}