namespace Markdown.Exporter
{
    internal interface IExportable
    {
        string Export(IExporter exporter);
    }
}