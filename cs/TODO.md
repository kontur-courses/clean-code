# TODO
* GetMarkupBoarders переименовать 
* GetSortedPositionsWithTags переделать Tuple на ValueTuple
* MarkupExtensionsTests дописать тесты, которые возвращают false
* MarkupFinderTests завернуть for в LINQ
* Сделать SetUp на инициализацию Markup
* Переименовать MarkupPosition
* Отрефакторить весь код

# DONE
* Исправить зоны видимости 
* Перенести инициализацию Markup'ов в MarkupFinder
* Заменить SortedSet на List в MarkupFinder.GetPositionsForMarkup
* MarkupFinder Создавать HashSet не в GetMarkupBoarders, а в GetPositionsForMarkup
* MarkupExtansions.ValidOpeningPosition и MarkupExtansions.ValidClosingPosition сделать более читаемым (разбить на несколько методов или разбить одно большое условие на несколько более простых)
* Переназвать Markup на Token или MarkdownToken или что-нибудь еще
* Убрать состояние из MarkupFinder
