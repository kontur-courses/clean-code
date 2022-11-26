namespace Markdown.Interfaces;

public interface ITokenTyper<TType> where TType : Enum
{
    public TType GetSymbolType(int i);
}