using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class BlockBuilder : IBlockBuilder
    {
        private readonly string text;
        public BlockBuilder(string text)
        {
            this.text = text;
        }
        
        public IBlock Build(IEnumerable<TagInfo> validTags)
        {
            var processedPosition = 0;
            var subBlocks = new List<IBlock>();
            var rootBlock = new RootBlock(subBlocks);

            var tagsEnumerator = validTags.GetEnumerator();
            while (tagsEnumerator.MoveNext())
            {
                processedPosition = AddPreviousUnprocessedBlock(tagsEnumerator, processedPosition, text, subBlocks);
                var (subBlock, processed) = BuildBlock(tagsEnumerator, processedPosition, text);
                subBlocks.Add(subBlock);
                processedPosition = processed;
            }

            subBlocks.Add(GetBlockFromText(text, processedPosition, text.Length));

            return rootBlock;
        }

        private static PlainBlock GetBlockFromText(string text, int start, int end)
        {
            return new PlainBlock(text.Substring(start, end - start));
        }

        private (IBlock, int) BuildBlock(IEnumerator<TagInfo> tagsEnumerator, int processedPosition, string text)
        {
            var rootTag = tagsEnumerator.Current;
            var subBlocks = new List<IBlock>();
            var rootBlock = new StyledBlock(rootTag.Tag, subBlocks);

            while (tagsEnumerator.MoveNext())
            {
                processedPosition = AddPreviousUnprocessedBlock(tagsEnumerator, processedPosition, text, subBlocks);
                var currentTag = tagsEnumerator.Current;

                if (currentTag.Closes(rootTag, text))
                    return (rootBlock, processedPosition);

                var (subBlock, processed) = BuildBlock(tagsEnumerator, processedPosition, text);
                subBlocks.Add(subBlock);
                processedPosition = processed;
            }

            throw new FormatException($"Closing tag missing for {rootTag.Offset} {rootTag.Length} {rootTag.Tag.Style}");
        }

        private static int AddPreviousUnprocessedBlock(
            IEnumerator<TagInfo> tagsEnumerator,
            int processedPosition,
            string text,
            List<IBlock> subBlocks)
        {
            var currentTag = tagsEnumerator.Current;
            if (currentTag.Offset < processedPosition)
                return processedPosition;

            subBlocks.Add(GetBlockFromText(text, processedPosition, currentTag.Offset));
            return currentTag.Offset + currentTag.Length;
        }
        
    }
}