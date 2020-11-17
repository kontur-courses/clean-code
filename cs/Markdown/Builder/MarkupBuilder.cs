using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag;

namespace Markdown.Builder
{
    public class MarkupBuilder : IMarkupBuilder
    {
        private readonly HashSet<ITagData> tags;
        
        private StringBuilder stringBuilder;
        private SortedDictionary<int, ReplacingData> positionsToChanges; 
        
        public MarkupBuilder(params ITagData[] tags)
        {
            this.tags = tags.ToHashSet();
        }
        
        public string Build(TextData parsedTextData)
        {
            stringBuilder = new StringBuilder(parsedTextData.Value);
            positionsToChanges = new SortedDictionary<int, ReplacingData>();
            
            foreach (var token in parsedTextData.Tokens)
                AnalyzeToken(token);
            foreach (var posToRemove in parsedTextData.ToRemove.Keys)
                positionsToChanges[posToRemove] = parsedTextData.ToRemove[posToRemove];
            

            foreach (var posToChange in positionsToChanges.Reverse())
            {
                stringBuilder.Remove(posToChange.Key, posToChange.Value.Old.Length);
                stringBuilder.Insert(posToChange.Key, posToChange.Value.New);
            }

            return stringBuilder.ToString();
        }

        private void AnalyzeToken(TextToken token)
        {
            positionsToChanges[token.Start] = new ReplacingData(token.Tag.IncomingBorder.Open,
                token.Tag.OutgoingBorder.Open);
            positionsToChanges[token.End] = new ReplacingData(token.Tag.IncomingBorder.Close,
                token.Tag.OutgoingBorder.Close);
            
            foreach (var subToken in token.SubTokens)
                AnalyzeToken(subToken);
        }
    }
}