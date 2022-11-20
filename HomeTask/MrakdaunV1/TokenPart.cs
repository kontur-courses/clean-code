using System;
using MrakdaunV1.Enums;

namespace MrakdaunV1
{
    public class TokenPart
    {
        public bool IsInsideTheNumber;
        public TokenPartType Type;
        public TokenPartPositionType PositionType;
        public int Index;

        public TokenPart(string s, int index)
        {
            string sourceString = s + " ";
            
            
            Index = index;

            if (index != sourceString.Length - 1)
                Type = sourceString[index + 1] == '_' ? TokenPartType.Bold : TokenPartType.Italic;


            var spacesCount = index == 0 ? 1 : sourceString[index - 1].IsDelimeter() ? 1 : 0;

            if(index != sourceString.Length - 1)
                spacesCount += sourceString[index + (Type == TokenPartType.Italic ? 1 : 2)].IsDelimeter() ? 2 : 0;

            PositionType = (TokenPartPositionType) spacesCount;

            if (PositionType == TokenPartPositionType.InsideTheWord)
                IsInsideTheNumber =
                    char.IsDigit(sourceString[index - 1])
                    && char.IsDigit(sourceString[index + (Type == TokenPartType.Italic ? 1 : 2)]);
        }

        public override string ToString()
        {
            return $"<{nameof(Type)}:{Enum.GetName(Type)}, " +
                   $"{nameof(PositionType)}:{Enum.GetName(PositionType)}, " +
                   $"{nameof(IsInsideTheNumber)}:{IsInsideTheNumber}, " +
                   $"{nameof(Index)}:{Index}>";
        }
    }
}