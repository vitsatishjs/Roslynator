* MObject
  * MElement 
    * MSimpleElememt (-)
      * Link
      * Image
      * Autolink
      * LinkReference
      * EntityReference
      * CharacterReference
      * Comment
	  * MText
      * InlineCode
    * MSimpleBlock (-)
      * IndentedCodeBlock
      * FencedCodeBlock
      * HorizontalRule
    * MContainer
      * Heading (Emphasis, MSimpleElement)
      * Emphasis (Emphasis, MSimpleElement) RENAME
        * Bold
        * Italic
        * Strikethrough
      * MTable
      * TableColumn
      * TableRow
      * MBlockContainer (all except MDocument)
        * MDocument
        * BlockQuote
        * UnorderedList
        * OrderedList
        * TaskList
        * ListItem
        * OrderedListItem
        * TaskListItem