using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Result;
using Markdown.Tag;

namespace Markdown.TagRendering
{
    internal class TagGenerator
    {
        private readonly List<HtmlTag> tagPrototypes;
        private readonly TagDetecter tagDetecter;
        private int investigatedPosition;
        private string paragraph;
        
        public TagGenerator()
        {
            tagDetecter = new TagDetecter();
            
            tagPrototypes = new List<HtmlTag>
            {
                new HtmlTag()
                {
                    MarkdownStringOrSymbol = "__",
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>("<strong>", "</strong>"),
                    TagName = TagNames.Strong
                },
                new HtmlTag()
                {
                    MarkdownStringOrSymbol = "_",
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>("<em>", "</em>"),
                    TagName = TagNames.Em
                },
                new HtmlTag()
                {
                    MarkdownStringOrSymbol = String.Empty,
                    OpeningAndClosingTagPair = new OpeningAndClosingTagPair<string, string>(string.Empty, string.Empty),
                    TagName = TagNames.Shielded
                }
            };
        }
        
        public Result<MarkdownTag> TryCreateTag(string text, int position)
        {
            this.paragraph = text;
            this.investigatedPosition = position;

            if (tagDetecter.ShieldedTagDetected(text, position))
            {
                var tagBulder = TagBuilder.Tag();

                tagBulder.WithName(TagNames.Shielded)
                    .WithMarkdownSymbolLength(1)
                    .WithType(TagTypes.Opening)
                    .InPosition(position);

                return Result<MarkdownTag>.Success(tagBulder.Build()); 
            }

            if (tagDetecter.StrongTagDetectedIn(text, position))
                return GetTag(TagNames.Strong);

            if (tagDetecter.EmTagDetectedIn(text, position))
                return GetTag(TagNames.Em);

            return Result<MarkdownTag>.Fail(Status.NotFound);
        }

        private MarkdownTag TryGetOpenOrCloseTag(HtmlTag prototype)
        {
            if (prototype == null)
                return null;

            var tagBuilder = TagBuilder.Tag()
                .InPosition(this.investigatedPosition)
                .WithMarkdownSymbolLength(prototype.MarkdownStringOrSymbol.Length)
                .WithName(prototype.TagName);

            if (tagDetecter.TryDetectClosing(this.paragraph, this.investigatedPosition))
            {
                tagBuilder.WithHtmlRepresentation(prototype.OpeningAndClosingTagPair.Closing)
                    .WithType(TagTypes.Closing);
                return tagBuilder.Build();
            }
            else
            {
                var canGetOpening = (prototype.TagName == TagNames.Em &&
                                     tagDetecter.TryDetectOpening(paragraph, investigatedPosition))
                                    || (prototype.TagName == TagNames.Strong &&
                                        tagDetecter.TryDetectOpening(paragraph, investigatedPosition + 1));
                if (canGetOpening)
                {
                    tagBuilder.WithHtmlRepresentation(prototype.OpeningAndClosingTagPair.Opening)
                        .WithType(TagTypes.Opening);
                    return tagBuilder.Build();
                }
            }

            return null;
        }

        private Result<MarkdownTag> GetTag(TagNames name)
        {
            var prototype = tagPrototypes.FirstOrDefault(t => t.TagName == name);
            var tag = TryGetOpenOrCloseTag(prototype);

            if (tag == null)
                return Result<MarkdownTag>.Fail(Status.Fail);
                
            return Result<MarkdownTag>.Success(tag);
        }
    }
}
