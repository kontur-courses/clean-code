using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class MdSpecification : IMdSpecification
    {
        private readonly List<Tag> _tags;
        
        public Dictionary<string, Tag> TagByMdRepresentation { get; }
        public char EscapeSymbol { get; }
        public Dictionary<string, string> EscapeReplaces { get; }

        public MdSpecification(List<Tag> tags = null, char escapeSymbol = '\\')
        {
            _tags = tags ?? new List<Tag>
            {
                new StrongTag(),
                new EmTag(),
                new HeadingTag()
            };
            EscapeSymbol = escapeSymbol;
            TagByMdRepresentation = _tags
                .ToDictionary(p => p.OpenMdTag, p => p)
                .Union(_tags.ToDictionary(p => p.CloseMdTag, p => p))
                .ToDictionary(p => p.Key, p => p.Value);

            EscapeReplaces = TagByMdRepresentation               
                .Select(p => $"{EscapeSymbol}{p.Key}")
                .ToDictionary(s => s, s => s.Substring(1, s.Length - 1));
        }

        public void CheckMdText(string mdText)
        {
            if (mdText == null)
                throw new ArgumentException("Маркдаун текст не должен быть null");
        }

        public string PreProcess(string mdText)
        {
            var heading = new HeadingTag();

            var open = (mdText.Length - mdText.Replace(heading.OpenMdTag, "").Length) / heading.OpenMdTag.Length;
            var close = (mdText.Length - mdText.Replace(heading.CloseMdTag, "").Length) / heading.CloseMdTag.Length;

            return open > close ? mdText + heading.CloseMdTag : mdText;
        }
    }
}
