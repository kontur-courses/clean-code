using System;
using System.Collections.Generic;

namespace Markdown
{
    internal static class Syntax
    {
        public enum AttributeType
        {
            Strong,
            Emphasis
        }

        public static readonly Dictionary<AttributeType, string> HtmlTagDictionary;

        public static readonly Dictionary<char, AttributeType> TypeDictionary;


        static Syntax()
        {
            TypeDictionary = new Dictionary<char, AttributeType>()
            {
                {'_', AttributeType.Emphasis}
            };

            HtmlTagDictionary = new Dictionary<AttributeType, string>
            {
                {AttributeType.Emphasis, "em"},
                {AttributeType.Strong, "strong"}
            };
        }

        public static bool CheckIfAttributeIsEnding(string source, int position)
        {
            throw new NotImplementedException();
        }

        public static bool IsEscapeCharacter(string source, int charPosition)
        {
            throw new NotImplementedException();
        }
    }
}