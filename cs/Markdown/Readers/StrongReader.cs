﻿using System.Collections.Generic;

namespace Markdown.Readers
{
    public class StrongReader : TagReader
    {
        public static  IEnumerable<IReader> Readers;
        public static  IEnumerable<TagReader> SkippedReaders;

        public StrongReader(string mdTag) : base(mdTag, Readers, SkippedReaders)
        {
        }
    }
}
