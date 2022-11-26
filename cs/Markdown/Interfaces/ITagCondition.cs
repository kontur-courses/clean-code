namespace Markdown.Interfaces;

public interface ITagCondition<TType> where TType : Enum
{
    public int GetOpenIndex(TType type);
    public bool GetTagOpenStatus(TType type);
    public void OpenTag(TType type, int index);
    public void CloseTag(TType type);
    public string GetTag(TType type);
}