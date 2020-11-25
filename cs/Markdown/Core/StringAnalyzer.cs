using System;

namespace Markdown.Core
{
    public class StringAnalyzer
    {
        public string AnalyzedString { get; }

        public StringAnalyzer(string value) => AnalyzedString = value;

        public bool HasValueWhiteSpaceAt(int index) =>
            IsCharInsideValue(index) && char.IsWhiteSpace(AnalyzedString[index]);

        public bool HasValueSelectionPartWordInDifferentWords(int startIndex, int endIndex) =>
            HasValueNonWhiteSpaceAt(startIndex - 1) && HasValueNonWhiteSpaceAt(endIndex + 1) &&
            AnalyzedString.IndexOf(" ", startIndex, endIndex - startIndex, StringComparison.Ordinal) != -1;

        public bool HasValueUnderscoreAt(int index) => IsCharInsideValue(index) && AnalyzedString[index] == '_';

        public bool IsCharInsideValue(int index) => index >= 0 && index < AnalyzedString.Length;

        private bool HasValueNonWhiteSpaceAt(int index) =>
            IsCharInsideValue(index) && !char.IsWhiteSpace(AnalyzedString[index]);
    }
}