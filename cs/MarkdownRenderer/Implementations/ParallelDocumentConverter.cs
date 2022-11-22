using System.Text;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Infrastructure;

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
        var parsedLines = source.EnhancedSplit("\n", "\r\n")
            .AsParallel()
            .AsOrdered()
            .Select(line => (_lineParser.ParseLineContent(line.SplitPart), line.SplitterAfter))
            .Select(line => (_lineRenderer.RenderLine(line.Item1), line.SplitterAfter));

        var result = new StringBuilder();
        foreach (var parsedLine in parsedLines)
            result.Append(parsedLine.Item1).Append(parsedLine.SplitterAfter);

        return result.ToString();
    }
}