using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.TagRendering
{
    class TagGenerator
    {
        private readonly List<HtmlTagPrototype> tagPrototypes;
        private readonly TagDetecter tagDetecter;
        private int investigatedPosition;
        private string paragraph;
        
        public TagGenerator()
        {
            tagDetecter = new TagDetecter();
            
            tagPrototypes = new List<HtmlTagPrototype>
            {
                new HtmlTagPrototype()
                {
                    MarkdownStringOrSymbol = "__",
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>("<strong>", "</strong>"),
                    TagName = TagNames.Strong
                },
                new HtmlTagPrototype()
                {
                    MarkdownStringOrSymbol = "_",
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>("<em>", "</em>"),
                    TagName = TagNames.Em
                },
                new HtmlTagPrototype()
                {
                    MarkdownStringOrSymbol = String.Empty,
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>(string.Empty, string.Empty),
                    TagName = TagNames.Shielded
                }
            };
        }
        
        public Result<HtmlTagInMarkdown> TryCreateTag(string paragraph, int position)
        {
            this.paragraph = paragraph;
            this.investigatedPosition = position;

            if (tagDetecter.ShieldedTagDetected(paragraph, position))
            {
                var tagBulder = TagBuilder.ATag();

                tagBulder.WithName(TagNames.Shielded)
                    .WithMarkdownSymbolLength(1)
                    .WithType(TagTypes.Opening)
                    .InPosition(position);

                return Result<HtmlTagInMarkdown>.Success(tagBulder.Build()); 
            }
            
            if (tagDetecter.StrongTagDetectedIn(paragraph, position))
                return GetTag(TagNames.Strong);
            if (tagDetecter.EmTagDetectedIn(paragraph, position))
                return GetTag(TagNames.Em);

            return Result<HtmlTagInMarkdown>.Fail(Status.NotFound);
        }

        private HtmlTagInMarkdown TryRenderTag(HtmlTagPrototype prototype)
        {
            if (prototype == null)
                return null;

            var tagBuilder = TagBuilder.ATag()
                .InPosition(this.investigatedPosition)
                .WithMarkdownSymbolLength(prototype.MarkdownStringOrSymbol.Length)
                .WithName(prototype.TagName);
            
            if (tagDetecter.TryDetectClosing(this.paragraph, this.investigatedPosition))
            {
                tagBuilder.WithHtmlRepresentation(prototype.OpeningAndClosingTagPair.Closing)
                    .WithType(TagTypes.Closing);
                return tagBuilder.Build();
            }
            else if (tagDetecter.TryDetectOpening(this.paragraph, this.investigatedPosition))
            {
                tagBuilder.WithHtmlRepresentation(prototype.OpeningAndClosingTagPair.Opening)
                    .WithType(TagTypes.Opening);
                return tagBuilder.Build();
            }

            return null;
        }

        private Result<HtmlTagInMarkdown> GetTag(TagNames name)
        {
            var prototype = tagPrototypes.FirstOrDefault(t => t.TagName == name);
            var tag = TryRenderTag(prototype);

            if (tag == null)
                return Result<HtmlTagInMarkdown>.Fail(Status.Fail);
                
            return Result<HtmlTagInMarkdown>.Success(tag);
        }
    }

    public enum TagNames
    {
        Em,
        Strong,
        Shielded
    }
    
    public enum TagTypes
    {
        Opening,
        Closing
    }
}
