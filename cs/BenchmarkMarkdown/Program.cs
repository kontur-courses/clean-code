using BenchmarkDotNet.Running;
using BenchmarkMarkdown;

BenchmarkRunner.Run<MarkdownParserBenchmark>();
BenchmarkRunner.Run<MdBenchmark>();