using System;

namespace Markdown.BasicTextTokenizer
{
    public interface ITagClassifier
    {
        bool IsOpeningSequence(string text, int position);
        bool IsClosingSequence(string text, int position);
        int Priority { get; }
        FormattedTokenType Type { get; }
        Type[] AllowedSubClassifiers { get; }
        int TagLength { get; }
        bool HasSecondPart { get; }
        bool HasFirstPart { get; }
        Type SecondPartType { get; }
    }
}
