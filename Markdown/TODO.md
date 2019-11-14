# Simple converter from Markdown to HTML

## Problem
Write `Markdown` project with `Md` class. Class has to method Render, that takes Markdown-like string and returns html formating String

### Process
* Parser parses document to tree with tags
    * There is Dictionary of tags of source language
    * Parser find the nearest open tag and try to find close tag
    * If there is close tag then tag append to the tree and inner tags check if they can be used
    * Every leaf have TagType NoneTag
* Builder builds outPut string from the tree
* We can change Parser and Builder for translator

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