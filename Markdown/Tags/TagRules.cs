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
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            inFrontRules.Add(tag);
        }
        
        public void AddTagInterruptTag(Tag tag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            tagInterruptTokens.Add(tag);
        }

        public bool IsInterruptTag(Tag tag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            return tagInterruptTokens.Contains(tag);
        }
        
        public void SetRule(Tag firstTag, Tag secondTag, InteractType interactType)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(firstTag), firstTag), 
                new ExceptionCheckObject(nameof(secondTag), secondTag));
            
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

        public bool CanBeNested(Tag outsideTag, Tag insideTag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(outsideTag), outsideTag), 
                new ExceptionCheckObject(nameof(insideTag), insideTag));
            return nestingRules.ContainsKey(outsideTag) && nestingRules[outsideTag].Contains(insideTag);
        }

        public bool CanContain(Tag outsideTag, Tag insideTag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(outsideTag), outsideTag), 
                new ExceptionCheckObject(nameof(insideTag), insideTag));
            return !containRules.ContainsKey(outsideTag) || !containRules[outsideTag].Contains(insideTag);
        }

        private bool CanBeNotInFront(Tag tag)
        {
            return !inFrontRules.Contains(tag);
        }

        public bool DoesMatchInFrontRule(TokenSegment segment, int newLinePosition)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(segment), segment), 
                new ExceptionCheckObject(nameof(newLinePosition), newLinePosition));
            return CanBeNotInFront(segment.GetBaseTag()) || newLinePosition == 0;
        }
    }
}