using System.Collections.Generic;
using Markdown;
using Markdown.Engine.Tokens;
using NUnit.Framework;

namespace MarkdownTest.ReducerTest
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
                Token.Strong, TokenText.FromText("__")
            }).SetName("Escape shield token");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, TokenText.FromText("text")
            }, new List<IToken>
            {
                TokenText.FromText("\\text")
            }).SetName("Escape before text");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, Token.Italics, TokenText.FromText("text"), 
                Token.Escape, Token.Italics
            }, new List<IToken>
            {
                TokenText.FromText("_"), TokenText.FromText("text"), TokenText.FromText("_")
            }).SetName("Escape before markup symbol");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Escape, Token.Escape, Token.Italics, 
                TokenText.FromText("text"), Token.Italics
            }, new List<IToken>
            {
                TokenText.FromText("\\"), Token.Italics, TokenText.FromText("text"), Token.Italics
            }).SetName("Shield escape before markup symbol");
        }

        public static IEnumerable<TestCaseData> Headers()
        {
            yield return new TestCaseData(new List<IToken>
            {
                TokenText.FromText("t"), Token.Header1, TokenText.FromText("t")
            }, new List<IToken>
            {
                TokenText.FromText("t"), TokenText.FromText("# "), TokenText.FromText("t")
            }).SetName("Header not at the beginning");

            yield return new TestCaseData(new List<IToken>
            {
                Token.Header1, Token.Header1, TokenText.FromText("t")
            }, new List<IToken>
            {
                Token.Header1, TokenText.FromText("# "), TokenText.FromText("t")
            }).SetName("Two headers");
        }
    }
}