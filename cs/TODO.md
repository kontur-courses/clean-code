# TODO 
* MarkupExtensionsTests дописать тесты, которые возвращают false
* MarkupFinderTests завернуть for в LINQ
* —делать SetUp на инициализацию Markup
* ќтрефакторить весь код
* ѕодумать над оптимизацией поиска закрывающих тегов

# DONE
* »справить зоны видимости 
* ѕеренести инициализацию Markup'ов в MarkupFinder
* «аменить SortedSet на List в MarkupFinder.GetPositionsForMarkup
* MarkupFinder —оздавать HashSet не в GetMarkupBoarders, а в GetPositionsForMarkup
* MarkupExtansions.ValidOpeningPosition и MarkupExtansions.ValidClosingPosition сделать более читаемым (разбить на несколько методов или разбить одно большое условие на несколько более простых)
* ѕереназвать Markup на Token или MarkdownToken или что-нибудь еще
* ”брать состо€ние из MarkupFinder
* GetSortedPositionsWithTags переделать Tuple на ValueTuple (убрал Tuple)
* Md2HtmlTranslator убрать ContainsKey, заменить на TryGetValue
* GetMarkupBoarders переименовать