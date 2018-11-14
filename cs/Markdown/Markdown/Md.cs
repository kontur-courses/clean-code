using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly HashSet<Mark> marks = new HashSet<Mark>();

        public Md(params Mark[] remainingMarks)
        {
            foreach (var mark in remainingMarks)
                marks.Add(mark);
        }

        public string Render(string text)
        {
            var tokenParser = new TokenParser(marks);
            var tokens = tokenParser.Parse(text);
            return HtmlBuilder.RemoveRedundantBackSlashes(HtmlBuilder.Build(tokens, text), marks);
        }
    }
}
