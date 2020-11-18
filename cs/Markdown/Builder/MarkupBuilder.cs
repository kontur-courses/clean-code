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
        private SortedDictionary<int, List<ReplacingData>> positionsToChanges; 
        
        public MarkupBuilder(params ITagData[] tags)
        {
            this.tags = tags.ToHashSet();
        }
        
        public string Build(TextData parsedTextData)
        {
            stringBuilder = new StringBuilder(parsedTextData.Value);
            positionsToChanges = new SortedDictionary<int, List<ReplacingData>>();
            
            foreach (var token in parsedTextData.Tokens)
                AnalyzeToken(token);
            foreach (var posToRemove in parsedTextData.ToRemove.Keys)
            {
                if (!positionsToChanges.ContainsKey(posToRemove))
                    positionsToChanges[posToRemove] = new List<ReplacingData>();
                positionsToChanges[posToRemove].Add(parsedTextData.ToRemove[posToRemove]);
            }
            

            foreach (var positionToChanges in positionsToChanges.Reverse())
            {
                foreach (var change in positionToChanges.Value)
                {
                    stringBuilder.Remove(positionToChanges.Key, change.Old.Length);
                    stringBuilder.Insert(positionToChanges.Key, change.New);
                }
            }

            return stringBuilder.ToString();
        }

        private void AnalyzeToken(TextToken token)
        {
            if (!positionsToChanges.ContainsKey(token.End))
                positionsToChanges[token.End] = new List<ReplacingData>();
            positionsToChanges[token.End].Add(new ReplacingData(token.Tag.IncomingBorder.Close,
                token.Tag.OutgoingBorder.Close));
            
            foreach (var subToken in token.SubTokens)
                AnalyzeToken(subToken);
            
            if (!positionsToChanges.ContainsKey(token.Start))
                positionsToChanges[token.Start] = new List<ReplacingData>();
            positionsToChanges[token.Start].Add(new ReplacingData(token.Tag.IncomingBorder.Open,
                token.Tag.OutgoingBorder.Open));
        }
    }
}