using System;
using System.Collections.Generic;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Extensions
{
    public static class CharacterTypeExtensions
    {
        private static readonly Dictionary<CharacterType, TextType> Conformity = new Dictionary<CharacterType, TextType>()
        {
            { CharacterType.SimpleCharacter, TextType.SimpleText },
            { CharacterType.SpecialCharacter, TextType.SpecialSymbols },
            { CharacterType.WhiteSpaces, TextType.WhiteSpaces }
        };

        public static TextType BelongsTo(this CharacterType characterType)
        {
            TextType textType;
            if (Conformity.TryGetValue(characterType, out textType))
                return textType;
            throw new ArgumentException($"This {characterType.ToString()} can't be transformed to text type");
        }
    }
}
