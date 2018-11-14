﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public string Name { get; }
        public string Open { get; }
        public string Close { get; }

        public Tag(string name, string open, string close)
        {
            Name = name;
            Open = open;
            Close = close;
        }
    }
}
