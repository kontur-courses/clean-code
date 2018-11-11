using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Readers
{
    class StrongReader : TagReader
    {
        private static readonly IEnumerable<IReader> readers = new IReader[] {new SlashReader(), new EmReader(), new CharReader() };
        private static readonly IEnumerable<TagReader> skippedReaders = new TagReader[0];

        public StrongReader() : base("__", "<strong>", readers, skippedReaders)
        {
        }
    }
}
