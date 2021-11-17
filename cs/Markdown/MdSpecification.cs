using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdSpecification : IMdSpecification
    {
        public Dictionary<string, string> MdToHTML =>
            new Dictionary<string, string>
            {
                {"_", "em"},
                {"__", "strong"},
                {"#", "h1"},
            };
        public Dictionary<string, string> HTMLToMd => MdToHTML.ToDictionary(p => p.Value, p => p.Key);
        public List<string> MdTags => MdToHTML.Keys.ToList();
        public List<string> EscapeSymbols => new List<string> { @"\" };
    }
}
