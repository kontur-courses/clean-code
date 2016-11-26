using System;
using System.Collections.Generic;

namespace Markdown.Shell
{
    class UrlShell : IShell
    {
        private const string Prefix = "[";
        private readonly List<Type> innerShellsTypes = new List<Type>();
        

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
            return text.IsSurroundedByNumbers(startPosition, endPosition + Prefix.Length - 1);
        }

        public bool TryOpen(string text, int startPrefix, out MatchObject matchObject)
        {
            if (IsIncorrectTagPosition(text, startPrefix, startPrefix + Prefix.Length - 1))
            {
                matchObject = null;
                return false;
            }
            return text.TryMatchSubstring(Prefix, startPrefix, out matchObject);
        }

        public bool TryClose(string text, int startSuffix, out MatchObject matchObject)
        {
            matchObject = null;
            if (!"](".IsSubstring(text, startSuffix))
            {
                return false;
            }
            for (var i = startSuffix + 3; i < text.Length; i++)
            {
                if (text[i] == ')' && !text.IsEscapedCharacter(i))
                {
                    var attributeText = text.Substring(startSuffix + 2, i - startSuffix - 2).RemoveEscapeСharacters();
                    var attribute = new Attribute(attributeText, AttributeType.Url);
                    matchObject = new MatchObject(startSuffix, i, new List<Attribute> {attribute});
                    return true;
                }
            }
            return false;
        }
    }
}
