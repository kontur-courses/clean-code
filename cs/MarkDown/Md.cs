using System.Text;

namespace MarkDown;

public static class Md
{
    private static bool IsEscapable(char c) => c == '\\' || c == '_' || c == '#';
    
    public static string Render(string markdown)
    {
        var paragraphs = SplitParagraph(markdown);
        var sb = new StringBuilder();

        foreach (var paragraph in paragraphs)
        {
            var symbols = ParagraphToSymbols(paragraph.Content);
            var subsequences     = DetectSubsequences(symbols);
            
            CheckFlagsForSubsequence(subsequences, symbols);
            
            var pairs    = PairAndValidate(subsequences, symbols);
            var inner    = RenderInline(symbols, pairs);

            if (paragraph.Type == ParagraphType.Heading)
                sb.Append("<h1>").Append(inner).Append("</h1>");
            else
                sb.Append("<p>").Append(inner).Append("</p>");
        }
        
        return sb.ToString();
    }

    private static Paragraph[] SplitParagraph(string input)
    {
        var paragraphs = new List<Paragraph>();

        foreach (var raw in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            if (raw.StartsWith("# "))
            {
                paragraphs.Add(new Paragraph(ParagraphType.Heading, raw.Substring(2)));
            }
            else
            {
                paragraphs.Add(new Paragraph(ParagraphType.Paragraph, raw));
            }
        }
        
        return paragraphs.ToArray();
    }

    private static Symbol[] ParagraphToSymbols(string content)
    {
        var result = new List<Symbol>();
        
        int i = 0;
        while (i < content.Length)
        {
            char c = content[i];

            if (c == '\\')
            {
                if (i + 1 < content.Length && IsEscapable(content[i + 1]))
                {
                    result.Add(new Symbol(SymbolType.Text, content[i + 1], i));
                    i += 2;
                    continue;
                }
                result.Add(new Symbol(SymbolType.Text, '\\', i));
                i++;
                continue;
            }

            if (c == '_')
            {
                result.Add(new Symbol(SymbolType.Special, '_', i));
                i++;
                continue;
            }

            result.Add(new Symbol(SymbolType.Text, content[i], i));
            i++;
        }
        return result.ToArray();
    }

    private static Subsequence[] DetectSubsequences(Symbol[] symbols)
    {
        var subsequences = new List<Subsequence>();
        
        int i = 0;
        while (i < symbols.Length)
        {
            if (symbols[i].SymbolType == SymbolType.Special && symbols[i].Contet == '_')
            {
                int start = i;
                
                while (i < symbols.Length 
                       && symbols[i].SymbolType == SymbolType.Special 
                       && symbols[i].Contet == '_')
                    i++;
                
                int lenght = i - start;
                
                subsequences.Add(new Subsequence(
                    start, 
                    lenght, 
                    lenght != 1 && lenght != 2)
                );
            }
            
            else i++;
        }

        return subsequences.ToArray();
    }
    
    private static void CheckFlagsForSubsequence(Subsequence[] subsequences, Symbol[] symbols)
    {
        foreach (var subsequence in subsequences)
        {
            if (subsequence.IsDead)
            {
                subsequence.CanOpen = false;
                subsequence.CanClose = false; 
                continue;
            }

            var left  = subsequence.StartIdx > 0 ? symbols[subsequence.StartIdx - 1] : null;
            
            var right = subsequence.StartIdx + subsequence.Length < symbols.Length 
                ? symbols[subsequence.StartIdx + subsequence.Length] : null;

            subsequence.CanOpen  = right != null && !char.IsWhiteSpace(right.Contet);
            subsequence.CanClose = left  != null && !char.IsWhiteSpace(left.Contet);

            if (left != null && right != null && char.IsDigit(left.Contet) && char.IsDigit(right.Contet))
            {
                subsequence.CanOpen = subsequence.CanClose = false;
            }
        }
    }
    
    private static Pair[] PairSubsequences(Subsequence[] subsequences, HashSet<(Subsequence, Subsequence)> forbidden)
    {
        var pairs = new List<Pair>();
        var stack = new List<Subsequence>();

        foreach (var subsequence in subsequences)
        {
            if (subsequence.IsDead) continue;

            if (subsequence.CanClose)
            {
                int found = -1;
                for (int k = stack.Count - 1; k >= 0; k--)
                {
                    if (stack[k].Length != subsequence.Length) continue;
                    if (forbidden.Contains((stack[k], subsequence))) continue;
                    found = k;
                    break;
                }
                if (found >= 0)
                {
                    pairs.Add(new Pair { Open = stack[found], Close = subsequence });
                    stack.RemoveAt(found);
                    continue;
                }
            }

            if (subsequence.CanOpen) stack.Add(subsequence);
        }
        return pairs.ToArray();
    }
    
    private static Pair[] PairAndValidate(Subsequence[] subsequences, Symbol[] symbols)
    {
        var forbidden = new HashSet<(Subsequence open, Subsequence close)>();
        int maxIterations = subsequences.Length * subsequences.Length + 10;

        for (int iter = 0; iter < maxIterations; iter++)
        {
            var pairs = PairSubsequences(subsequences, forbidden);

            var killed = new List<Pair>();
            ApplyFilterEmpty(pairs, symbols, killed);
            ApplyFilterIntrawordSpace(pairs, symbols, killed);
            ApplyFilterBoldInsideItalic(pairs, killed);
            ApplyFilterIntersection(pairs, killed);
            ApplyFilterDigitInside(pairs, symbols, killed);
            ApplyFilterNestedSameType(pairs, forbidden, killed);

            if (killed.Count == 0)
                return pairs.Where(p => p.IsAlive).ToArray();

            foreach (var p in killed)
                forbidden.Add((p.Open, p.Close));
        }

        throw new InvalidOperationException("Pairing did not converge");
    }
    
