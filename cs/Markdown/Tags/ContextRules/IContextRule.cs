using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags.ContextRules
{
    public interface IContextRule
    {
        public bool IsContextCorrect(ReadOnlySpan<char> context, int currentPosition, string tag) => true;

        public bool IsContextEnd(ReadOnlySpan<char> context, int currentPosition, string tag) => false;
    }
}
