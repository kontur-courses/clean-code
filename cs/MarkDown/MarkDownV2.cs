using System.Buffers;
using System.Net.Mime;
using System.Text;
using MarkDown;

namespace MarkDownV2;

public class Md
{
    private readonly string neadChars = "_\\";
    private List<Symbol> symbols;
    
    public string Render(string markdown)
    {
        var paragraphs = markdown.Split("\\");

        foreach (paragraphs)
        {
            ParagraphToSymbols();
            SymbolsToSubsequences();
            CheckSubsequence();
            SubsequenceToPair();
            
            Dictionary<int, string> tags = new();
            
            foreach (Pair p in Pairs) {
                tags[p.Open.get_Stars]  = p.Length == 1 ? "<em>"  : "<strong>";
                tags[p.Close.get_Stars] = p.Length == 1 ? "</em>" : "</strong>"; 
            }
            
            
            var sb = new StringBuilder();

            for (int i = 0; i < symbols.Count; i++)
            {
                if (tags.TryGetValue(i, out var html))
                {
                    sb.Append(html);
                }
                else
                {
                    sb.Append(symbols[i]._symbol);
                }
            }

            sb.ToString();
        }
        
            
        return markdown;
    }
    
    private List<Symbol> ParagraphToSymbols() {} // на этом этапе проверяем экранирование
    
    private List<Subsequence> SymbolsToSubsequences() {} 
    
    private List<Subsequence> CheckSubsequence() {} //Тут заполняем флаги для последовательности
    
    private List<Pair> SubsequenceToPair() {} //Обьединям в пары через стэк
    
    
}