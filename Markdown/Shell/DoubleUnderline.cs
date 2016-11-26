using System;
using System.Collections.Generic;
namespace Markdown.Shell
{
    public class DoubleUnderline : IShell
    {
        private const string Prefix = "__";
        private const string Suffix = "__";
        private readonly List<Type> innerShellsTypes = new List<Type>()
        {
            typeof(SingleUnderline)
        };

        public bool Contains(IShell shell)
        {
            return innerShellsTypes.Contains(shell.GetType());
        }

        private static bool IsIncorrectTagPosition(string text, int startPosition, int endPosition)
        {
            if (text.IsEscapedCharacter(startPosition))
            {
                return true;
            }
            if (text.IsSurroundedByNumbers(startPosition, endPosition + Prefix.Length - 1))
            {
                return true;
            }
            return false;
        }

        public bool TryOpen(string text, int startPrefix, out MatchObject matchObject)
        {
            if (text.HasSpace(startPrefix + Prefix.Length))
            {
                matchObject = null;
                return false;
            }
            return TryMatch(text, Prefix, startPrefix, out matchObject);
        }

        public bool TryClose(string text, int startSuffix, out MatchObject matchObject)
        {
            if (text.HasSpace(startSuffix - 1))
            {
                matchObject = null;
                return false;
            }
            return TryMatch(text, Suffix, startSuffix, out matchObject);
        }

        private static bool TryMatch(string text, string substring, int startPosition, out MatchObject matchObject)
        {
            if (IsIncorrectTagPosition(text, startPosition, startPosition + substring.Length - 1))
            {
                matchObject = null;
                return false;
            }
            return text.TryMatchSubstring(substring, startPosition, out matchObject);
        }
    }
}
