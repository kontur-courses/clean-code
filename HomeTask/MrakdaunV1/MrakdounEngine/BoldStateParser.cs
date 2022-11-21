using System;
using System.Collections.Generic;
using System.Linq;
using MrakdaunV1.Enums;

namespace MrakdaunV1.Interfaces
{
    public class BoldStateParser : IStateParser
    {
        public CharState[] GetParsedStates(List<TokenPart> tokenParts, CharState[] charStatesOld, string text)
        {
            var charStates = charStatesOld.Clone() as CharState[];

            if (charStates is null)
                throw new NullReferenceException(nameof(charStates));
            
            List<TokenPart> listOfBoldTokenParts = tokenParts
                .Where(p => p.Type == TokenPartType.Bold)
                .ToList();

            for (int i = 0; i < listOfBoldTokenParts.Count; i++)
            {
                // на одном теге далеко не уедешь, поэтому просто запомним, что он есть
                if (i == 0)
                    continue;

                // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                if ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Special) == CharState.Special)
                    continue;

                // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                var sliceHasDelimeters =
                    text.ContainsDelimeters(
                        listOfBoldTokenParts[i - 1].Index,
                        listOfBoldTokenParts[i].Index);

                var currnetTokPosType = listOfBoldTokenParts[i].PositionType;
                var prevPosType = listOfBoldTokenParts[i - 1].PositionType;


                // Абсолютно конченый IF
                // жир имеет шансы на валидность, если:
                // 1) в нем нет курсива
                // 2) курсив есть, но по бокам курсива точно нет (нет коллизий)
                if (
                    charStates.Skip(listOfBoldTokenParts[i - 1].Index)
                        .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i - 1].Index)
                        .All(cs => (cs & CharState.Italic) != CharState.Italic)
                    || (charStates.Skip(listOfBoldTokenParts[i - 1].Index)
                            .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i - 1].Index)
                            .Any(cs => (cs & CharState.Italic) == CharState.Italic)
                        && (charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) !=
                        CharState.Italic
                        && (charStates[listOfBoldTokenParts[i].Index] & CharState.Italic) !=
                        CharState.Italic))
                {
                    // 4 проверки - 4 условия существования валидной пары тегов
                    // 1 - тег до и после слов, хоть убейся, но будет валиден всегда
                    // 2, 3 и 4 случаи так или иначе задействуют часть в слове, а тут нужно учесть,
                    // что это не должно быть число, а также слово должно быть одно, без разделителей (пробел)
                    if ((currnetTokPosType == TokenPartPositionType.AfterWord
                         && prevPosType == TokenPartPositionType.BeforeWord)
                        || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                            && prevPosType == TokenPartPositionType.BeforeWord
                            && !sliceHasDelimeters
                            && !listOfBoldTokenParts[i].IsInsideTheNumber)
                        || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                            && prevPosType == TokenPartPositionType.InsideTheWord
                            && !sliceHasDelimeters
                            && !listOfBoldTokenParts[i].IsInsideTheNumber
                            && !listOfBoldTokenParts[i - 1].IsInsideTheNumber)
                        || (currnetTokPosType == TokenPartPositionType.AfterWord
                            && prevPosType == TokenPartPositionType.InsideTheWord
                            && !sliceHasDelimeters
                            && !listOfBoldTokenParts[i - 1].IsInsideTheNumber))
                    {
                        for (var c = 0;
                            c < listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i - 1].Index + 2;
                            c++)
                            charStates[c + listOfBoldTokenParts[i - 1].Index] |= CharState.Bold;

                        charStates[listOfBoldTokenParts[i].Index] |= CharState.Special;
                        charStates[listOfBoldTokenParts[i].Index + 1] |= CharState.Special;
                        charStates[listOfBoldTokenParts[i - 1].Index] |= CharState.Special;
                        charStates[listOfBoldTokenParts[i - 1].Index + 1] |= CharState.Special;
                    }
                }
                // если же коллизии есть, то надо игнорить жир + убрать курсивное выделение
                else
                {
                    // если вообще все символы курсивные, то мы имеем дело с жиром внутри курсива, в целом
                    // это валидно, просто игнорим
                    if (
                        charStates.Skip(listOfBoldTokenParts[i - 1].Index)
                            .Take(listOfBoldTokenParts[i].Index - listOfBoldTokenParts[i - 1].Index)
                            .All(cs => (cs & CharState.Italic) == CharState.Italic))
                        continue;

                    /*
                         * Сейчас сложная ситуация - в жирном есть курсив и надо понять,
                         * где пересечения + обнулить его
                         */

                    var numOfCollisions = 0;
                    numOfCollisions += ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) ==
                                        CharState.Italic)
                        ? 1
                        : 0;
                    numOfCollisions += ((charStates[listOfBoldTokenParts[i].Index] & CharState.Italic) ==
                                        CharState.Italic)
                        ? 1
                        : 0;

                    while (numOfCollisions != 0)
                    {
                        var isCollisionFromLeft = ((charStates[listOfBoldTokenParts[i - 1].Index] & CharState.Italic) ==
                                                   CharState.Italic);

                        // Произошло пересечение с левой границей
                        // Запускаем обнулялку влево и вправо
                        var currentIndex = isCollisionFromLeft
                            ? listOfBoldTokenParts[i - 1].Index
                            : listOfBoldTokenParts[i].Index;

                        while ((charStates[currentIndex] & CharState.Special) != CharState.Special)
                        {
                            charStates[currentIndex--] &= ~CharState.Italic;
                        }

                        charStates[currentIndex] &= ~(CharState.Italic | CharState.Special);

                        currentIndex = isCollisionFromLeft ? listOfBoldTokenParts[i - 1].Index : listOfBoldTokenParts[i].Index;
                        while ((charStates[currentIndex] & CharState.Special) != CharState.Special)
                        {
                            charStates[currentIndex++] &= ~CharState.Italic;
                        }

                        charStates[currentIndex] &= ~(CharState.Italic | CharState.Special);

                        numOfCollisions--;
                    }
                }
            }

            return charStates;
        }
    }
}