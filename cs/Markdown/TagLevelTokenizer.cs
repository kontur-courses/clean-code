using System.Collections.Generic;

namespace Markdown
{
    public class TagLevelTokenizer
    {
        /*Создание нового списка с токенами второго уровня: обрабатка токенов первого уровня и замена их на
         SecondTokenType, 
         получился новый список с Токенами, в котором лежат токены, которые потенциально могут быть тегами
        */
        public List<SecondLevelToken> Tokenize(List<FirstLevelToken> characterTokenList)
        {
            var tagLevelTokenList = new List<SecondLevelToken>();
            var listLength = characterTokenList.Count;
            CheckStringWithNumbersAndChangeType(characterTokenList, listLength);

            for (int i = 0; i < listLength; i++)
            {
                var tokenType = characterTokenList[i].GetFirstTokenType();
                var tokenValue = characterTokenList[i].GetTokenValue();
                switch (tokenType)
                {
                    case FirstLevelTokenType.String:
                        tagLevelTokenList.Add(new SecondLevelToken(tokenValue, SecondLevelTokenType.String));
                        break;
                    case FirstLevelTokenType.StringWithNumbers:
                        tagLevelTokenList.Add(new SecondLevelToken(tokenValue, SecondLevelTokenType.StringWithNumbers));
                        break;
                    case FirstLevelTokenType.Lattice:
                        tagLevelTokenList.Add(new SecondLevelToken("#", SecondLevelTokenType.Header));
                        break;
                    case FirstLevelTokenType.Backslash:
                        i = DoIfTokenIsBackslash(characterTokenList, i, listLength, tagLevelTokenList);
                        continue;
                    case FirstLevelTokenType.Space:
                        tagLevelTokenList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
                        break;
                    default:
                        i = DoIfTokenIsUnderscore(characterTokenList, tokenType, i, tagLevelTokenList, tokenValue,
                            listLength);
                        break;
                }
            }

            return tagLevelTokenList;
        }

