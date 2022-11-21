using System;
using System.Collections.Generic;
using System.Linq;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1
{
    public class ItalicStateParser : IStateParser
    {
        public CharState[] GetParsedStates(List<TokenPart> tokenParts, CharState[] charStatesOld, string text)
        {
            var charStates = charStatesOld.Clone() as CharState[];

            if (charStates is null)
                throw new NullReferenceException(nameof(charStates));
            
            List<TokenPart> listOfItalicTokenParts = tokenParts
                .Where(p => p.Type == TokenPartType.Italic)
                .ToList();

            for (int i = 0; i < listOfItalicTokenParts.Count; i++)
            {
                // на одном теге далеко не уедешь, поэтому просто запомним, что он есть
                if (i == 0)
                    continue;

                // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                if ((charStates[listOfItalicTokenParts[i - 1].Index] & CharState.Special) == CharState.Special)
                    continue;

                // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                var sliceHasDelimeters =
                    text.ContainsDelimeters(
                        listOfItalicTokenParts[i - 1].Index,
                        listOfItalicTokenParts[i].Index);


                var currnetTokPosType = listOfItalicTokenParts[i].PositionType;
                var prevPosType = listOfItalicTokenParts[i - 1].PositionType;

                // 4 проверки - 4 условия существования валидной пары тегов
                // 1 - тег до и после слов, хоть убейся, но будет валиден всегда
                // 2, 3 и 4 случаи так или иначе задействуют часть в слове, а тут нужно учесть,
                // что это не должно быть число, а также слово должно быть одно, без разделителей (пробел)
                if ((currnetTokPosType == TokenPartPositionType.AfterWord
                     && prevPosType == TokenPartPositionType.BeforeWord)
                    || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                        && prevPosType == TokenPartPositionType.BeforeWord
                        && !sliceHasDelimeters
                        && !listOfItalicTokenParts[i].IsInsideTheNumber)
                    || (currnetTokPosType == TokenPartPositionType.InsideTheWord
                        && prevPosType == TokenPartPositionType.InsideTheWord
                        && !sliceHasDelimeters
                        && !listOfItalicTokenParts[i].IsInsideTheNumber
                        && !listOfItalicTokenParts[i - 1].IsInsideTheNumber)
                    || (currnetTokPosType == TokenPartPositionType.AfterWord
                        && prevPosType == TokenPartPositionType.InsideTheWord
                        && !sliceHasDelimeters
                        && !listOfItalicTokenParts[i - 1].IsInsideTheNumber))
                {
                    // помечаем все символы флагами курсива + спецсимволы также пометим
                    for (var c = 0;
                        c < listOfItalicTokenParts[i].Index - listOfItalicTokenParts[i - 1].Index + 1;
                        c++)
                        charStates[c + listOfItalicTokenParts[i - 1].Index] |= CharState.Italic;

                    charStates[listOfItalicTokenParts[i].Index] |= CharState.Special;
                    charStates[listOfItalicTokenParts[i - 1].Index] |= CharState.Special;
                }
            }

            return charStates;
        }
    }
}