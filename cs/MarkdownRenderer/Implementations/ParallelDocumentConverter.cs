using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations;

public class ParallelDocumentConverter : IDocumentsConverter
{
    private readonly ILineParser _lineParser;
    private readonly ILineRenderer _lineRenderer;


    public ParallelDocumentConverter(ILineParser lineParser, ILineRenderer lineRenderer)
    {
        _lineParser = lineParser;
        _lineRenderer = lineRenderer;
    }

    public string Convert(string source)
    {
        var parsedLines = source.Split("\n")
            .AsParallel()
            .AsOrdered()
            .Select(_lineParser.ParseContentLine)
            .Select(_lineRenderer.RenderLine);

        return string.Join("\n", parsedLines);
    }
}