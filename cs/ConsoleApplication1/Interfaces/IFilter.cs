namespace ConsoleApplication1.Interfaces
{
    public interface IFilter<TValue>
    {
        TValue Filter(TValue item);
    }
}
