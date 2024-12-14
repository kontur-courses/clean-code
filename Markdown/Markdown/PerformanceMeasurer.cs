using System.Diagnostics;

namespace Markdown;

public class PerformanceMeasurer(Action<string> logAction)
{
    public long MeasureAverageTime(Action action, int times)
    {
        var measures = new List<long>();
        var stopwatch = new Stopwatch();
        for (var i = 0; i < times; i++)
        {
            stopwatch.Start();
            action();
            stopwatch.Stop();
            measures.Add(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
        }
        var time = (long)Math.Round(measures.Average());
        logAction($"Average time in ms: {time}");
        return time;
    }
}