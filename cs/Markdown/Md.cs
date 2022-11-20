using System.Text;

namespace Markdown;

public class Md
{
    public static string Render(string text)
    {
        var segmentsStack = new Stack<TextSegment>();
        var mainSegment = new TextSegment(Symbols.EmptySymbol, null, false);
        var segment = mainSegment;
        
        for (var i = 0; i < text.Length; i++)
        {            
            if (Symbols.IsEscapeSymbol(text, i) is not null)
            {
                HandleEscapeSymbol(segment, text, ref i);
                continue;
            }

            if (text[i] == ' ' && segment.IsInsideWord)
                segment = segmentsStack.Pop();

            var symbol = Symbols.IsMarkdownSymbol(text, i);
            if (i == 0 || text[i - 1] == '\n')
            {
                if (symbol is not null)
                {
                    if (symbol == "#")
                    {
                        segment = AddSegment(segmentsStack, segment, symbol);
                        i += symbol.Length - 1;
                        continue;
                    }
                }
                else
                {
                    if (segment.StartSymbol == "#")
                        segment = CloseSegment(segmentsStack, segment);
                }
            }
            
            if (symbol is not null)
            {
                segment = HandleUnderlineSymbol(segmentsStack, segment, symbol, text, ref i);
                continue;
            }
            segment.AddSymbol(text[i]);
        }

        return mainSegment.Render();
    }

    private static void HandleEscapeSymbol(TextSegment segment, string text, ref int indexOfSymbol)
    {
        var nextSymbol = Symbols.IsMarkdownSymbol(text, indexOfSymbol + 1);
        if (nextSymbol is null)
        {
            segment.AddSymbol(text[indexOfSymbol]);
            if (indexOfSymbol + 1 != text.Length && Symbols.IsEscapeSymbol(text, indexOfSymbol + 1) is null)
                segment.AddSymbol(text[indexOfSymbol + 1]);
            indexOfSymbol++;
        }
        else
        {
            segment.AddSymbol(nextSymbol);
            indexOfSymbol += nextSymbol.Length;
        }
    }

    private static TextSegment HandleUnderlineSymbol(Stack<TextSegment> segmentsStack, TextSegment segment, Symbol symbol, string text, ref int indexOfSymbol)
    {
        if (segment.StartSymbol == symbol)
            segment = HandleUnderlineCloseSymbol(segmentsStack, segment, symbol, text, ref indexOfSymbol);
        else
            segment = HandleUnderlineOpenSymbol(segmentsStack, segment, symbol, text, ref indexOfSymbol);

        indexOfSymbol += symbol.Length - 1;
        return segment;
    }
    
    private static TextSegment HandleUnderlineOpenSymbol(Stack<TextSegment> segmentsStack, TextSegment segment, Symbol symbol, string text, ref int indexOfSymbol)
    {
        if (IsNextSymbolEmpty(text, indexOfSymbol))
        {
            segment.AddSymbol(symbol);
        }
        else if (IsSymbolInMiddleOfWord(text, indexOfSymbol, symbol))
        {
            segment = AddSegment(segmentsStack, segment, symbol, true);
        }
        else
        {
            segment = AddSegment(segmentsStack, segment, symbol);
        }
        return segment;
    }
    
    private static TextSegment HandleUnderlineCloseSymbol(Stack<TextSegment> segmentsStack, TextSegment segment, Symbol symbol, string text, ref int indexOfSymbol)
    {
        if (IsPreviousSymbolEmpty(text, indexOfSymbol))
            segment.AddSymbol(symbol);
        else
            segment = CloseSegment(segmentsStack, segment);
        return segment;
    }
    
    private static TextSegment AddSegment(Stack<TextSegment> segmentsStack, TextSegment segment, Symbol symbol, bool isInsideWord = false)
    {
        var innerSegment = new TextSegment(symbol, segment, isInsideWord);
        segment.AddInnerSegment(innerSegment);
        segmentsStack.Push(segment);
        return innerSegment;
    }

    private static TextSegment CloseSegment(Stack<TextSegment> segmentsStack, TextSegment segment)
    {
        segment.Close();
        return segmentsStack.Pop();
    }

    private static bool IsSymbolInMiddleOfWord(string text, int indexOfSymbol, Symbol symbol)
    {
        return IsPreviousSymbolEmpty(text, indexOfSymbol) == false && indexOfSymbol != 0 &&
               IsNextSymbolEmpty(text, indexOfSymbol + symbol.Length - 1) == false && indexOfSymbol + 1 != text.Length &&
               Symbols.IsEscapeSymbol(text, indexOfSymbol - 1) is null;
    }
    
    private static bool IsPreviousSymbolEmpty(string text, int indexOfSymbol)
    {
        if (indexOfSymbol == 0)
            return false;
        return char.IsWhiteSpace(text[indexOfSymbol - 1]);
    }
    
    private static bool IsNextSymbolEmpty(string text, int indexOfSymbol)
    {
        if (indexOfSymbol + 1 == text.Length)
            return true;
        return char.IsWhiteSpace(text[indexOfSymbol + 1]);
    }

    private static bool NextSymbolIsMarkdownSymbol(string text, int indexOfSymbol)
    {
        return Symbols.IsMarkdownSymbol(text, indexOfSymbol) != null;
    }
}