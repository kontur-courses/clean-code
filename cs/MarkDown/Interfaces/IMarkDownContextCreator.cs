namespace MarkDown.Interfaces;

public interface IMarkDownContextCreator
{
    public IContextInfo GetFilledEntryContext(string mdText);
}