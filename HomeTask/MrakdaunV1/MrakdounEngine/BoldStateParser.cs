using System;
using System.Collections.Generic;
using System.Linq;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1.MrakdounEngine
{
    public class BoldStateParser : IStateParser
    {
        public CharState[] GetParsedStates(List<TokenPart> tokenParts, CharState[] charStates, string text)
        {
            //var charStates = charStatesOld.Clone() as CharState[];

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

                var currentTokenPart = listOfBoldTokenParts[i];
                var prevTokenPart = listOfBoldTokenParts[i - 1];

                // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                if (charStates[prevTokenPart.Index].HasFlag(CharState.Special | CharState.Bold))
                    continue;

                // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                var sliceHasDelimeters =
                    text.ContainsDelimeters(
                        prevTokenPart.Index, currentTokenPart.Index);

                var sliceContainsItalicButNoCollisions = (AnyStatesBetweenTokenPartsContainsFlag(
                                                              currentTokenPart, prevTokenPart,
                                                              CharState.Italic, charStates)
                                                          && !charStates[prevTokenPart.Index].HasFlag(CharState.Italic)
                                                          && !charStates[currentTokenPart.Index]
                                                              .HasFlag(CharState.Italic));

                var sliceNotContainsItalic = AllStatesBetweenTokenPartsDontContainsFlag(
                    currentTokenPart, prevTokenPart,
                    CharState.Italic, charStates);

                // жир имеет шансы на валидность, если:
                // 1) в нем нет курсива
                // 2) курсив есть, но по бокам курсива нет (нет коллизий)
                if (sliceNotContainsItalic || sliceContainsItalicButNoCollisions)
                {
                    if (!currentTokenPart.IsPossiblePairWith(prevTokenPart, sliceHasDelimeters))
                        continue;

                    for (var c = 0;
                        c < currentTokenPart.Index - prevTokenPart.Index + 2;
                        c++)
                        charStates[c + prevTokenPart.Index] |= CharState.Bold;

                    charStates[currentTokenPart.Index] |= CharState.Special;
                    charStates[currentTokenPart.Index + 1] |= CharState.Special;
                    charStates[prevTokenPart.Index] |= CharState.Special;
                    charStates[prevTokenPart.Index + 1] |= CharState.Special;
                }
                // если же коллизии есть, то надо игнорить жир + убрать курсивное выделение
                else
                {
                    // если вообще все символы курсивные, то мы имеем дело с жиром внутри курсива, в целом
                    // это валидно, просто игнорим
                    if (AllStatesBetweenTokenPartsContainsFlag(
                        currentTokenPart, prevTokenPart,
                        CharState.Italic, charStates))
                        continue;

                    if (charStates[prevTokenPart.Index].HasFlag(CharState.Italic))
                        StartCleaningItalicFromIndex(prevTokenPart.Index, charStates);

                    if (charStates[currentTokenPart.Index].HasFlag(CharState.Italic))
                        StartCleaningItalicFromIndex(currentTokenPart.Index, charStates);
                }
            }

            return charStates;
        }

        private bool AllStatesBetweenTokenPartsContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .All(cs => cs.HasFlag(flag));
        }

        private bool AnyStatesBetweenTokenPartsContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .Any(cs => cs.HasFlag(flag));
        }

        private bool AllStatesBetweenTokenPartsDontContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .All(cs => !cs.HasFlag(flag));
        }

        private void StartCleaningItalicFromIndex(int currentIndex, CharState[] charStates)
        {
            // Очищаем курсив влево
            var counter = currentIndex;
            while (!charStates[counter].HasFlag(CharState.Special))
                charStates[counter--] &= ~CharState.Italic;
            charStates[counter] &= ~(CharState.Italic | CharState.Special);

            // Очищаем курсив вправо
            counter = currentIndex;
            while (!charStates[counter].HasFlag(CharState.Special))
                charStates[counter++] &= ~CharState.Italic;
            charStates[counter] &= ~(CharState.Italic | CharState.Special);
        }
    }
}