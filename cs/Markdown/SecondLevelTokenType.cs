namespace Markdown
{
    public enum SecondLevelTokenType
    {
        String,
        StringWithNumbers,
        Space,
        OpeningItalics, //открывающийся курсив
        ClosingItalics, //закрывающийся курсив
        OpeningBold, //открывающийся полужирный
        ClosingBold, //закрывающийся полужирный
        OpenCloseItalics,
        OpenCloseBold,
        Header, //решётка
        Backslash, //обратный слэш
    }
}