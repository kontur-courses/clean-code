using System.Diagnostics;

namespace Markdown_Tests;

internal class Benchmark
{
    public long MeasureMilliseconds(
        Action measuringAction,
        int repetitionsCount)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        measuringAction.Invoke();

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        for (var i = 0; i < repetitionsCount; i++)
        {
            measuringAction.Invoke();
        }

        stopWatch.Stop();

        return stopWatch.ElapsedMilliseconds;
    }
}
