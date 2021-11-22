using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TagRules
    {
        private readonly Dictionary<Tag, HashSet<Tag>> containRules = new();
        private readonly Dictionary<Tag, HashSet<Tag>> intersectingRules = new();

        public void SetRule(Tag firstTag, Tag secondTag, InteractType interactType)
        {
            var ruleListToUpdate = interactType switch
            {
                InteractType.Intersecting => intersectingRules,
                InteractType.Nesting => containRules,
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
            return containRules.ContainsKey(outsideTag) && containRules[outsideTag].Contains(insideTag);
        }
    }
}