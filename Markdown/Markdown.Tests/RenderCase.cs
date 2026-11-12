using NUnit.Framework;

namespace Markdown.Tests;

internal static class RenderCase
{
    public static TestCaseData Create(
        string description,
        string input,
        string expected)
    {
        return new TestCaseData(input, expected)
            .SetName(description)
            .SetDescription(description);
    }
}
