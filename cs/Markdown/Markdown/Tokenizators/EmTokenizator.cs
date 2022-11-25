namespace Markdown;

public class EmTokenizator : Tokenizator
{
    private HashSet<int> usedIndexes;
    private List<TagToken> result;
    public string OpenTag => MarkdownEmTag.OpenMdTag;
    public string CloseTag => MarkdownEmTag.CloseMdTag;

    public List<TagToken> Tokenize(string mdstring)
    {
        result = new();
        usedIndexes = new();
        var words = mdstring.Split();
        var startIndex = 0;
        foreach (var word in words)
        {
            GetTokensFromWord(word, startIndex);
            startIndex += word.Length + 1;
        }

        GetAnotherTokens(mdstring);
        return result;
    }

    public void GetTokensFromWord(string word, int startIndex)
    {
        if (!word.Contains("_"))
            return;
        int i = 0, j;
        while (true)
        {
            i = word.IndexOf(OpenTag, i);
            if (i == -1)
                break;
            j = word.IndexOf(CloseTag, i + 1);
            if (j == -1)
                break;
            if (j - i == 1)
            {
                i = j;
                continue;
            }

            result.Add(new TagToken(
                startIndex + i,
                startIndex + j + CloseTag.Length - 1,
                new EmTag()));
            UpdateUsedIndexes(startIndex + i, startIndex + j);
            i = j + 1;
        }
    }

    public void GetAnotherTokens(string mdstring)
    {
        int i = mdstring.IndexOf(" " + OpenTag, 0), j;
        while (true)
        {
            while (usedIndexes.Contains(i + 1) && i != -1)
            {
                i = mdstring.IndexOf(OpenTag, i);
            }

            if (i == -1)
                break;

            j = mdstring.IndexOf(CloseTag + " ", i + 1);
            while (usedIndexes.Contains(j) && j != -1)
            {
                j = mdstring.IndexOf(OpenTag, j + 1);
            }

            if (j == -1)
                break;

            if (j - i == 1)
            {
                i = j;
                continue;
            }

            result.Add(new TagToken(
                i + 1,
                j + CloseTag.Length,
                new EmTag()));
            i = j + 1;
        }
    }

    public void UpdateUsedIndexes(int openTagIndex, int closeTagIndex)
    {
        for (int i = 0; i < OpenTag.Length; i++)
            usedIndexes.Add(openTagIndex + i);
        for (int i = 0; i < OpenTag.Length; i++)
            usedIndexes.Add(closeTagIndex + i);
    }
}