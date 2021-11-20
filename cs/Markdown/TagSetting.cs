namespace Markdown;

public class TagSetting
{
    private record struct VariableDescriptor(string Name, string Start, string End);

    public string MdTag { get; private set; }
    public bool IsLineOnly { get; private set; }

    private readonly string htmlPattern;

    private readonly List<VariableDescriptor> variables = new();

    public TagSetting(string mdTag, string mdPattern, string htmlPattern, bool isLineOnly = false)
    {
        MdTag = mdTag;
        this.htmlPattern = htmlPattern;
        IsLineOnly = isLineOnly;
        ParseVariables(mdPattern);
    }

    public string Render(string text)
    {
        var result = htmlPattern;
        var position = 0;
        foreach (var descriptor in variables)
        {
            var startIndex = text.IndexOf(descriptor.Start, position);
            if (startIndex == -1)
                throw new ArgumentException($"Invalid string format: failed to find start of variable $({descriptor.Name})", nameof(text));
            startIndex += descriptor.Start.Length;

            var endIndex = descriptor.End != string.Empty
                ? text.IndexOf(descriptor.End, startIndex + 1)
                : text.Length;
            if (endIndex == -1)
                return text;

            result = result.Replace($"$({descriptor.Name})", text[startIndex..endIndex]);
            position = endIndex;
        }

        return result;
    }

    private void ParseVariables(string mdPattern)
    {
        for (var i = 0; i < mdPattern.Length; i++)
        {
            var variableStart = mdPattern.IndexOf("$(", i);

            if (variableStart == -1)
                return;
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
            variables.Add(variable);

            i = variableEnd + 1;
        }
    }

    private static VariableDescriptor CreateVariableDescriptor(string mdPattern, int variableStart, int variableStartPosition, int variableEnd, int variableEndPosition)
    {
        var variableStartTag = mdPattern[variableStartPosition..variableStart];
        var variableName = mdPattern[(variableStart + 2)..variableEnd];
        var variableEndTag = mdPattern[(variableEnd + 1)..variableEndPosition];

        return new(variableName, variableStartTag, variableEndTag);
    }
}
