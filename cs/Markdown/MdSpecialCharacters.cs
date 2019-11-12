using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class MdSpecialCharacters
    {
        public const char Escape = ',';

        public static string RemoveAllEscapeCharacterEntries(string text)
        {
            if (text == null)
                throw new ArgumentNullException();
            foreach (var i in FindAllEscapeSymbolsToBeRemovedInReverseOrder(text))
                text = text.Remove(i, 1);
            return text;
        }

        public static bool IsCharacterEscapedByEscapeCharacter(int characterIndex, string str) =>
            (characterIndex + 1 < str.Length) && str[characterIndex + 1] == Escape;

        private static int[] FindAllEscapeSymbolsToBeRemovedInReverseOrder(string text)
        {
            var index = -1;
            var startIndex = 0;
            var indexesToRemove = new Stack<int>();
            while ((index = text.IndexOf(Escape, startIndex)) != -1)
            {
                indexesToRemove.Push(index);
                startIndex = index + 1;
                if (IsCharacterEscapedByEscapeCharacter(index, text))
                    startIndex++;
            }
            return indexesToRemove.ToArray();
        }
    }
}