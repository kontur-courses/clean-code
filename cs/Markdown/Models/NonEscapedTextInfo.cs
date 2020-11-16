using System.Collections.Generic;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class NonEscapedTextInfo
    {
        public string TextWithoutEscapeSymbols { get; }
        public List<TagInfo> AllTagInfos { get; }
        public List<int> WhiteSpacesPositions { get; }

        public NonEscapedTextInfo(string textWithoutEscapeSymbols,
            List<TagInfo> allTagInfos, List<int> whiteSpacesPositions)
        {
            TextWithoutEscapeSymbols = textWithoutEscapeSymbols;
            AllTagInfos = allTagInfos;
            WhiteSpacesPositions = whiteSpacesPositions;
        }
    }
}
