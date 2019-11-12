using Markdown.Core.Processors;

namespace Markdown.Core
{
    static class Md
    {
        private static readonly BaseParser[] parsers = new BaseParser[]
        {
            new SingleUnderscoreProcessor(),
            new DoubleUnderscoreProcessor()
        };

        public static string Render(string markdown)
        {
            return null;
        }
    }
}