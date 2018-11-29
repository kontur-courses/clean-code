using System.Collections.Generic;

namespace Markdown.Registers
{
    internal class StrongRegister : DelimeterRunRegister
    {
        protected override int DelimLen => 2;
        protected override int Priority => 1;
        protected override string Prefix => "<strong>";
        protected override string Suffix => "</strong>";
        
        public StrongRegister()
        {
            Delimeters = new HashSet<string>(new[] {new string('*', DelimLen), new string('_', DelimLen)});
        }
    }
}