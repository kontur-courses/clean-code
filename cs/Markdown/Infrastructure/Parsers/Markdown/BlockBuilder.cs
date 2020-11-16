using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers.Markdown
{
    public class BlockBuilder : IBlockBuilder
    {
        private readonly ITextHelper textHelper;

        public BlockBuilder(ITextHelper textHelper)
        {
            this.textHelper = textHelper;
        }
        
        public IBlock Build(IEnumerable<TagInfo> validTags)
        {
            var processedPosition = 0;
            var subBlocks = new List<IBlock>();
            var rootBlock = new RootBlock(subBlocks);

            var tagsEnumerator = validTags.GetEnumerator();
            while (tagsEnumerator.MoveNext())
            {
                processedPosition = AddPreviousUnprocessedBlock(tagsEnumerator, processedPosition, subBlocks);
                var (subBlock, processed) = BuildBlock(tagsEnumerator, processedPosition);
                subBlocks.Add(subBlock);
                processedPosition = processed;
            }

            subBlocks.Add(GetBlockFromText(processedPosition, textHelper.GetTextLength()));

            return rootBlock;
        }

        private PlainBlock GetBlockFromText(int start, int end)
        {
            return new PlainBlock(textHelper.Substring(start, end - start));
        }

        private (IBlock, int) BuildBlock(IEnumerator<TagInfo> tagsEnumerator, int processedPosition)
        {
            var rootTag = tagsEnumerator.Current;
            var subBlocks = new List<IBlock>();
            var rootBlock = new StyledBlock(rootTag.Tag, subBlocks);

            while (tagsEnumerator.MoveNext())
            {
                processedPosition = AddPreviousUnprocessedBlock(tagsEnumerator, processedPosition, subBlocks);
                var currentTag = tagsEnumerator.Current;

                if (currentTag.Closes(rootTag, textHelper))
                    return (rootBlock, processedPosition);

                var (subBlock, processed) = BuildBlock(tagsEnumerator, processedPosition);
                subBlocks.Add(subBlock);
                processedPosition = processed;
            }

            throw new FormatException($"Closing tag missing for {rootTag.Offset} {rootTag.Length} {rootTag.Tag.Style}");
        }

        private int AddPreviousUnprocessedBlock(
            IEnumerator<TagInfo> tagsEnumerator,
            int processedPosition,
            List<IBlock> subBlocks)
        {
            var currentTag = tagsEnumerator.Current;
            if (currentTag.Offset < processedPosition)
                return processedPosition;

            subBlocks.Add(GetBlockFromText(processedPosition, currentTag.Offset));
            return currentTag.Offset + currentTag.Length;
        }
        
    }
}