    private static string RenderInline(Symbol[] symbols, Pair[] pairs)
    {
        var map = new Dictionary<int, (Subsequence run, bool isOpen)>();
        foreach (var p in pairs.Where(x => x.IsAlive))
        {
            map[p.Open.StartIdx]  = (p.Open,  true);
            map[p.Close.StartIdx] = (p.Close, false);
        }

        var sb = new StringBuilder();
        int i = 0;
        while (i < symbols.Length)
        {
            var smb = symbols[i];

            if (smb.SymbolType == SymbolType.Special && smb.Contet == '_')
            {
                if (map.TryGetValue(i, out var entry))
                {
                    string tag = entry.run.Length == 1 ? "em" : "strong";
                    sb.Append(entry.isOpen ? $"<{tag}>" : $"</{tag}>");
                    i += entry.run.Length;
                }
                else
                {
                    sb.Append('_');
                    i++;
                }
                continue;
            }

            sb.Append(EscapeHtml(smb.Contet));
            i++;
        }
        
        return sb.ToString();
    }

    private static string EscapeHtml(char c) => c switch
    {
        '<' => "&lt;",
        '>' => "&gt;",
        '&' => "&amp;",
        _   => c.ToString(),
    };
    
        private static void ApplyFilterEmpty(Pair[] pairs, Symbol[] symbols, List<Pair> killed)
    {
        foreach (var p in pairs.Where(x => x.IsAlive))
        {
            int contentStart = p.Open.StartIdx + p.Open.Length;
            if (contentStart == p.Close.StartIdx) { p.IsAlive = false; killed.Add(p); }
        }
    }

    private static void ApplyFilterIntrawordSpace(Pair[] pairs, Symbol[] lexemes, List<Pair> killed)
    {
        foreach (var p in pairs.Where(x => x.IsAlive))
        {
            var left  = p.Open.StartIdx  > 0 ? lexemes[p.Open.StartIdx - 1] : null;
            int afterClose = p.Close.StartIdx + p.Close.Length;
            var right = afterClose < lexemes.Length ? lexemes[afterClose] : null;

            bool intraword =
                (left  != null && char.IsLetterOrDigit(left.Contet)) ||
                (right != null && char.IsLetterOrDigit(right.Contet));
            if (!intraword) continue;

            for (int i = p.Open.StartIdx + p.Open.Length; i < p.Close.StartIdx; i++)
                if (char.IsWhiteSpace(lexemes[i].Contet)) { p.IsAlive = false; killed.Add(p); break; }
        }
    }

    private static void ApplyFilterBoldInsideItalic(Pair[] pairs, List<Pair> killed)
    {
        var alive = pairs.Where(x => x.IsAlive).ToList();
        foreach (var bold in alive.Where(x => x.Length == 2))
        {
            if (!bold.IsAlive) continue;
            foreach (var italic in alive.Where(x => x.Length == 1))
            {
                if (IsContained(bold, italic)) { bold.IsAlive = false; killed.Add(bold); break; }
            }
        }
    }

    private static bool IsContained(Pair inner, Pair outer) =>
        outer.Open.StartIdx  < inner.Open.StartIdx &&
        inner.Close.StartIdx < outer.Close.StartIdx;

    private static void ApplyFilterIntersection(Pair[] pairs, List<Pair> killed)
    {
        var alive = pairs.Where(x => x.IsAlive).ToList();
        for (int i = 0; i < alive.Count; i++)
        {
            for (int j = i + 1; j < alive.Count; j++)
            {
                var p = alive[i]; var q = alive[j];
                
                if (!p.IsAlive || !q.IsAlive) continue;
                
                if (Intersect(p, q))
                {
                    p.IsAlive = false; killed.Add(p);
                    q.IsAlive = false; killed.Add(q);
                }
            }
        }
    }
    
    private static void ApplyFilterNestedSameType(Pair[] pairs, HashSet<(Subsequence, Subsequence)> forbidden, List<Pair> killed)
    {
        var alive = pairs.Where(x => x.IsAlive).ToList();
        foreach (var inner in alive)
        {
            if (!inner.IsAlive) continue;
            foreach (var outer in alive)
            {
                if (inner == outer) continue;
                if (inner.Length != outer.Length) continue;
                if (!IsContained(inner, outer)) continue;

                inner.IsAlive = false;
                killed.Add(inner);

                forbidden.Add((outer.Open, inner.Close));
                forbidden.Add((inner.Open, outer.Close));
            }
        }
    }

    private static bool Intersect(Pair a, Pair b)
    {
        int aOpen = a.Open.StartIdx, aClose = a.Close.StartIdx;
        int bOpen = b.Open.StartIdx, bClose = b.Close.StartIdx;
        
        bool bOverlapsRightOfA = aOpen < bOpen && bOpen < aClose && aClose < bClose;
        
        bool aOverlapsRightOfB = bOpen < aOpen && aOpen < bClose && bClose < aClose;

        return bOverlapsRightOfA || aOverlapsRightOfB;
    }

    private static void ApplyFilterDigitInside(Pair[] pairs, Symbol[] symbols, List<Pair> killed)
    {
        foreach (var p in pairs.Where(x => x.IsAlive))
        {
            for (int i = p.Open.StartIdx + p.Open.Length; i < p.Close.StartIdx; i++)
            {
                if (!char.IsDigit(symbols[i].Contet)) continue;
                p.IsAlive = false; killed.Add(p); break;
            }
        }
    }
}