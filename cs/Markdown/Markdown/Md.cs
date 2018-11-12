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

        public Md(Mark firstMark, params Mark[] remainingMarks)
        {
            marks.Add(firstMark);
            foreach (var mark in remainingMarks)
                marks.Add(mark);
        }

        public string Render(string text)
        {
            foreach (var mark in marks)
            {
                text = HtmlBuilder.Build(TokenParser.ParseByOne(text, mark));
            }
            text = HtmlBuilder.RemoveRedundantBackSlashes(text, marks);
            return text;
        }
    }
}
