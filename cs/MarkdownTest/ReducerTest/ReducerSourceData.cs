using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    public static class ReducerSourceData
    {
        public static IEnumerable<TestCaseData> Escape()
        {
            yield return new TestCaseData(new List<IToken>
            {
                Token.Strong, Token.Escape, Token.Strong
            }, new List<IToken>
            {
                Token.Strong, Token.FromText("__")
            }).SetName("Escape shield token");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, Token.FromText("text")
            }, new List<IToken>
            {
                Token.FromText("\\text")
            }).SetName("Escape before text");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, Token.Italics, Token.FromText("text"), Token.Escape, Token.Italics
            }, new List<IToken>
            {
                Token.FromText("_"), Token.FromText("text"), Token.FromText("_")
            }).SetName("Escape before markup symbol");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, Token.Escape, Token.Italics, Token.FromText("text"), Token.Italics
            }, new List<IToken>
            {
                Token.FromText("\\"), Token.Italics, Token.FromText("text"), Token.Italics
            }).SetName("Shield escape before markup symbol");
        }

        public static IEnumerable<TestCaseData> Headers()
        {
            yield return new TestCaseData(new List<IToken>
            {
                Token.FromText("t"), Token.Header1, Token.FromText("t")
            }, new List<IToken>
            {
                Token.FromText("t"), Token.FromText("# "), Token.FromText("t")
            }).SetName("Header not at the beginning");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Header1, Token.Header1, Token.FromText("t")
            }, new List<IToken>
            {
                Token.Header1, Token.FromText("# "), Token.FromText("t")
            }).SetName("Two headers");
        }
    }
}