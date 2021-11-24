﻿using System;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string mdText)
        {
            return new HtmlConverter().Convert(new Parser()
                .Parse(mdText)
                .ToList());
        }
    }
}
