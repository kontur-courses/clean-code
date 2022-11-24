using System.Collections.Generic;

namespace Markdown
{
    public class SemanticLevelTokenizer
    {
        /*Анализ предыдущего списка: опредление может ли токен быть тегом или нет, если нет, то тогда его тип
         заменяется на String*/
        public List<SecondLevelToken> Tokenize(List<SecondLevelToken> tagTokenList)
        {
            var tokenIndexesStack = new Stack<int>();
            for (int i = 0; i < tagTokenList.Count; i++)
            {
                var currentItemType = tagTokenList[i].GetSecondTokenType();
                switch (currentItemType)
                {
                    case SecondLevelTokenType.OpeningItalics:
                        tokenIndexesStack.Push(i);
                        break;
                    case SecondLevelTokenType.OpeningBold:
                        DoIfItemIsOpeningBold(tagTokenList, tokenIndexesStack, i);
                        break;
                    case SecondLevelTokenType.ClosingItalics:
                        DoIfItemClosingItalics(tagTokenList, tokenIndexesStack, i);
                        break;
                    case SecondLevelTokenType.ClosingBold:
                        DoIfItemClosingBold(tagTokenList, tokenIndexesStack, i);
                        break;
                    case SecondLevelTokenType.OpenCloseItalics:
                        DoIfItemOpenCloseItalics(tagTokenList, tokenIndexesStack, i);
                        break;
                    case SecondLevelTokenType.OpenCloseBold:
                        DoIfOpenCloseBold(tagTokenList, tokenIndexesStack, i);
                        break;
                    case SecondLevelTokenType.Space:
                        DoItItemIsSpace(tagTokenList, tokenIndexesStack);
                        break;
                }
            }

            while (tokenIndexesStack.Count > 0)
            {
                var topIndex = tokenIndexesStack.Pop();
                ChangeType(tagTokenList, topIndex, SecondLevelTokenType.String);
            }

            return tagTokenList;
        }

        private static void DoItItemIsSpace(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack)
        {
            if (tokenIndexesStack.Count == 0)
            {
                return;
            }
            
            while (tokenIndexesStack.Count > 0)
            {
                var topIndex = tokenIndexesStack.Pop();
                var topStackElementType = tagTokenList[topIndex].GetSecondTokenType();
                if (topStackElementType != SecondLevelTokenType.OpeningBold &&
                    topStackElementType != SecondLevelTokenType.OpeningItalics)
                {
                    ChangeType(tagTokenList, topIndex, SecondLevelTokenType.String);
                }
                else
                {
                    tokenIndexesStack.Push(topIndex);
                    break;
                }
            }
        }

        private static void DoIfOpenCloseBold(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack, int i)
        {
            if (tokenIndexesStack.Count == 0)
            {
                tokenIndexesStack.Push(i);
                return;
            }

            var topIndex = tokenIndexesStack.Pop();
            var topStackElementType = tagTokenList[topIndex].GetSecondTokenType();
            switch (topStackElementType)
            {
                case SecondLevelTokenType.OpeningBold:
                    ChangeType(tagTokenList, i, SecondLevelTokenType.ClosingBold);
                    return;
                case SecondLevelTokenType.OpenCloseBold:
                    ChangeType(tagTokenList, i, SecondLevelTokenType.ClosingBold);
                    ChangeType(tagTokenList, topIndex, SecondLevelTokenType.OpeningBold);
                    return;
                default:
                    tokenIndexesStack.Push(i);
                    break;
            }
        }

        private static void DoIfItemOpenCloseItalics(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack,
            int i)
        {
            if (tokenIndexesStack.Count == 0)
            {
                tokenIndexesStack.Push(i);
                return;
            }

            var topIndex = tokenIndexesStack.Pop();
            var topStackElementType = tagTokenList[topIndex].GetSecondTokenType();
            switch (topStackElementType)
            {
                case SecondLevelTokenType.OpenCloseItalics:
                    ChangeType(tagTokenList, topIndex, SecondLevelTokenType.OpeningItalics);
                    ChangeType(tagTokenList, i, SecondLevelTokenType.ClosingItalics);
                    return;
                case SecondLevelTokenType.OpeningItalics:
                    ChangeType(tagTokenList, i, SecondLevelTokenType.ClosingItalics);
                    return;
                case SecondLevelTokenType.OpenCloseBold:
                    ChangeType(tagTokenList, topIndex, SecondLevelTokenType.String);
                    ChangeType(tagTokenList, i, SecondLevelTokenType.String);
                    return;
                case SecondLevelTokenType.OpeningBold:
                    ChangeType(tagTokenList, topIndex, SecondLevelTokenType.String);
                    ChangeType(tagTokenList, i, SecondLevelTokenType.String);
                    return;
            }

            tokenIndexesStack.Push(i);
        }

        private static void ChangeType(IReadOnlyList<SecondLevelToken> tagTokenList, int i,
            SecondLevelTokenType newType)
        {
            tagTokenList[i].ChangeTokenType(newType);
        }

        private static void DoIfItemClosingBold(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack,
            int i)
        {
            if (tokenIndexesStack.Count == 0)
            {
                tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                return;
            }

            var topIndex = tokenIndexesStack.Pop();
            var topStackElementType = tagTokenList[topIndex].GetSecondTokenType();
            switch (topStackElementType)
            {
                case SecondLevelTokenType.OpeningItalics:
                    tokenIndexesStack.Push(topIndex);
                    tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                    return;
                case SecondLevelTokenType.OpeningBold:
                    return;
                case SecondLevelTokenType.OpenCloseBold:
                    tagTokenList[topIndex].ChangeTokenType(SecondLevelTokenType.OpeningBold);
                    return;
                case SecondLevelTokenType.OpenCloseItalics:
                    tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                    tagTokenList[topIndex].ChangeTokenType(SecondLevelTokenType.String);
                    break;
            }
        }

        private static void DoIfItemClosingItalics(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack,
            int i)
        {
            if (tokenIndexesStack.Count == 0)
            {
                tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                return;
            }

            var topIndex = tokenIndexesStack.Pop();
            var topStackElementType = tagTokenList[topIndex].GetSecondTokenType();
            switch (topStackElementType)
            {
                case SecondLevelTokenType.OpeningItalics:
                    return;
                case SecondLevelTokenType.OpenCloseItalics:
                    tagTokenList[topIndex].ChangeTokenType(SecondLevelTokenType.OpeningItalics);
                    return;
                default:
                    tagTokenList[topIndex].ChangeTokenType(SecondLevelTokenType.String);
                    tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                    break;
            }
        }

        private static void DoIfItemIsOpeningBold(List<SecondLevelToken> tagTokenList, Stack<int> tokenIndexesStack,
            int i)
        {
            if (tokenIndexesStack.Count == 0)
            {
                tokenIndexesStack.Push(i);
                return;
            }

            var topIndex = tokenIndexesStack.Pop();
            if (tagTokenList[topIndex].GetSecondTokenType() == SecondLevelTokenType.OpeningItalics)
            {
                tokenIndexesStack.Push(topIndex);
                tagTokenList[i].ChangeTokenType(SecondLevelTokenType.String);
                return;
            }

            tokenIndexesStack.Push(topIndex);
            tokenIndexesStack.Push(i);
        }
    }
}