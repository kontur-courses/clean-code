# TODO
* Переназвать Markup на Token или MarkdownToken или что-нибудь еще
* MarkupExtansions.ValidOpeningPosition и MarkupExtansions.ValidClosingPosition сделать более читаемым (разбить на несколько методов или разбить одно большое условие на несколько более простых)
* Убрать состояние из MarkupFinder
* GetMarkupBoarders переименовать 
* GetSortedPositionsWithTags переделать Tuple на ValueTuple
* MarkupExtensionsTests дописать тесты, которые возвращают false
* MarkupFinderTests завернуть for в LINQ
* Сделать SetUp на инициализацию Markup
* Переименовать MarkupPosition

# DONE
* Исправить зоны видимости 
* Перенести инициализацию Markup'ов в MarkupFinder
* Заменить SortedSet на List в MarkupFinder.GetPositionsForMarkup
* MarkupFinder Создавать HashSet не в GetMarkupBoarders, а в GetPositionsForMarkup
