namespace Markdown
{
    enum SecondTokenType
    {
        String,
        Space,
        OpeningItalics, //открывающийся курсив
        ClosingItalics, //закрывающийся курсив
        OpeningBold, //открывающийся полужирный
        ClosingBold, //закрывающийся полужирный
        Header, //решётка
        Backslash, //обратный слэш
        NewLine
    }
}