using System.Collections.Generic;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Blocks
{
    public class PlainBlock : IBlock
    {
        private readonly string word;

        public PlainBlock(string word)
        {
            this.word = word;
        }

        public IEnumerable<string> Format(IBlockFormatter _)
        {
            yield return word;
        }
    }
}