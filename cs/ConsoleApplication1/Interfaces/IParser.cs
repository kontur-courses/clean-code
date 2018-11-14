namespace ConsoleApplication1.Interfaces
{
    public interface IParser
    {
        bool AnyParts();
        TextPart GetNextPart();
    }
}
