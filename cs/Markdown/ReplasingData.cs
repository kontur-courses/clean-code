using System;
using System.Linq.Expressions;

namespace Markdown
{
    public class ReplacingData
    {
        public string Old { get; }
        public string New { get; }

        public ReplacingData(string oldString, string newString)
        {
            Old = oldString;
            New = newString;
        }
    }
}