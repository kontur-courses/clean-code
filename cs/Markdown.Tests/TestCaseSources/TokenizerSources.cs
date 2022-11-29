using System.Collections.Generic;
using Markdown.Primitives;
using NUnit.Framework;

namespace Markdown.Tests.TestCaseSources;

public static class TokenizerSources
{
    public static IEnumerable<TestCaseData> ItalicTestCaseData => GetFormattingTests(Tokens.Italic);

    public static IEnumerable<TestCaseData> BoldTestCaseData => GetFormattingTests(Tokens.Bold);

    public static IEnumerable<TestCaseData> EscapeTestCaseData => GetFormattingTests(Tokens.Escape);

    public static IEnumerable<TestCaseData> Header1TestCaseData()
    {
        var token = Tokens.Header1;
        var value = token.Value;
        var methodName = "{m}";
        yield return new TestCaseData(value, new Token[]
        {
            Tokens.Text(value)
        }).SetName($"{methodName} {value}");
        yield return new TestCaseData($"{value} text", new Token[]
        {
            token,
            Tokens.Text("text"),
        }).SetName($"{methodName} {value} text");
        yield return new TestCaseData($"{value}text", new Token[]
        {
            Tokens.Text($"{value}text"),
        }).SetName($"{methodName} {value}text");
        yield return new TestCaseData($"text{value}", new Token[]
        {
            Tokens.Text($"text{value}"),
        }).SetName($"{methodName} text{value}");
        yield return new TestCaseData($"text\n{value} text", new Token[]
        {
            Tokens.Text("text"),
            Tokens.NewLine,
            Tokens.Header1,
            Tokens.Text("text")
        }).SetName($"{methodName} text\n{value} text");
    }

    private static IEnumerable<TestCaseData> GetFormattingTests(Token token)
    {
        var value = token.Value;
        var methodName = "{m}";
        yield return new TestCaseData(value, new Token[]
        {
            token
        }).SetName($"{methodName} {value}");
        yield return new TestCaseData($"prefix{value}", new Token[]
        {
            Tokens.Text("prefix"),
            token
        }).SetName($"{methodName} prefix{value}");
        yield return new TestCaseData($"{value}suffix", new Token[]
        {
            token,
            Tokens.Text("suffix"),
        }).SetName($"{methodName} {value}suffix");
        yield return new TestCaseData($"{value}text{value}", new Token[]
        {
            token,
            Tokens.Text("text"),
            token,
        }).SetName($"{methodName} {value}text{value}");
    }
}