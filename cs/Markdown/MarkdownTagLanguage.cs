using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownTagLanguage
    {
        public readonly string SeparatingSymbol;
        public readonly string ScreeningSymbol;
        public readonly Dictionary<int, Dictionary<string, TagSymbolSpecification>> Specifications =
            new Dictionary<int, Dictionary<string, TagSymbolSpecification>>();

        public MarkdownTagLanguage(string separatingSymbol, string screeningSymbol, IEnumerable<TagSymbolSpecification> specifications)
        {
            SeparatingSymbol = separatingSymbol;
            ScreeningSymbol = screeningSymbol;
            foreach (var specification in specifications)
            {
                var rawViewLength = specification.RawView.Length;
                if (!Specifications.ContainsKey(rawViewLength))
                    Specifications.Add(rawViewLength, new Dictionary<string, TagSymbolSpecification>());
                Specifications[rawViewLength][specification.RawView] = specification;
            }
        }

        public IEnumerable<int> GetPossibleTagSymbolsLengths()
        {
            throw new NotImplementedException();
        }
    }
}