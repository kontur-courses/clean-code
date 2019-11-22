using System;
using System.Collections.Generic;
using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor
{
    public static class Markdown
    {
        public const char EscapeCharacter = '\\';

        public static HashSet<char> ServiceSymbols => new HashSet<char> { EscapeCharacter, '_' }; // TODO: for what it?
        public static HashSet<char> WhiteSpaceSymbols => new HashSet<char> { ' ', '\t', '\n' };

        public static Dictionary<ITextWrap, (int openMarkerIndex, int closeMarkerIndex)[]> textWraps =
            new Dictionary<ITextWrap, (int openMarkerIndex, int closeMarkerIndex)[]>();

        public static string RenderHtml(string markdownText)
        {
            (int openMarkerIndex, int closeMarkerIndex)[] underscoresWraps;
            (int openMarkerIndex, int closeMarkerIndex)[] doubleUnderscoresWraps;

            throw new NotImplementedException();
        }
    }
}