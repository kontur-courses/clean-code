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
        
        public static TagTranslatorConfigurator CreateTokenTranslator()
        {
            return new TagTranslatorConfigurator();
        }
        
        public TagTranslatorFromConfigurator SetReference()
        {
            // var  confType = typeof(TagTranslatorToConfigurator);
            // var constructorInfoObj = confType.GetConstructor(new[] {typeof(Dictionary<Tag, Tag>)});
            // return (TagTranslatorFromConfigurator) constructorInfoObj.Invoke(new object[] {tags});
            
            return new TagTranslatorFromConfigurator(tags);
        }

        public ITagTranslator Configure()
        {
            var translator = new TagTranslator();
            
            foreach (var (from, to) in tags)
            {
                translator.SetReference(from, to);
            }

            return translator;
        }
        
        public class TagTranslatorToConfigurator
        {
            private readonly Dictionary<Tag, Tag> tags = new();
            private readonly Tag fromTag;
        
            private TagTranslatorToConfigurator(){}

            public TagTranslatorToConfigurator(Dictionary<Tag, Tag> tags, Tag fromTag)
            {
                this.fromTag = fromTag;
                this.tags = tags;
            }
        
            public TagTranslatorConfigurator To(Tag toTag)
            {
                tags[fromTag] = toTag ?? throw new ArgumentNullException();
                return new TagTranslatorConfigurator(tags);
            }
        } 
    
        public class TagTranslatorFromConfigurator
        {
            private readonly Dictionary<Tag, Tag> tags = new();
        
            private TagTranslatorFromConfigurator(){}

            public TagTranslatorFromConfigurator(Dictionary<Tag, Tag> tags)
            {
                this.tags = tags;
            }
        
            public TagTranslatorToConfigurator From(Tag tag)
            {
                // if (tag is null) throw new ArgumentNullException();
                // if (tags.ContainsKey(tag)) 
                //     throw new ArgumentException($"tag {tag.Start} already translating to tag {tags[tag].Start}");
                //
                // var  confType = typeof(TagTranslatorToConfigurator);
                // var paramsTypes = new[]
                // {
                //     typeof(Dictionary<Tag, Tag>),
                //     typeof(Tag)
                // };
                // var constructorInfoObj = confType.GetConstructor(paramsTypes);
                // return (TagTranslatorToConfigurator) constructorInfoObj.Invoke(new object[] {tags, tag});

                return new TagTranslatorToConfigurator(tags, tag);
            }
        }
    }
}