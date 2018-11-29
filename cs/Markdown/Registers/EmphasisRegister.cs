using System.Collections.Generic;

namespace Markdown.Registers
{
    internal class EmphasisRegister : DelimeterRunRegister
    {
        protected override int DelimLen => 1;
        protected override int Priority => 0;
        protected override string Prefix => "<em>";
        protected override string Suffix => "</em>";

        public EmphasisRegister()
        {
            Delimeters = new HashSet<string>(new[] {new string('*', DelimLen), new string('_', DelimLen)});
        }
    }
}