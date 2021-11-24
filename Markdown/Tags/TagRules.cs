using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TagRules
    {
        private readonly Dictionary<Tag, HashSet<Tag>> nestingRules = new();
        private readonly Dictionary<Tag, HashSet<Tag>> intersectingRules = new();
        private readonly Dictionary<Tag, HashSet<Tag>> containRules = new();
        private readonly HashSet<Tag> inFrontRules = new();
        private readonly HashSet<Tag> tagInterruptTokens = new();

        public void AddInFrontOnlyTag(Tag tag)
        {
            inFrontRules.Add(tag);
        }
        
        public void AddTagInterruptTag(Tag tag)
        {
            tagInterruptTokens.Add(tag);
        }

        public bool IsInterruptTag(Tag tag)
        {
            return tagInterruptTokens.Contains(tag);
        }
        
        public void SetRule(Tag firstTag, Tag secondTag, InteractType interactType)
        {
            var ruleListToUpdate = interactType switch
            {
                InteractType.Intersecting => intersectingRules,
                InteractType.Nesting => nestingRules,
                InteractType.Contain => containRules,
                _ => throw new ArgumentException($"unexpected interaction type {interactType}")
            };

            if (!ruleListToUpdate.ContainsKey(firstTag))
                ruleListToUpdate[firstTag] = new HashSet<Tag>();
            if (ruleListToUpdate[firstTag].Contains(secondTag))
                return;
            ruleListToUpdate[firstTag].Add(secondTag);

            if (interactType == InteractType.Intersecting)
            {
                if (!ruleListToUpdate.ContainsKey(secondTag))
                    ruleListToUpdate[secondTag] = new HashSet<Tag>();
                if (!ruleListToUpdate[secondTag].Contains(firstTag))
                    ruleListToUpdate[secondTag].Add(firstTag);
            }
        }

        public bool CanIntersect(Tag firstTag, Tag secondTag)
        {
            return intersectingRules.ContainsKey(firstTag) && intersectingRules[firstTag].Contains(secondTag);
        }

        public bool CanBeNested(Tag outsideTag, Tag insideTag)
        {
            return nestingRules.ContainsKey(outsideTag) && nestingRules[outsideTag].Contains(insideTag);
        }

        public bool CanContain(Tag outsideTag, Tag insideTag)
        {
            return !containRules.ContainsKey(outsideTag) || !containRules[outsideTag].Contains(insideTag);
        }
        
        public bool CanBeNotInFront(Tag tag)
        {
            return !inFrontRules.Contains(tag);
        }
        
        public bool DoesMatchIntersectingRule(TokenSegment first, TokenSegment second)
        {
            return CanIntersect(first.GetBaseTag(), second.GetBaseTag()) || !first.IsIntersectWith(second);
        }
        
        public bool DoesMatchNestingRule(TokenSegment outside, TokenSegment inside)
        {
            return CanBeNested(outside.GetBaseTag(), inside.GetBaseTag()) || !outside.Contain(inside);
        }
        
        public bool DoesMatchContainRule(TokenSegment outside, TokenSegment inside)
        {
            return CanContain(outside.GetBaseTag(), inside.GetBaseTag()) || !outside.Contain(inside);
        }

        public bool DoesMatchInFrontRule(TokenSegment segment, int newLinePosition)
        {
            return CanBeNotInFront(segment.GetBaseTag()) || newLinePosition == 0;
        }
    }
}