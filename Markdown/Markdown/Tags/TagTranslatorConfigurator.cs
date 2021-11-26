using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TagTranslatorConfigurator
    {
        private readonly Dictionary<Tag, Tag> tags = new();
        
        private TagTranslatorConfigurator(){}

        private TagTranslatorConfigurator(Dictionary<Tag, Tag> tags)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tags), tags));
            this.tags = tags;
        }
        
        internal static TagTranslatorConfigurator CreateTokenTranslator()
        {
            return new TagTranslatorConfigurator();
        }
        
        internal TagTranslatorFromConfigurator SetReference()
        {
            return new TagTranslatorFromConfigurator(tags);
        }

        internal ITagTranslator Configure()
        {
            var translator = new TagTranslator();
            
            foreach (var (from, to) in tags)
            {
                translator.SetReference(from, to);
            }

            return translator;
        }
        
        internal class TagTranslatorToConfigurator
        {
            private readonly Dictionary<Tag, Tag> tags = new();
            private readonly Tag fromTag;
        
            private TagTranslatorToConfigurator(){}

            internal TagTranslatorToConfigurator(Dictionary<Tag, Tag> tags, Tag fromTag)
            {
                MdExceptionHelper.ThrowArgumentNullExceptionIf(
                    new ExceptionCheckObject(nameof(tags), tags),
                    new ExceptionCheckObject(nameof(fromTag), fromTag));
                this.fromTag = fromTag;
                this.tags = tags;
            }
        
            internal TagTranslatorConfigurator To(Tag toTag)
            {
                MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(toTag), toTag));
                tags[fromTag] = toTag;
                return new TagTranslatorConfigurator(tags);
            }
        } 
    
        internal class TagTranslatorFromConfigurator
        {
            private readonly Dictionary<Tag, Tag> tags = new();
        
            private TagTranslatorFromConfigurator(){}

            internal TagTranslatorFromConfigurator(Dictionary<Tag, Tag> tags)
            {
                MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tags), tags));
                this.tags = tags;
            }
        
            internal TagTranslatorToConfigurator From(Tag tag)
            {
                MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
                return new TagTranslatorToConfigurator(tags, tag);
            }
        }
    }
}