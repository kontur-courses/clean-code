using System;
using System.Collections.Generic;
using System.Linq;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1.MrakdounEngine
{
    public class ItalicStateParser : IStateParser
    {
        public CharState[] GetParsedStates(List<TokenPart> tokenParts, CharState[] charStates, string text)
        {
            //var charStates = charStatesOld.Clone() as CharState[];

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

                var currentTokenPart = listOfItalicTokenParts[i];
                var prevTokenPart = listOfItalicTokenParts[i - 1];

                // если предыдущий тег был использован в паре с предпредыдущим, то его использовать не надо
                if (charStates[prevTokenPart.Index].HasFlag(CharState.Special | CharState.Italic))
                    continue;

                // есть ли разделители (пробелы) между текущим и предыдущим тегами?
                var sliceHasDelimeters =
                    text.ContainsDelimeters(prevTokenPart.Index, currentTokenPart.Index);

                if (!currentTokenPart.IsPossiblePairWith(prevTokenPart, sliceHasDelimeters))
                    continue;

                // помечаем все символы флагами курсива + спецсимволы также пометим
                for (var c = 0;
                    c < currentTokenPart.Index - prevTokenPart.Index + 1;
                    c++)
                    charStates[c + prevTokenPart.Index] |= CharState.Italic;

                charStates[currentTokenPart.Index] |= CharState.Special;
                charStates[prevTokenPart.Index] |= CharState.Special;
            }

            return charStates;
        }
    }
}