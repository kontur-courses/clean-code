namespace ConsoleApplication1.Interfaces
{
    public interface IConverter<in TFirst, out TSecond>
    {
        TSecond Convert(TFirst item);
    }
}
