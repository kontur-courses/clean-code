namespace Markdown;

public class EmTokenizator : Tokenizator
{
    public string OpenTag => MarkdownEmTag.OpenMdTag;
    public string CloseTag => MarkdownEmTag.CloseMdTag;

    public List<TagToken> Tokenize(string mdstring)
    {
        List<TagToken> result = new();
        HashSet<int> usedIndexes = new();
        var words = mdstring.Split();
        foreach (var word in words)
        {
            if (!word.Contains("_"))
                continue;
            int i = 0, j;
            while (true)
            {
                i = mdstring.IndexOf(OpenTag, i);
                if (i == -1)
                    break;
                j = mdstring.IndexOf(CloseTag, i + 1);
                if (j == -1)
                    break;
                result.Add(new TagToken(
                    i,
                    j + CloseTag.Length - 1,
                    new EmTag()));
                usedIndexes.Add(i);
                usedIndexes.Add(j);
                i = j + 1;
            }
        }

        int k = mdstring.IndexOf(" " + OpenTag, 0), l;
        while (true)
        {
            while (usedIndexes.Contains(k)
                   && k != -1)
            {
                k = mdstring.IndexOf(OpenTag, k);
            }

            if (k == -1)
                break;

            l = mdstring.IndexOf(CloseTag + " ", k + 1);
            while (usedIndexes.Contains(l)
                   && l != -1)
            {
                l = mdstring.IndexOf(OpenTag, l);
            }

            if (l == -1)
                break;

            result.Add(new TagToken(
                k + 1,
                l + CloseTag.Length,
                new EmTag()));
            k = l + 1;
        }

        return result;
    }
}