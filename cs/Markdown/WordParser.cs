using System.Collections.Generic;

namespace Markdown
{
    public static class WordParser
    {
        public static TextInfo Parse(string word)
        {
            var underscorePositions = new List<int>();
            for (var i = 0; i < word.Length; i++)
            {
                if (char.IsDigit(word[i]))
                {
                    var textInfo = new TextInfo();
                    textInfo.AddText(word);
                    return textInfo;
                }

                if (word[i] == '_' && word[i - 1] != '\\' && !AdjacentIsUnderscore(i, word))
                    underscorePositions.Add(i);
            }

            return FormatWord(word, underscorePositions);
        }

        private static TextInfo FormatWord(string word, List<int> underscorePositions)
        {
            var textInfo = new TextInfo();

            switch (underscorePositions.Count)
            {
                case 0:
                    textInfo.AddText(word);
                    return textInfo;
                case 1:
                    var (mainTag, contentTag) = underscorePositions[0] > (word.Length - 1) / 2
                        ? (Tag.NoFormatting, Tag.Italic)
                        : (Tag.Italic, Tag.NoFormatting);
                    textInfo.AddContent(new TextInfo(mainTag, word.Substring(0, underscorePositions[0])));
                    textInfo.AddContent(new TextInfo(contentTag, word.Substring(underscorePositions[0] + 1)));
                    return textInfo;
                default:
                    return FormatMultipleUnderscores(word, underscorePositions);
            }

        }

        private static TextInfo FormatMultipleUnderscores(string word, List<int> underscorePositions)
        {
            var textInfo = new TextInfo();
            if (underscorePositions.Count % 2 == 1)
                underscorePositions.RemoveAt(underscorePositions.Count - 1);
            var content = new TextInfo(text: word.Substring(0, underscorePositions[0]));
            textInfo.AddContent(content);
            for (var i = 0; i < underscorePositions.Count - 1; i++)
            {
                var tag = i % 2 == 0 ? Tag.Italic : Tag.NoFormatting;
                var length = underscorePositions[i + 1] - underscorePositions[i] - 1;
                var start = underscorePositions[i] + 1;
                content = new TextInfo(tag, word.Substring(start, length));
                textInfo.AddContent(content);
            }
            content = new TextInfo(text: word.Substring(underscorePositions[^1] + 1));
            textInfo.AddContent(content);
            return textInfo;
        }

        private static bool AdjacentIsUnderscore(int index, string word)
        {
            return index > 0 && word[index - 1] == '_' || index < word.Length - 1 && word[index + 1] == '_';
        }
    }
}