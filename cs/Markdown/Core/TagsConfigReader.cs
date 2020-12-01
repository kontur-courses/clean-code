using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Markdown.Core
{
    public static class TagsConfigReader
    {
        private static Dictionary<string, string> TokensTags { get; set; }

        static TagsConfigReader()
        {
            const string tagsConfig = "TagsConfig.json";
            if (!File.Exists(tagsConfig))
                throw new ArgumentException($"Config on path \"{tagsConfig}\" does not exists");

            var rawJson = File.ReadAllText(tagsConfig);
            TokensTags = ParseFromJson(rawJson);
        }

        public static bool IsMarkdownTag(string possibleTag) => TokensTags.ContainsValue(possibleTag);
        public static string GetMdTagForTokenName(string tokenName) => TokensTags[tokenName];

        private static Dictionary<string, string> ParseFromJson(string rawJson) =>
            JObject.Parse(rawJson)
                .Properties()
                .ToDictionary(property => property.Name, property => property.Value.ToString());
    }
}