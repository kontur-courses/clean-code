using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class TokenHighlighter
{
    public static List<IToken> Excluded { get; } = new();

    private readonly List<ITag> tags = new()
    {
        new StrongTag(),
        new EmTag(),
        new HeaderTag()
    };

    public IEnumerable<IToken> HighlightTokens(string markdownText)
    {
        return GetTokens(markdownText);
    }

    private IEnumerable<IToken> GetTokens(string markdownText)
    {
        var tokens = new List<IToken>();

        var count = 0;
        for (var i = 0; i < markdownText.Length; i++)
        {
            if (tags.Any(tag => tokens.TryAddToken(tag, markdownText, i)))
            {
                if (count != 0)
                    tokens.Insert(tokens.Count - 1,
                        new StringToken(markdownText.Substring(i - count, count)));

                count = 0;

                var added = tokens.Last();
                i += added.Str.Length - 1;
                added.AddInner(GetTokens(added.GetBody()));
            }
            else count++;
        }

        if (count != 0)
            tokens.Add(new StringToken(markdownText.Substring(markdownText.Length - count, count)));

        return tokens;
    }

    // // запихать в token
    // public void RemoveStrongInsideEmTags(ref Dictionary<Type, List<PairTagInfo>> pairTagsIndexes)
    // {
    //     var emTagInfos = pairTagsIndexes[typeof(EmToken)];
    //     var strongTagInfos = pairTagsIndexes[typeof(StrongToken)];
    //
    //     var i = 0;
    //     var j = 0;
    //     while (true)
    //     {
    //         if (i >= emTagInfos.Count) break;
    //         if (j >= strongTagInfos.Count) break;
    //
    //         var emTagInfo = emTagInfos[i];
    //         var strongTagInfo = strongTagInfos[j];
    //
    //         // condition
    //         if (emTagInfo.OpenIdx < strongTagInfo.OpenIdx && emTagInfo.CloseIdx > strongTagInfo.CloseIdx)
    //         {
    //             // action
    //             strongTagInfos.RemoveAt(j);
    //             continue;
    //         }
    //
    //         if (emTagInfo.OpenIdx < strongTagInfo.OpenIdx) i++;
    //         else j++;
    //     }
    // }
}