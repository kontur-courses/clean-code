using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Interfaces
{
    public interface IDirectionChooser<in TKey>
    {
        Direction GetDirection(TKey leftType, TKey rightType);
    }
}
