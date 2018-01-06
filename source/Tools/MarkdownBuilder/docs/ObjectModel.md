* MObject
  * MElement 
    * MSimpleElememt (nothing)
      * Link
      * Image
      * Autolink
      * LinkReference
      * EntityCharacterReference
      * NumericCharacterReference
      * Comment
	  * MText
      * CodeText
	* MBlockElement ? (nothing)
      * IndentedCodeBlock
      * FencedCodeBlock
      * HorizontalRule
    * MContainer
      * Heading (Emphasis, MSimpleElement)
      * Emphasis (Emphasis, MSimpleElement)
        * BoldText
        * ItalicText
        * StrikethroughText
      * MBlockContainer (all except MDocument)
        * MDocument
        * BlockQuote
        * List ? (ListItem only)
        * ListItem
          * BulletListItem
          * OrderedListItem
          * TaskListItem
        * MJoin ?