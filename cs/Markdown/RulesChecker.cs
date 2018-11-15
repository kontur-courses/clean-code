using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class RulesChecker
    {
        private readonly SymbolAndTagConnection[] symbolAndTagConnection =
        {
            new SymbolAndTagConnection {Position = Position.Start, Symbol = SymbolType.Underscore, Tag = "<em>"},
            new SymbolAndTagConnection {Position = Position.End, Symbol = SymbolType.Underscore, Tag = "</em>"},
            new SymbolAndTagConnection
                {Position = Position.Start, Symbol = SymbolType.DoubleUnderscore, Tag = "<strong>"},
            new SymbolAndTagConnection
                {Position = Position.End, Symbol = SymbolType.DoubleUnderscore, Tag = "</strong>"},
            new SymbolAndTagConnection {Position = Position.Start, Symbol = SymbolType.GraveAccent, Tag = "<code>"},
            new SymbolAndTagConnection {Position = Position.End, Symbol = SymbolType.GraveAccent, Tag = "</code>"},
            new SymbolAndTagConnection {Position = Position.Start, Symbol = SymbolType.Backslash, Tag = "backslash"},
        };

        public SortedDictionary<int, string> CheckCorrectness(SortedDictionary<int, SymbolType> dictionary)
        {
            var dictForChecking = DeleteShieldedSymbols(dictionary);
            dictForChecking = DeleteDoubleUnderscoreBetweenUnderscore(dictForChecking);
            var countUnderscore = 1;
            var countDoubleUnderscore = 1;
            var countGraveAccent = 1;
            var dictCorrectTags = new SortedDictionary<int, string>();
            foreach (var pair in dictForChecking)
            {
                switch (pair.Value)
                {
                    case SymbolType.Underscore:
                    {
                        var position = countUnderscore % 2 == 1 ? Position.Start : Position.End;
                        dictCorrectTags.Add(pair.Key, GetTag(pair.Value, position));
                        countUnderscore++;
                        break;
                    }
                    case SymbolType.DoubleUnderscore:
                    {
                        var position = countDoubleUnderscore % 2 == 1 ? Position.Start : Position.End;
                        dictCorrectTags.Add(pair.Key, GetTag(pair.Value, position));
                        countDoubleUnderscore++;
                        break;
                    }
                    case SymbolType.GraveAccent:
                    {
                        var position = countGraveAccent % 2 == 1 ? Position.Start : Position.End;
                        dictCorrectTags.Add(pair.Key, GetTag(pair.Value, position));
                        countGraveAccent++;
                        break;
                    }
                    default:
                        dictCorrectTags.Add(pair.Key, GetTag(pair.Value, Position.Start));
                        break;
                }
            }

            return dictCorrectTags;
        }

        private string GetTag(SymbolType symbol, Position position)
        {
            foreach (var tagsPosition in symbolAndTagConnection)
                if (symbol == tagsPosition.Symbol && position == tagsPosition.Position)
                    return tagsPosition.Tag;
            return null;
        }

        private SortedDictionary<int, SymbolType> DeleteShieldedSymbols(SortedDictionary<int, SymbolType> dictionary)
        {
            if (!dictionary.ContainsValue(SymbolType.Backslash))
                return dictionary;
            var posWithoutShieldSymbols = new SortedDictionary<int, SymbolType>();
            foreach (var pair in dictionary)
                if (!IsSymbolShielded(dictionary, pair))
                    posWithoutShieldSymbols.Add(pair.Key, pair.Value);

            return posWithoutShieldSymbols;
        }

        private static bool IsSymbolShielded(SortedDictionary<int, SymbolType> dictionary, KeyValuePair<int, SymbolType> pair)
        {
            if (pair.Value == SymbolType.Backslash)
                return false;

            return dictionary.ContainsKey(pair.Key - 1) && dictionary[pair.Key - 1] == SymbolType.Backslash;
        }

        private SortedDictionary<int, SymbolType> DeleteDoubleUnderscoreBetweenUnderscore(
            SortedDictionary<int, SymbolType> dictionary)
        {
            if (!dictionary.ContainsValue(SymbolType.DoubleUnderscore))
                return dictionary;
            var underscorePos = GetArrayOfPositions(dictionary, SymbolType.Underscore);
            var doubleUnderscorePos = GetArrayOfPositions(dictionary, SymbolType.DoubleUnderscore);
            var posOfDeletedSymbols = new List<int>();
            if (underscorePos.Length >= 2 && doubleUnderscorePos.Length >= 2)
            {
                var upperBorder = Math.Min(underscorePos.Length, doubleUnderscorePos.Length);
                for (var i = 0; i < upperBorder - 1; i++)
                {
                    if (IsDoubleUnderscoreBetweenUnderscore(underscorePos, i, doubleUnderscorePos))
                    {
                        posOfDeletedSymbols.Add(doubleUnderscorePos[i]);
                        posOfDeletedSymbols.Add(doubleUnderscorePos[i + 1]);
                    }
                }
            }
            else
                return dictionary;

            var correctPositions = new SortedDictionary<int, SymbolType>();
            foreach (var pair in dictionary)
                if (!posOfDeletedSymbols.Contains(pair.Key))
                    correctPositions.Add(pair.Key, pair.Value);

            return correctPositions;
        }

        private static bool IsDoubleUnderscoreBetweenUnderscore(int[] underscorePos, int i, int[] doubleUnderscorePos)
        {
            return underscorePos[i] < doubleUnderscorePos[i] && doubleUnderscorePos[i + 1] < underscorePos[i + 1];
        }

        private int[] GetArrayOfPositions(SortedDictionary<int, SymbolType> dictionary, SymbolType type)
        {
            return dictionary
                .Where(pair => pair.Value == type)
                .Select(pair => pair.Key)
                .ToArray();
        }
    }
}