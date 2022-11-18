namespace Markdown.States;

public enum ProcessState
{
    ReadDocument,
    ReadPlainText,
    ReadItalicText,
    EndReadPlainText,
    EndReadItalicText,
    EndReadParagraph,
    ReadParagraph,
    ReadBoldText,
    EndReadBoldText,
    EndReadDocument,
    ReadHeader,
    EndReadHeader
}