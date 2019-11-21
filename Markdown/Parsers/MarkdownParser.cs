using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using Markdown.IntermediateState;
using Markdown.Parsers.MarkdownRules;
using Configuration = Markdown.Parsers.MarkdownRules.Configuration;

namespace Markdown.Parsers
{
    class MarkdownParser : ILanguageParser
    {
        private readonly Dictionary<TagType, IParserRule> parserRules;
        
        public MarkdownParser()
        {
            parserRules = Configuration.GetConfiguration();
        }


        public DocumentNode GetParsedDocument(string inputDocument)
        {
            if (inputDocument is null)
                return new DocumentNode(TagType.All, null, 0, 0, 0);

            var root = new ParserNode(TagType.All, 0, inputDocument.Length, parserRules[TagType.All]);

            var nearNodes = GetFirstTags(inputDocument);

            var tags = new Stack<ParserNode>();
            tags.Push(root);
            var currentPosition = 0;

            while (tags.Count > 0)
            {
                var tag = nearNodes.FirstOrDefault().Value;

                // Если в текущем теге больше нет тегов то добиваем в его подтеги оставшийся текст и выходим из него
                if (tag == null || 
                    !TagInTheOther(tags.Peek(), tag) && !TagPartiallyInTheOther(tags.Peek(), tag))
                {
                    if (tags.Peek().EndInnerPartPosition != currentPosition)
                        tags.Peek().AddChild(new ParserNode(TagType.NoneTag, currentPosition,
                            tags.Peek().EndInnerPartPosition, parserRules[TagType.NoneTag]));
                    currentPosition = tags.Pop().EndPosition;
                    continue;
                }

                // Если внутри текущего тега есть теги он может быть помещен внутрь то добавляем его
                if (TagInTheOther(tags.Peek(), tag))
                {
                    if (!parserRules[tags.Peek().TypeTag].CanUseInCurrent(tag.TypeTag))
                    {
                        nearNodes.Remove(tag.StartPosition);
                        UpdateTag(nearNodes, inputDocument, tags.Peek().EndPosition, tag.TypeTag);
                        continue;
                    }
                    if (tag.StartPosition - currentPosition > 0)
                        tags.Peek().AddChild(new ParserNode(TagType.NoneTag, currentPosition, tag.StartPosition,
                            parserRules[TagType.NoneTag]));

                    currentPosition = tag.StartInnerPartPosition;
                    tags.Peek().AddChild(tag);
                    tags.Push(tag);
                    nearNodes.Remove(tag.StartPosition);
                    UpdateTag(nearNodes, inputDocument, tag.StartInnerPartPosition, tag.TypeTag);
                    continue;
                }

                nearNodes.Remove(tag.StartPosition);
                UpdateTag(nearNodes, inputDocument, tag.StartInnerPartPosition, tag.TypeTag);
            }


            return root.GetDocumentNode(inputDocument);
        }

        private static bool TagInTheOther(ParserNode wrapper, ParserNode inner)
        {
            return inner.StartPosition >= wrapper.StartInnerPartPosition &&
                   inner.EndPosition <= wrapper.EndInnerPartPosition;
        }

        private static bool TagPartiallyInTheOther(ParserNode wrapper, ParserNode inner)
        {
            return inner.StartPosition < wrapper.EndPosition && inner.EndPosition > wrapper.StartPosition;
        }

        private SortedDictionary<int, ParserNode> GetFirstTags(string source)
        {
            var result = new SortedDictionary<int, ParserNode>();
            foreach (var tagType in parserRules.Keys)
            {
                UpdateTag(result, source, 0, tagType);
            }

            return result;
        }

        private void UpdateTag(SortedDictionary<int, ParserNode> nearNodes, string source, int position, TagType tagType)
        {
            var node = parserRules[tagType].FindFirstElement(source, position);
            while (node != null)
            {
                if (!nearNodes.ContainsKey(node.StartPosition) || nearNodes[node.StartPosition].StartInnerPartPosition -
                    nearNodes[node.StartPosition].StartPosition < parserRules[tagType].OpenTag.Length)
                {
                    if (nearNodes.ContainsKey(node.StartPosition))
                    {
                        var tempTagType = nearNodes[node.StartPosition].TypeTag;
                        UpdateTag(nearNodes, source, node.StartInnerPartPosition, tempTagType);
                    }
                    nearNodes[node.StartPosition] = node;
                    break;
                }

                node = parserRules[tagType].FindFirstElement(source, node.StartInnerPartPosition);
            }
        }
    }
}
