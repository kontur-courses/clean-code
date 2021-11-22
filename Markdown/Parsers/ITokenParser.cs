using System.Collections.Generic;

namespace Markdown
{
    internal interface ITokenParser
    {
        // void ParseParagraph(string paragraph);

        // Метод для валидации двух множеств отрезков токенов на пересечение и вложенность
        // возможно, стоит убрать в другое место
        public (SegmentsCollection, SegmentsCollection) IgnoreSegmentsThatDoNotMatchRules(SegmentsCollection first, SegmentsCollection second);

        string ReplaceTokens(string text, SegmentsCollection segments, ITagTranslator translator);

        TokenInfoCollection FindAllTokens(string paragraph);
    }
}