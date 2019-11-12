# Simple converter from Markdown to HTML

## Problem
Write `Markdown` project with `Md` class. Class has to method Render, that takes Markdown-like string and returns html formating String

### Process
* Processor initialize by configuration
* Configuration contains parsers in right order
* Parser includes mark if it can have inner tags, open and close tags and method of parsing
* In render method we use two stacks: first - for strings that you need to parse, second
contains parsed string
* In the parsing process you receive morphemes in reverse order and so you have second stack, that
contains parsed morphemes that you do not need parse more
* in the conclusion string is formed from the second stack and returned from render method

### Optional classes/methods in the project (not in the class)
* Render full `HTML` doc with title
* Including `CSS` files

### TODO List
* Create structure of the project
  - [x] Create Interfaces that are used in the project
  - [ ] Create methods of Md class
* Realizing
  - [ ] __TESTS!!!__
  - [ ] Create parsers for basic morphemes
  - [ ] Create default configuration
  - [ ] 
* Optional TODO:
  - [ ] Load configuration from the file
  - [ ] Generate full HTML document with adding class attributes and ability to use CSS styling