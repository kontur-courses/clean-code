using System.Text;

namespace Markdown;

public class TagSetting
{
    private record struct VariableDescriptor(string Name, string Start, string End);

    public string HtmlTag { get; private set; }
    public string OpeningTag { get; private set; }
    public string EndTag { get; private set; }
    public bool IsLineOnly { get; private set; }
    public IReadOnlySet<string> SpecialParts { get; private set; }

    private readonly string htmlPattern;
    private readonly int nestingLevel;
    private readonly List<VariableDescriptor> variables;

    public TagSetting(string mdTag, string htmlTag, string mdPattern, string htmlPattern, bool isLineOnly = false, int nestingLevel = 0)
    {
        OpeningTag = mdTag;
        HtmlTag = htmlTag;
        this.htmlPattern = htmlPattern;
        IsLineOnly = isLineOnly;
        this.nestingLevel = nestingLevel;
        variables = ParseVariables(mdPattern);
        EndTag = variables.Count > 0 ? variables[^1].End : mdTag;
        SpecialParts = variables.Select(x => x.Start)
            .Concat(variables.Select(x => x.End))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();
    }

    public bool CanBeNestedIn(TagSetting? setting)
    {
        return setting == null || nestingLevel > setting.nestingLevel;
    }

    public StringBuilder Render(string text, IReadOnlySet<string> excludedParts)
    {
        var result = new StringBuilder(htmlPattern);
        var position = 0;
        for (var i = 0; i < variables.Count; i++)
        {
            var descriptor = variables[i];
            var startIndex = FindNext(text, position, descriptor.Start, excludedParts);
            if (startIndex == -1)
                throw new ArgumentException($"Invalid string format: failed to find start of variable $({descriptor.Name})", nameof(text));
            startIndex += descriptor.Start.Length;

            var endIndex = descriptor.End != string.Empty
                ? FindNext(text, startIndex, descriptor.End, excludedParts)
                : text.Length;
            if (endIndex == -1)
                throw new ArgumentException($"Invalid string format: failed to find end of variable $({descriptor.Name})", nameof(text));

            result = result.Replace($"$({descriptor.Name})", text[startIndex..endIndex]);
            position = endIndex;
        }

        return result;
    }

    private static int FindNext(string text, int start, string toFind, IReadOnlySet<string> excludedParts)
    {
        var possibleFind = text.IndexOf(toFind, start);
        while (possibleFind != -1)
        {
            var toSkip = 0;
            foreach (var part in excludedParts)
            {
                if (Tokenizer.IsEscaped(text, possibleFind))
                {
                    toSkip = toFind.Length;
                    break;
                }

                if (text.Length < possibleFind + part.Length)
                    continue;
                if (toFind.Length > part.Length)
                    continue;

                if (excludedParts.Contains(text.Substring(possibleFind, part.Length)))
                {
                    toSkip = part.Length;
                    break;
                }
            }

            if (toSkip == 0)
                break;
            possibleFind = text.IndexOf(toFind, possibleFind + toSkip);
        }

        return possibleFind;
    }

    private List<VariableDescriptor> ParseVariables(string mdPattern)
    {
        var result = new List<VariableDescriptor>();
        for (var i = 0; i < mdPattern.Length; i++)
        {
            var variableStart = mdPattern.IndexOf("$(", i);

            if (variableStart == -1)
                return result;
            var variableEnd = mdPattern.IndexOf(')', variableStart);
            if (variableEnd == -1)
            {
                i = variableStart;
                continue;
            }

            var nextVariable = mdPattern.IndexOf("$(", variableEnd + 1);
            if (nextVariable == -1)
            {
                nextVariable = mdPattern.Length;
            }

            var variable = CreateVariableDescriptor(mdPattern, variableStart, i, variableEnd, nextVariable);
            result.Add(variable);

            i = variableEnd + 1;
        }

        return result;
    }

    private static VariableDescriptor CreateVariableDescriptor(string mdPattern, int variableStart, int variableStartPosition, int variableEnd, int variableEndPosition)
    {
        var variableStartTag = mdPattern[variableStartPosition..variableStart];
        var variableName = mdPattern[(variableStart + 2)..variableEnd];
        var variableEndTag = mdPattern[(variableEnd + 1)..variableEndPosition];

        return new(variableName, variableStartTag, variableEndTag);
    }
}
