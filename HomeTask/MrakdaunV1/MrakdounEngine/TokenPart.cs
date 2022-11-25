using System;
using System.ComponentModel.DataAnnotations;
using MrakdaunV1.Enums;

namespace MrakdaunV1.MrakdounEngine
{
    public class TokenPart
    {
        public readonly bool IsInsideTheNumber;
        public readonly TokenPartType Type;
        public readonly TokenPartPositionType PositionType;
        public readonly int Index;
        
        public TokenPart(string sourceString, int index)
        {
            Index = index;

            Type = GetTokenPartType(sourceString, index);
            PositionType = GetPositionType(sourceString, index);

            if (PositionType == TokenPartPositionType.InsideTheWord)
                IsInsideTheNumber =
                    char.IsDigit(sourceString[index - 1])
                    && char.IsDigit(sourceString[index + (Type == TokenPartType.Italic ? 1 : 2)]);
        }
        
        public override string ToString()
        {
            return $"<{nameof(Type)}:{Type:G}, " +
                   $"{nameof(PositionType)}:{PositionType:G}, " +
                   $"{nameof(IsInsideTheNumber)}:{IsInsideTheNumber}, " +
                   $"{nameof(Index)}:{Index}>";
        }
        
        /// <summary>
        /// Метод вернет тип тега в тексте
        /// </summary>
        private TokenPartType GetTokenPartType(string sourceString, int index)
        {
            if (sourceString[index] == '#')
                return TokenPartType.Header1;
            
            if (index != sourceString.Length - 1)
                return sourceString[index + 1] == '_' ? TokenPartType.Bold : TokenPartType.Italic;
            
            throw new ValidationException("Неопределенный токен");
        }

        /// <summary>
        /// Метод вернет тип позиции тега в тексте
        /// </summary>
        private TokenPartPositionType GetPositionType(string sourceString, int index)
        {
            // есть ли пробельный символ перед тегом?
            var spacesCount = index == 0 ? 1 : sourceString[index - 1].IsSpaceChar() ? 1 : 0;

            // есть ли пробельный символ после тега?
            if (index != sourceString.Length - 1)
                spacesCount += sourceString[index + (Type == TokenPartType.Italic ? 1 : 2)].IsSpaceChar() ? 2 : 0;

            return (TokenPartPositionType)spacesCount;
        }
    }
}