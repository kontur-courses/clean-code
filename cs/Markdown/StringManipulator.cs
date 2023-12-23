namespace Markdown;

public class StringManipulator
{
    public string OldString { get; }
    private int EndInnerString { get; set; }
    private int StartInnerString { get; set; }
    public string Content { get; private set; }

    public StringManipulator(string content, string oldString, int startInnerString, int endInnerString)
    {
        if (startInnerString < 0 || content.Length <= startInnerString)
            throw new ArgumentOutOfRangeException(nameof(startInnerString));

        if (endInnerString < 0 || content.Length <= endInnerString || endInnerString < startInnerString)
            throw new ArgumentOutOfRangeException(nameof(endInnerString));

        Content = content;
        OldString = oldString;
        StartInnerString = startInnerString;
        EndInnerString = endInnerString;
    }

    public string GetInnerString() => Content.Substring(StartInnerString, EndInnerString - StartInnerString);

    public void ReplaceInnerString(string text)
    {
        Content = Content[..StartInnerString] + text + Content[EndInnerString..];
        EndInnerString += text.Length - (EndInnerString - StartInnerString);
    }

    public static StringManipulator Default(string text) => new(text, text, 0, text.Length);
}