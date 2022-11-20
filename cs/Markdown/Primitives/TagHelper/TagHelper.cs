using Markdown.Primitives.TokenHelper;
using System.Collections.ObjectModel;

namespace Markdown.Primitives.TagHelper
{
    public static class TagHelper
    {
        public static IReadOnlyDictionary<TokenTypes, TagTypes> ComparisonsDict { get; }

        static TagHelper()
        {
            ComparisonsDict = new ReadOnlyDictionary<TokenTypes, TagTypes>(
                new Dictionary<TokenTypes, TagTypes>()
                {
                    [TokenTypes.Bold] = TagTypes.Bold,
                    [TokenTypes.Italic] = TagTypes.Italic,
                    [TokenTypes.Header] = TagTypes.Header
                });
        }
    }
}
