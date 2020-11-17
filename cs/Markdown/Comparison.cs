using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Comparison
    {
        private static readonly Dictionary<string, TextSelectionType> _markdownSymbolsAndSelectionTypes =
            new Dictionary<string, TextSelectionType>
            {
                {"_", TextSelectionType.Italic},
                {"__", TextSelectionType.Bold},
                {"#", TextSelectionType.Header},
                {@"\", TextSelectionType.Slash},
                {"-", TextSelectionType.Marker}
            };

        private static readonly Dictionary<TextSelectionType, string[]> _selectionTypesAndHtmlMarks =
            new Dictionary<TextSelectionType, string[]>
            {
                {TextSelectionType.Header, new[] {"<h1>", "</h1>"}},
                {TextSelectionType.Italic, new[] {"<em>", "</em>"}},
                {TextSelectionType.Bold, new[] {"<strong>", "</strong>"}},
                {TextSelectionType.Slash, new[] {""}},
                {TextSelectionType.MarkContainer, new[] {"<ul>\r\n", "\r\n</ul>"}},
                {TextSelectionType.Marker, new[] {" <li>", "</li>"}},
            };

        private static readonly Dictionary<TextSelectionType, HashSet<TextSelectionType>>
            TypeAndCompatibleInternalSelectionTypes
                = new Dictionary<TextSelectionType, HashSet<TextSelectionType>>
                {
                    {
                        TextSelectionType.Header,
                        new HashSet<TextSelectionType> {TextSelectionType.Italic, TextSelectionType.Bold}
                    },
                    {
                        TextSelectionType.Bold,
                        new HashSet<TextSelectionType> {TextSelectionType.Italic}
                    },
                    {
                        TextSelectionType.Italic,
                        new HashSet<TextSelectionType>()
                    },
                    {
                        TextSelectionType.Marker,
                        new HashSet<TextSelectionType> {TextSelectionType.Italic, TextSelectionType.Bold}
                    }
                };

        public static bool IsCompatibleWith(this TextSelectionType externalSelectionType,
            TextSelectionType internalSelectionType)
            => TypeAndCompatibleInternalSelectionTypes[externalSelectionType].Contains(internalSelectionType);

        public static bool IsMarkdownSymbol(string symbol) =>
            _markdownSymbolsAndSelectionTypes.ContainsKey(symbol);

        public static TextSelectionType GetTextSelectionType(string markdownSymbol) =>
            _markdownSymbolsAndSelectionTypes[markdownSymbol];

        public static string GetHtmlSymbol(Tag tag)
        {
            var type = tag.GetType();
            if (type == typeof(StartTag) || type == typeof(SingleTag))
                return _selectionTypesAndHtmlMarks[tag.SelectionType][0];
            if (type == typeof(EndTag))
                return _selectionTypesAndHtmlMarks[tag.SelectionType][1];
            throw new Exception("Doesnt contains html symbol for this markdown tag");
        }
    }
}