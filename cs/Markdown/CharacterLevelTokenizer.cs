using System.Collections.Generic;

namespace Markdown
{
    public class CharacterLevelTokenizer
    {
        /*Определение токенов первого уровня из текста: Пробел, нижнее подчёркивание, решётка, обратный слеш, новая строка, а всё остальное - строки
Формируется список, в котором лежат Token: значение строки и её тип, если склеить все строки по порядку, то получится исходный текст.
*/
        public List<FirstLevelToken> Tokenize(string inputString)
        {
            var characterTokenList = new List<FirstLevelToken>();
            var currentIndex = 0;
            var startWordIndex = 0;
            var stringLength = inputString.Length;
            while (currentIndex < stringLength)
            {
                var currentSymbol = inputString[currentIndex];
                if (currentSymbol == ' ')
                {
                    startWordIndex = CheckIfWordBefore(inputString, startWordIndex, currentIndex, characterTokenList);
                    characterTokenList.Add(
                        new FirstLevelToken(" ", FirstLevelTokenType.Space));
                    startWordIndex += 1;
                }
                else if (currentSymbol == '#')
                {
                    startWordIndex = CheckIfWordBefore(inputString, startWordIndex, currentIndex, characterTokenList);
                    characterTokenList.Add(
                        new FirstLevelToken("#", FirstLevelTokenType.Lattice));
                    startWordIndex += 1;
                }
                else if (currentSymbol == '_')
                {
                    startWordIndex = CheckIfWordBefore(inputString, startWordIndex, currentIndex, characterTokenList);
                    characterTokenList.Add(
                        new FirstLevelToken("_", FirstLevelTokenType.Underscore));
                    startWordIndex += 1;
                }
                else if (currentSymbol == '\\')
                {
                    startWordIndex = CheckIfWordBefore(inputString, startWordIndex, currentIndex, characterTokenList);
                    characterTokenList.Add(
                        new FirstLevelToken("\\", FirstLevelTokenType.Backslash));
                    startWordIndex += 1;
                }

                CheckIfLastIndexAndAddLastString(inputString, currentIndex, stringLength, startWordIndex, characterTokenList);

                currentIndex += 1;
            }

            return characterTokenList;
        }

        private static void CheckIfLastIndexAndAddLastString(string inputString, int currentIndex, int stringLength,
            int startWordIndex, List<FirstLevelToken> characterTokenList)
        {
            if (currentIndex == stringLength - 1)
            {
                var substring = inputString.Substring(startWordIndex,
                    currentIndex - startWordIndex + 1);
                if (substring.Contains('1') || substring.Contains('2') || substring.Contains('3') ||
                    substring.Contains('4') || substring.Contains('5') || substring.Contains('6') ||
                    substring.Contains('7') || substring.Contains('8') || substring.Contains('9') ||
                    substring.Contains('0'))
                {
                    characterTokenList.Add(new FirstLevelToken(substring, FirstLevelTokenType.StringWithNumbers));
                }
                else
                {
                    characterTokenList.Add(
                        new FirstLevelToken(substring, FirstLevelTokenType.String));
                }
            }
        }

        private static int CheckIfWordBefore(string inputString, int startWordIndex, int currentIndex,
            List<FirstLevelToken> characterTokenList)
        {
            if (startWordIndex != currentIndex)
            {
                var substring = inputString.Substring(startWordIndex,
                    currentIndex - startWordIndex);
                if (substring.Contains('1') || substring.Contains('2') || substring.Contains('3') ||
                    substring.Contains('4') || substring.Contains('5') || substring.Contains('6') ||
                    substring.Contains('7') || substring.Contains('8') || substring.Contains('9') ||
                    substring.Contains('0'))
                {
                    characterTokenList.Add(new FirstLevelToken(substring, FirstLevelTokenType.StringWithNumbers));
                    startWordIndex = currentIndex;
                }
                else
                {
                    characterTokenList.Add(new FirstLevelToken(substring, FirstLevelTokenType.String));
                    startWordIndex = currentIndex;
                }
            }

            return startWordIndex;
        }
    }
}