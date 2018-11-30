namespace Markdown
{
    interface IConverter<T, P>
    {
        T Convert(P markdown);
    }
}
