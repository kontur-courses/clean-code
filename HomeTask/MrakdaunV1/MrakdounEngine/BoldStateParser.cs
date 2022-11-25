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
            if (charStates is null)
                throw new NullReferenceException(nameof(charStates));

            // из всех тегов берем только жирные
            List<TokenPart> listOfBoldTokenParts = tokenParts
                .Where(p => p.Type == TokenPartType.Bold)
                .ToList();

            for (int i = 0; i < listOfBoldTokenParts.Count; i++)
            {
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

                // жирный тег может существовать только в таких случаях:
                // 1) в нем нет курсива
                // 2) курсив есть, но по бокам курсива нет (нет коллизий)
                if (sliceNotContainsItalic || sliceContainsItalicButNoCollisions)
                {
                    if (!currentTokenPart.IsPossiblePairWith(prevTokenPart, sliceHasDelimeters))
                        continue;

                    // помечаем весь текст как жирный + спецсимволы на теги тоже проставим
                    for (var c = 0;
                        c < currentTokenPart.Index - prevTokenPart.Index + 2;
                        c++)
                        charStates[c + prevTokenPart.Index] |= CharState.Bold;

                    charStates[currentTokenPart.Index] |= CharState.Special;
                    charStates[currentTokenPart.Index + 1] |= CharState.Special;
                    charStates[prevTokenPart.Index] |= CharState.Special;
                    charStates[prevTokenPart.Index + 1] |= CharState.Special;
                }
                else
                {
                    // если вообще все символы курсивные, то мы имеем дело
                    // с жиром внутри курсива, такое поведение допустимо, курсив
                    // удалять не нужно
                    if (AllStatesBetweenTokenPartsContainsFlag(
                        currentTokenPart, prevTokenPart,
                        CharState.Italic, charStates))
                        continue;

                    
                    // удаляем курсив в зависимости от того, есть ли коллизия слева или справа
                    if (charStates[prevTokenPart.Index].HasFlag(CharState.Italic))
                        StartCleaningItalicFromIndex(prevTokenPart.Index, charStates);

                    if (charStates[currentTokenPart.Index].HasFlag(CharState.Italic))
                        StartCleaningItalicFromIndex(currentTokenPart.Index, charStates);
                }
            }

            return charStates;
        }

        /// <summary>
        /// Метод вернет true только если все символы между тегами имеют нужный стейт
        /// </summary>
        private bool AllStatesBetweenTokenPartsContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .All(cs => cs.HasFlag(flag));
        }

        /// <summary>
        /// Метод вернет true только если хоть один символ между тегами имеет нужный стейт
        /// </summary>
        private bool AnyStatesBetweenTokenPartsContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .Any(cs => cs.HasFlag(flag));
        }

        /// <summary>
        /// Метод вернет true только если все символы между тегами НЕ имеют нужного стейта
        /// </summary>
        private bool AllStatesBetweenTokenPartsDontContainsFlag(
            TokenPart current,
            TokenPart prev,
            CharState flag, CharState[] charStates)
        {
            return charStates.Skip(prev.Index)
                .Take(current.Index - prev.Index)
                .All(cs => !cs.HasFlag(flag));
        }

        /// <summary>
        /// Метод убирает стейт курсива
        /// </summary>
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