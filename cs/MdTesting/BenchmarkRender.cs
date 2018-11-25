using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Markdown;

namespace MdTesting
{
    [SimpleJob(RunStrategy.Monitoring, warmupCount: 3, launchCount: 2, id: "MonitoringJob")]
    public class BenchmarkRender
    {
        [Params(800, 2400, 7200, 14400)]
        public int LengthContent;

        [Benchmark]
        public void Test()
        {
            var content = Properties.Resources.FastTest.Substring(0, LengthContent);
            var md = new Md();
            md.Render(content);
        }
    }
}