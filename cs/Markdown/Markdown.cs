using System.Collections.Generic;
using Markdown.Tag;
using Markdown.TagRendering;

namespace Markdown
{
    public class Markdown
    {
        private readonly Stack<MarkdownTag> stackedTags;
        private readonly List<MarkdownTag> replacements;
        private readonly TagGenerator tagGenerator;
        
        public Markdown()
        {
            stackedTags = new Stack<MarkdownTag>();
	        replacements = new List<MarkdownTag>();
            tagGenerator = new TagGenerator();
        }
        
        public string Render(string markdown)
        {
            for (int i = 0; i < markdown.Length; i++)
            {
                var rendeResult = tagGenerator.TryCreateTag(markdown, i);
                if (rendeResult.IsFaulted)
                    continue;
                
                var tag = rendeResult.Value;
                i += tag.MarkdownSymbolLength;
                
                stackedTags.Push(tag);
                
                if (tag.Name == TagNames.Shielded)
                    replacements.Add(tag);
                
                if (tag.Type == TagTypes.Closing)
                    PushTagsToReplacements();
            }

            return ReplaceMdToHtmlTags(markdown);
        }
        
        private void PushTagsToReplacements()
        {
            var closingTag = stackedTags.Pop();
            while (stackedTags.Count != 0)
            {
                if (stackedTags.Peek().Name == closingTag.Name)
                    break;
                
                stackedTags.Pop();
            }

            if (stackedTags.Count == 0) return;

            var openingTag = stackedTags.Pop();
            replacements.Add(openingTag);
            replacements.Add(closingTag);
        }
        
        private string ReplaceMdToHtmlTags(string paragraph)
        {
            replacements.Sort((tag1, tag2) 
                => tag2.PositionInMarkdown.CompareTo(tag1.PositionInMarkdown));
            var inEmTag = false;
            foreach (var tag in replacements)
            {
                if (inEmTag && tag.Name == TagNames.Strong)
                    continue;

                if (tag.Name == TagNames.Em)
                    inEmTag = (tag.Type == TagTypes.Closing);

                paragraph = paragraph
                    .Remove(tag.PositionInMarkdown, tag.MarkdownSymbolLength)
                    .Insert(tag.PositionInMarkdown, tag.HtmlRepresentation);
            }
            return paragraph;
        }
    }
}