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

        public void AddInFrontOnlyTag(Tag tag)
        {
            inFrontRules.Add(tag);
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

        private bool CanIntersect(Tag firstTag, Tag secondTag)
        {
            return intersectingRules.ContainsKey(firstTag) && intersectingRules[firstTag].Contains(secondTag);
        }

        private bool CanBeNested(Tag outsideTag, Tag insideTag)
        {
            return nestingRules.ContainsKey(outsideTag) && nestingRules[outsideTag].Contains(insideTag);
        }

        private bool CanContain(Tag outsideTag, Tag insideTag)
        {
            return !containRules.ContainsKey(outsideTag) || !containRules[outsideTag].Contains(insideTag);
        }
        
        private bool CanBeNotInFront(Tag tag)
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

        public bool DoesMatchInFrontRule(TokenSegment segment)
        {
            return CanBeNotInFront(segment.GetBaseTag()) || segment.StartPosition == 0;
        }
    }
}