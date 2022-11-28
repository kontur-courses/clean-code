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
        if (!(word.Contains(OpenTag) && word.Contains(CloseTag)))
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
        Stack<int> stack = new();
        for (int i = 0; i < mdstring.Length; i++)
        {
            if (i + OpenTag.Length < mdstring.Length
                && mdstring[i + OpenTag.Length] != ' '
                && (mdstring.Substring(i, OpenTag.Length) == OpenTag
                    && (i == 0 || mdstring[i - 1] == ' '))
                && !Contains(i, OpenTag.Length))
            {
                stack.Push(i);
            }

            if (i != 0
                && i + CloseTag.Length - 1 < mdstring.Length
                && mdstring[i - 1] != ' '
                && (i == mdstring.Length - CloseTag.Length
                    && mdstring.Substring(i, CloseTag.Length) == CloseTag
                    || mdstring.Substring(i, CloseTag.Length + 1) == CloseTag + " ")
                && !Contains(i, CloseTag.Length))
            {
                var j = stack.Pop();
                if (i - j == 1)
                    continue;
                result.Add(new TagToken(
                    j,
                    i + CloseTag.Length - 1,
                    new EmTag()));
                UpdateUsedIndexes(j, i);
            }
        }
    }

    public void UpdateUsedIndexes(int openTagIndex, int closeTagIndex)
    {
        for (int i = 0; i < OpenTag.Length; i++)
            usedIndexes.Add(openTagIndex + i);
        for (int i = 0; i < OpenTag.Length; i++)
            usedIndexes.Add(closeTagIndex + i);
    }

    public bool Contains(int i, int tagLength)
    {
        for (int j = 0; j < tagLength; j++)
        {
            if (usedIndexes.Contains(i + j))
                return true;
        }

        return false;
    }
}