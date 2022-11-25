using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public static class DictionaryExt
{
    public static void Reboot(
        this Dictionary<ITag?, (int startCount, int endCount)> tagsOpenCloseCounter)
    {
        foreach (var (key, _) in tagsOpenCloseCounter)
            tagsOpenCloseCounter[key] = (0, 0);
    }
}