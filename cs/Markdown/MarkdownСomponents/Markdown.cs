using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Automaton;

namespace Markdown.MarkdownСomponents
{
    internal static class Markdown
    {
        private static readonly PushdownAutomaton mainMarkdownHandler;
        private static readonly PushdownAutomaton mainTagCorrector;
        private static readonly PushdownAutomaton differentWordSelectionHandler;
        private static readonly PushdownAutomaton afterSpacesTagsCorrector;
        private static readonly PushdownAutomaton blankLineCorrector;
        private static readonly List<string> tags = new() { "_", "\\", "#", "<em>", @"<\em>", "<strong>", @"<\strong>" };
        private static bool isTitle; 
        static Markdown()
        {
            mainMarkdownHandler = MarkdownHandlerBuilder.BuildMarkdownHandler();
            mainTagCorrector = TagCorrectorBuilder.BuildTagCorrector();
            differentWordSelectionHandler = DifferentWordSelectionHandlerBuilder.BuildDifferentWordSelectionHandler();
            afterSpacesTagsCorrector = AfterSpacesTagsCorrectorBuilder.BuildAfterSpacesTagsCorrector();
            blankLineCorrector = BlankLineCorrectorBuilder.BuildBlankLineCorrector();
        }

        public static string Render(string input)
        {
            if (input.StartsWith("# "))
            {
                isTitle = true;
                input = input.Substring(2);
            }

            var mainMemory = GetMemoryOfMainAutomaton(input);
            var simpleTokens = GetSimpleToken(input);

            PushdownAutomaton[] automatonArray =
            {
                blankLineCorrector,
                afterSpacesTagsCorrector,
                mainTagCorrector,
                differentWordSelectionHandler,
            };

            foreach (var automaton in automatonArray)
            {
                var problems = GetProblematicTags(automaton, mainMemory);
                HandleProblematicTags(mainMemory, problems.Key, problems.Value, automaton.IndexOffset);
            }

            return BuildResult(mainMemory, simpleTokens);
        }

        private static KeyValuePair<string[], int[]> GetProblematicTags(PushdownAutomaton automaton, string[] tokens)
        {
            automaton.Run(tokens);
            var problemTags = automaton.GetAutomatonMemory();
            var problemTagsIndices = automaton.GetMainStackIndicesAutomatonMemory();
            automaton.Dispose();

            return new KeyValuePair<string[], int[]>(problemTags, problemTagsIndices);
        }

        private static string[] GetMemoryOfMainAutomaton(string input)
        {
            mainMarkdownHandler.Run(input);
            var mainMemory = mainMarkdownHandler.GetAutomatonMemory();
            mainMarkdownHandler.Dispose();

            return mainMemory;
        }

        private static void HandleProblematicTags(string[] memory, string[] problemTags, int[] problemTagsIndex, int indexOffset = 0)
        {
            for (int i = 0; i < problemTags.Length; i++)
            {
                var problem = problemTags[i];
                var problemIndex = problemTagsIndex[i] - indexOffset;

                memory[problemIndex] = problem switch
                {
                    "<em>" or @"<\em>" => "_",
                    "<strong>" or @"<\strong>" => "__",
                    _ => memory[problemIndex]
                };
            }
        }

        private static string BuildResult(string[] memory, string[] letters)
        {
            var builder = new StringBuilder();

            if (isTitle)
                builder.Append("<h1>");
            DestandardizeTokens(memory, letters, builder);

            if (builder[^1] == '%')
                builder.Remove(builder.Length - 1, 1);

            if (isTitle)
                builder.Append(@"<\h1>");

            isTitle = false;
            return builder.ToString();
        }

        private static void DestandardizeTokens(string[] memory, string[] letters, StringBuilder builder)
        {
            var j = 0;
            for (var i = 0; i < memory.Length; i++)
            {
                if (IsSimpleStandartizeToken(memory[i]))
                {
                    memory[i] = letters[j];
                    j++;
                }

                builder.Append(memory[i]);

                if (j < letters.Length) continue;

                for (var w = i + 1; w < memory.Length; w++)
                    builder.Append(memory[w]);

                break;
            }
        }
        
        private static bool IsSimpleStandartizeToken(string token) => token is "K" or "1" or " ";

        private static string[] GetSimpleToken(string tokens)
        {
            return tokens
                .Select(token => token.ToString())
                .Where(t => !tags.Contains(t))
                .ToArray();
        }
    }
}