        private static int DoIfTokenIsUnderscore(List<FirstLevelToken> characterTokenList,
            FirstLevelTokenType tokenType, int i,
            List<SecondLevelToken> tagLevelTokenList, string tokenValue, int listLength)
        {
            switch (tokenType)
            {
                case FirstLevelTokenType.Underscore when i == 0:
                    i = DoIfFirstTokenIsUnderscore(characterTokenList, tagLevelTokenList, i, tokenValue);
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfIsOpenItalicsInMiddleOFLine(characterTokenList, i, listLength):
                    AddOpenItalicsTypeToken(tagLevelTokenList);
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfOpenCloseItalics(characterTokenList, i, listLength):
                    AddOpenCloseItalicsTypeToken(tagLevelTokenList);
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfCloseItalics(characterTokenList, i, listLength):
                    AddCloseItalicsTypeToken(tagLevelTokenList);
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfOpeningBold(characterTokenList, i, listLength):
                    i += 1;
                    tagLevelTokenList.Add(
                        new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfOpenCloseBold(characterTokenList, i, listLength):
                    i += 1;
                    tagLevelTokenList.Add(
                        new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
                    break;
                case FirstLevelTokenType.Underscore when
                    CheckIfClosingBold(characterTokenList, i, listLength):
                    i += 1;
                    tagLevelTokenList.Add(
                        new SecondLevelToken("__", SecondLevelTokenType.ClosingBold));
                    break;
                case FirstLevelTokenType.Underscore:
                    tagLevelTokenList.Add(
                        new SecondLevelToken(tokenValue, SecondLevelTokenType.String));
                    break;
            }

            return i;
        }

        private static bool CheckIfClosingBold(List<FirstLevelToken> characterTokenList, int i, int listLength)
        {
            return (i + 2 < listLength &&
                    characterTokenList[i - 1].GetFirstTokenType() ==
                    FirstLevelTokenType.String &&
                    characterTokenList[i + 1].GetFirstTokenType() ==
                    FirstLevelTokenType.Underscore &&
                    characterTokenList[i + 2].GetFirstTokenType() ==
                    FirstLevelTokenType.Space) ||
                   (i + 1 < listLength &&
                    characterTokenList[i - 1].GetFirstTokenType() ==
                    FirstLevelTokenType.String &&
                    characterTokenList[i + 1].GetFirstTokenType() ==
                    FirstLevelTokenType.Underscore);
        }

        private static bool CheckIfOpenCloseBold(List<FirstLevelToken> characterTokenList, int i, int listLength)
        {
            return i + 2 < listLength &&
                   characterTokenList[i - 1].GetFirstTokenType() ==
                   FirstLevelTokenType.String &&
                   characterTokenList[i + 1].GetFirstTokenType() ==
                   FirstLevelTokenType.Underscore &&
                   characterTokenList[i + 2].GetFirstTokenType() ==
                   FirstLevelTokenType.String;
        }

        private static bool CheckIfOpeningBold(List<FirstLevelToken> characterTokenList, int i, int listLength)
        {
            return i + 1 < listLength &&
                   characterTokenList[i - 1].GetFirstTokenType() ==
                   FirstLevelTokenType.Space &&
                   characterTokenList[i + 1].GetFirstTokenType() ==
                   FirstLevelTokenType.Underscore;
        }

        private static int DoIfFirstTokenIsUnderscore(List<FirstLevelToken> characterTokenList,
            List<SecondLevelToken> tagLevelTokenList, int i, string tokenValue)
        {
            if (characterTokenList.Count > 1 &&
                characterTokenList[1].GetFirstTokenType() != FirstLevelTokenType.Space &&
                characterTokenList[1].GetFirstTokenType() != FirstLevelTokenType.Underscore)
            {
                tagLevelTokenList.Add(new SecondLevelToken("_",
                    SecondLevelTokenType.OpeningItalics));
            }
            else if (characterTokenList.Count > 2 &&
                     characterTokenList[1].GetFirstTokenType() == FirstLevelTokenType.Underscore &&
                     characterTokenList[2].GetFirstTokenType() != FirstLevelTokenType.Space &&
                     characterTokenList[2].GetFirstTokenType() != FirstLevelTokenType.Underscore)
            {
                i += 1;
                tagLevelTokenList.Add(new SecondLevelToken("__",
                    SecondLevelTokenType.OpeningBold));
            }
            else
            {
                tagLevelTokenList.Add(
                    new SecondLevelToken(tokenValue, SecondLevelTokenType.String));
            }

            return i;
        }

        private static int DoIfTokenIsBackslash(List<FirstLevelToken> characterTokenList, int i, int listLength,
            List<SecondLevelToken> tagLevelTokenList)
        {
            if (i + 1 < listLength)
            {
                if (characterTokenList[i + 1].GetFirstTokenType() == FirstLevelTokenType.Underscore)
                {
                    tagLevelTokenList.Add(
                        new SecondLevelToken("\\", SecondLevelTokenType.Backslash));
                    tagLevelTokenList.Add(new SecondLevelToken("_", SecondLevelTokenType.String));
                    i += 1;
                    return i;
                }
            }

            tagLevelTokenList.Add(new SecondLevelToken("\\", SecondLevelTokenType.String));
            return i;
        }

        private static void AddCloseItalicsTypeToken(List<SecondLevelToken> tagLevelTokenList)
        {
            tagLevelTokenList.Add(
                new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
        }

        private static bool CheckIfCloseItalics(List<FirstLevelToken> characterTokenList, int i, int listLength)
        {
            return (i == listLength - 1 || i + 1 < listLength &&
                       characterTokenList[i + 1].GetFirstTokenType() == FirstLevelTokenType.Space) &&
                   characterTokenList[i - 1].GetFirstTokenType() == FirstLevelTokenType.String;
        }

        private static void AddOpenCloseItalicsTypeToken(List<SecondLevelToken> tagLevelTokenList)
        {
            tagLevelTokenList.Add(
                new SecondLevelToken("_", SecondLevelTokenType.OpenCloseItalics));
        }

        private static bool CheckIfOpenCloseItalics(List<FirstLevelToken> characterTokenList, int i, int listLength)
        {
            return i + 1 < listLength &&
                   characterTokenList[i - 1].GetFirstTokenType() == FirstLevelTokenType.String &&
                   characterTokenList[i + 1].GetFirstTokenType() == FirstLevelTokenType.String;
        }

        private static void AddOpenItalicsTypeToken(List<SecondLevelToken> tagLevelTokenList)
        {
            tagLevelTokenList.Add(
                new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
        }

        private static bool CheckIfIsOpenItalicsInMiddleOFLine(List<FirstLevelToken> characterTokenList, int i,
            int listLength)
        {
            return i + 1 < listLength &&
                   characterTokenList[i - 1].GetFirstTokenType() == FirstLevelTokenType.Space &&
                   characterTokenList[i + 1].GetFirstTokenType() != FirstLevelTokenType.Space &&
                   characterTokenList[i + 1].GetFirstTokenType() != FirstLevelTokenType.Underscore;
        }

        private static void CheckStringWithNumbersAndChangeType(List<FirstLevelToken> characterTokenList,
            int listLength)
        {
            var spaceIndexes = new List<int>();
            for (int i = 0; i < listLength; i++)
            {
                if (characterTokenList[i].GetFirstTokenType() == FirstLevelTokenType.Space)
                {
                    spaceIndexes.Add(i);
                }
            }

            for (int i = 0; i < spaceIndexes.Count - 1; i++)
            {
                var shouldChangeType = false;
                for (int j = spaceIndexes[i] + 1; j < spaceIndexes[i + 1] - 1; j++)
                {
                    if (characterTokenList[i].GetFirstTokenType() != FirstLevelTokenType.StringWithNumbers) continue;
                    shouldChangeType = true;
                    break;
                }

                if (!shouldChangeType) continue;
                for (int j = spaceIndexes[i] + 1; j < spaceIndexes[i + 1] - 1; j++)
                {
                    characterTokenList[i].PutNewFirstLevelType(FirstLevelTokenType.StringWithNumbers);
                }
            }
        }
    }
}