using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace Markdown
{
    public class Md
    {
        private readonly IMdHeuristic[] orderedHeuristics;
        private readonly Stack<HtmlTextWriterTag> openTags = new Stack<HtmlTextWriterTag>();
        private string renderingString;
        
        public Md(IMdHeuristic[] orderedHeuristics)
        {
            this.orderedHeuristics = orderedHeuristics;
        }

        public Md() =>
            new Md(new []
            {
                new MdWrapperHeuristic("__", HtmlTextWriterTag.Strong,this),
                new MdWrapperHeuristic("_", HtmlTextWriterTag.U,this),
            });

        public IEnumerable<HtmlTextWriterTag> OpenTags => openTags;
        
        public string Render(string markdowned)
        {
            throw new NotImplementedException();
        }
    }
}