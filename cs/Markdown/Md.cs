using System.Text;
using Markdown.MarkupSpecification;
using Markdown.Tags;
using Markdown.Tags.TagSpecification;

namespace Markdown;

public class Md(ISpecificationProvider specificationProvider)
{
    public string Render(string markdown)
    {
        var markupSpecification = specificationProvider.GetMarkupSpecification().ToArray();

        var tagReplacements = FindAllTags(markdown, markupSpecification);
        var renderedString = PerformTextFormatting(markdown, tagReplacements.ToArray());

        return RemoveEscapingOfControlSubstrings(renderedString, markupSpecification);
    }

    private IEnumerable<TagReplacementSpecification> FindAllTags(string text, BaseTag[] markupSpecification)
    {
        var result = new BinaryTree<TagReplacementSpecification>();
        
        foreach (var tagSpecification in markupSpecification)
        {
            var fragment = tagSpecification.FindNextPairOfTags(text, 0, tagSpecification);
            
            while (fragment is not null)
            {
                if (fragment.OpeningTag.StartIndex > fragment.ClosingTag.StartIndex)
                    throw new Exception("Открывающийся тег должен находиться перед закрывающимся");
                
                result.Add(fragment.OpeningTag);
                result.Add(fragment.ClosingTag);

                var nextStartIndex = fragment.ClosingTag.StartIndex + fragment.ClosingTag.Tag.Old.Length;
                if (nextStartIndex >= text.Length) break;
                
                fragment = tagSpecification.FindNextPairOfTags(text, nextStartIndex, tagSpecification);
            }
        }

        return EliminateTagConflictsAndIntersections(result);
    }

    private BinaryTree<TagReplacementSpecification> EliminateTagConflictsAndIntersections(
        BinaryTree<TagReplacementSpecification> tags)
    {
        var stack = new Stack<TagReplacementSpecification>();
        TagReplacementSpecification? openingTag = null;
        var binaryTree = new BinaryTree<TagReplacementSpecification>();
        var len = tags.Count();

        for (var i = 0; i < len; i++)
        {
            if (openingTag is not null && openingTag.Markup.Closing == tags[i].Tag)
            {
                binaryTree.Add(openingTag);
                binaryTree.Add(tags[i]);
                openingTag = stack.Count != 0 ? stack.Pop() : null;
            }
            else if (tags[i].Tag == tags[i].Markup.Opening)
            {
                if ((openingTag is null || !openingTag.Markup.DidConflict(tags[i].Markup)) &&
                    !stack.Any(specification => specification.Markup.DidConflict(tags[i].Markup)))
                {
                    if (openingTag is not null) stack.Push(openingTag);
                    openingTag = tags[i];
                }
            }
            if (stack.Any(specification => specification.Markup == tags[i].Markup))
            {
                openingTag = stack.Pop();
                while(openingTag.Markup != tags[i].Markup)
                    openingTag = stack.Pop();
            }
        }
        
        return binaryTree;
    }
    
    private string PerformTextFormatting(string text, TagReplacementSpecification[] replacements)
    {
        if (replacements.Length == 0)
            return text;

        var result = new StringBuilder();
        var endOfLastReplacement = -1;

        for (var i = 0; i < replacements.Length; i++)
        {
            result.Append(text[(endOfLastReplacement + 1)..replacements[i].StartIndex]);
            result.Append(replacements[i].Markup.PerformTagFormatting(
                replacements[i],
                i != 0 ? replacements[i - 1].Markup : null,
                i < replacements.Length - 1 ? replacements[i + 1].Markup : null));
            endOfLastReplacement = replacements[i].StartIndex + replacements[i].Tag.Old.Length - 1;
        }
        
        if (endOfLastReplacement + 1 != text.Length)
            result.Append(text[(endOfLastReplacement + 1)..text.Length]);
        
        return result.ToString();
    }
    
    private string RemoveEscapingOfControlSubstrings(string text, IEnumerable<BaseTag> tags)
    {
        foreach (var tag in tags)
        {
            text = text.Replace('\\' + tag.Opening.Old, tag.Opening.Old);
            if (tag.Closing.Old != tag.Opening.Old)
                text = text.Replace('\\' + tag.Closing.Old, tag.Closing.Old);
        }
        
        return text.Replace(@"\\", "\\");
    }
}