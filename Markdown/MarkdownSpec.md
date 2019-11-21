## Definitions
* All in [] are additional symbols which don't influence ont the representation

## Bold and Italic
Can be used everywhere
* `_Italic line_`<br>
_Italic line_
* `___Bold Line___**`<br>
**Bold Line**
* `___Bold italic Line___`<br>
***Bold italic line***

* Note: 
    * After open and before close underscore non-space character must be
    * If there are not spaces before and after underscores, then it's not a tag,
    it's part of the line
        * It's _not_italic_tag
    * Doubled underscores can include single underscores but
    single underscores can't include doubled underscores
        * __It _can_ work__
        * _It \_\_can not\_\_ work_
* Text can include escape symbols
    * \_It is not included in \<em> tag\_

## Headers
Can be used only in the start of a line
* `# h1 Header[ #*]`
    # h1 Header
---
* `## h2 Header[ #*]`
    ## h2 Header
---
* `### h3 Header[ #*]`
    ### h3 Header
---
* `#### h4 Header[ #*]`
    #### h4 Header
---
* `##### h5 Header[ #*]`
    ##### h5 Header
---
* `###### h6 Header[ #*]`
    ###### h6 Header
---

## Paragraphs
If you place two new-line symbols you split text for two paragraphs. One new-line symbol have no influence. Example:<br/>

    First paragraph
    Still first paragraph

    Second paragraph

## Highlight line and blocks
* If you want to `highlight part of the non-formatted text` you can use \`this\` or \`\`this\`\` 
(or more pair \` if you need use this symbol in the line) for one-line highlight 
* If you want to highlight whole block you should use \```\<newLine>Some text\<newline>\```
```
## Some block without __formatting__
```

## Highlight with formatting
>If you want to highlight line with formatting you should use \> before first line of paragraph

## Special symbols and escape-characters
All symbols like \< and \> that uses in HTML will be translatted to their escape-characters. So you can not put html tags in the source document to use them in the HTML document
