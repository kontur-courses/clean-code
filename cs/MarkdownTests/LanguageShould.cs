using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Tokenizing;

namespace MarkdownTests
{
    [TestFixture]
    public class LanguageShould
    {
        [Test]
        public void ContainAllTags()
        {
            var types = Assembly.GetAssembly(typeof(Language)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Language)));

            var tags = typeof(Tag).GetEnumValues().Cast<Tag>().Where(t => t != Tag.Raw).ToArray();

            foreach (var type in types)
            {
                var language = (Language) Activator.CreateInstance(type);

                var openingTags = (Dictionary<Tag, string>) type
                    .BaseType.GetField("openingTags", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(language);
                var closingTags = (Dictionary<Tag, string>) type
                    .BaseType.GetField("closingTags", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(language);

                openingTags.Should().ContainKeys(tags);
                closingTags.Should().ContainKeys(tags);
            }
        }
    }
}
