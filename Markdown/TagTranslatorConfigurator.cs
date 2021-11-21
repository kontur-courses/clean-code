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
                this.fromTag = fromTag;
                this.tags = tags;
            }
        
            internal TagTranslatorConfigurator To(Tag toTag)
            {
                tags[fromTag] = toTag ?? throw new ArgumentNullException();
                return new TagTranslatorConfigurator(tags);
            }
        } 
    
        internal class TagTranslatorFromConfigurator
        {
            private readonly Dictionary<Tag, Tag> tags = new();
        
            private TagTranslatorFromConfigurator(){}

            internal TagTranslatorFromConfigurator(Dictionary<Tag, Tag> tags)
            {
                this.tags = tags;
            }
        
            internal TagTranslatorToConfigurator From(Tag tag)
            {
                return new TagTranslatorToConfigurator(tags, tag);
            }
        }
    }
}