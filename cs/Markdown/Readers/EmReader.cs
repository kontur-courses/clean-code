using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class EmReader : TagReader
        
    {
        private static readonly IEnumerable<IReader> readers = new IReader[]{new SlashReader(), new CharReader()};
        private static readonly IEnumerable<TagReader> skippedReaders = new TagReader[] { new StrongReader()};

        public EmReader() : base("_", "<em>", readers, skippedReaders)
        {
        }
    }
}
