namespace Markdown;

public class TagSetting
{
    private record struct VariableDescriptor(string Name, string Start, string End);

    public string MdTag { get; private set; }

    private readonly string htmlPattern;

    private readonly List<VariableDescriptor> variables = new();

    public TagSetting(string mdTag, string mdPattern, string htmlPattern)
    {
        MdTag = mdTag;
        this.htmlPattern = htmlPattern;
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
                throw new ArgumentException("Invalid string format", nameof(text));
            startIndex += descriptor.Start.Length;

            var endIndex = descriptor.End != string.Empty
                ? text.IndexOf(descriptor.End, startIndex + 1)
                : text.Length;
            if (endIndex == -1)
                throw new ArgumentException("Invalid string format", nameof(text));

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

            var variable = CreateVariableDescriptor(mdPattern, variableStart, variableEnd, out i);
            variables.Add(variable);
        }
    }

    private static VariableDescriptor CreateVariableDescriptor(string mdPattern, int variableStart, int variableEnd, out int nextPosition)
    {
        var vairableStartTag = mdPattern[..variableStart];
        var variableName = mdPattern[(variableStart + 2)..variableEnd];
        var nextVariable = mdPattern.IndexOf("$(", variableEnd + 1);
        string variableEndTag;
        if (nextVariable == -1)
        {
            variableEndTag = mdPattern[(variableEnd + 1)..];
            nextPosition = mdPattern.Length;
        }
        else
        {
            variableEndTag = mdPattern[variableEnd..(nextVariable - 1)];
            nextPosition = nextVariable;
        }

        return new(variableName, vairableStartTag, variableEndTag);
    }
}
