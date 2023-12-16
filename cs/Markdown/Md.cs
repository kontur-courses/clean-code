using System.Text;

namespace Markdown;

public class Md
{
    public static string Render(string textInMd)
    {
        var renderedText = new StringBuilder();
        foreach (var paragraph in GetParagraphs(textInMd))
        {
            renderedText.Append(RenderParagraph(paragraph));
        }

        return renderedText.ToString();
    }

    private static string RenderParagraph(string paragraph)
    {
        var tags = GetTags(paragraph);
        var textWithoutTags = DeleteTags(paragraph, tags);
        return InsertHtmlTags(textWithoutTags, tags.ToArray());
    }

    private static IEnumerable<Tag> GetTags(string text)
    {
        var allTags = GetTagsWithEscapedTagsFromText(text);
        var tags = GetTagsWithoutEscapedTags(allTags);

        return DeleteNotPairTags(tags);
    }

    private static List<Tag> GetTagsWithoutEscapedTags(List<Tag> tags)
    {
        var tagsWithoutEscapedTags = new List<Tag>();
        for (var i = 0; i < tags.Count; i++)
        {
            if (tags[i].Symbol == "\\" && tags.Count > i + 1 && tags[i].Position + 1 == tags[i + 1].Position)
            {
                tagsWithoutEscapedTags.Add(tags[i]);
                i++;
            }
            else if (tags[i].Symbol != "\\")
                tagsWithoutEscapedTags.Add(tags[i]);
        }

        return tagsWithoutEscapedTags;
    }

    private static List<Tag> GetTagsWithEscapedTagsFromText(string text)
    {
        var openTags = new HashSet<string>();
        var tags = new List<Tag>();
        Tag tag;
        for (var i = 0; i < text.Length; i += tag?.Lenght ?? 1)
        {
            tag = GetTag(text, i);
            if (tag == null) continue;
            if (!TagIsOk(text, tag, openTags, i)) continue;

            tags.Add(tag);
        }

        return tags;
    }

    private static IEnumerable<Tag> DeleteNotPairTags(List<Tag> tags)
    {
        var tagCount = new Dictionary<string, int>();
        foreach (var tag in tags.Where(tag => tag.IsPaired && tag.Symbol != "\\"))
        {
            if (tagCount.ContainsKey(tag.Symbol))
                tagCount[tag.Symbol]++;
            else
                tagCount.Add(tag.Symbol, 1);
        }

        foreach (var index in tagCount.Where(x => x.Value % 2 != 0)
                     .Select(x => tags.FindLastIndex(tag => tag.Symbol == x.Key)))
        {
            tags.RemoveAt(index);
        }

        return tags;
    }
    
    private static Tag GetTag(string text, int position)
    {
        if (text[position] == '#') 
            return new Tag("#", position, false);
        
        if (text[position] == '\\') 
            return new Tag("\\", position);
        
        if (text[position] != '_') 
            return null;
        
        var tagLength = 1;
        if (text.Length > position + 1 && text[position + 1] == '_') tagLength++;

        if (position != 0 && char.IsDigit(text[position - 1])) 
            return null;
        
        if (text.Length > position + tagLength && char.IsDigit(text[position + tagLength]))
            return null;

        if (position != 0
            && text.Length > position + tagLength
            && text[position - 1] == ' '
            && text[position + tagLength] == ' ')
            return null;

        return tagLength == 2 ? new Tag("__", position) : new Tag("_", position);
    }


    private static bool TagIsOk(string text, Tag tag, HashSet<string> openTags, int i)
    {
        throw new NotImplementedException();
    }

    private static string DeleteTags(string text, IEnumerable<Tag> tags)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<string> GetParagraphs(string text)
    {
        throw new NotImplementedException();
    }

    private static string InsertHtmlTags(string text, Tag[] tags)
    {
        throw new NotImplementedException();
    }
}