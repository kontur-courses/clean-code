# Simple converter from Markdown to HTML

## Problem
Write `Markdown` project with `Md` class. Class has to method Render, that takes Markdown-like string and returns html formating String

## Solve

### Decomposition of processor
* Splitter - splits input string into morphemes
* Parser/Translator - find pair and non-pair tags and represents them to html code
* Assembler - assembles all tags into one html string

### Optional functions of processor
* Ability to load additional/other configuration of processor from a file

### Optional classes/methods in the project (not in the class)
* Render full `HTML` doc with title
* Including `CSS` files

TODO List
- [ ] Make splitter by morphemes
- [ ] Make parser-translator
- [ ] Make assembler to full HTML page

OPTIONAL TASKS
- [ ] Ability to load configurations
- [ ] Generating full HTML document with ability of import CSS files