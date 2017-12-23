// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Markdown
{
    public class MarkdownSettings
    {
        public MarkdownSettings(
            string boldDelimiter = "**",
            string italicDelimiter = "*",
            string strikethroughDelimiter = "~~",
            ListItemStyle listItemStyle = ListItemStyle.Asterisk,
            string tableDelimiter = "|",
            string codeDelimiter = "`",
            string codeBlockChars = "```",
            string horizontalRule = "- - -",
            EmptyLineOptions headerOptions = EmptyLineOptions.EmptyLineBeforeAndAfter,
            EmptyLineOptions codeBlockOptions = EmptyLineOptions.EmptyLineBeforeAndAfter,
            bool useTablePadding = true,
            bool useTableOuterPipe = true,
            TableFormatting tableFormatting = TableFormatting.Header,
            bool allowLinkWithoutUrl = true,
            bool closeHeading = true,
            string indentChars = "  ",
            Func<char, bool> shouldBeEscaped = null)
        {
            BoldDelimiter = boldDelimiter;
            ItalicDelimiter = italicDelimiter;
            StrikethroughDelimiter = strikethroughDelimiter;
            ListItemStyle = listItemStyle;
            TableDelimiter = tableDelimiter;
            CodeDelimiter = codeDelimiter;
            CodeBlockChars = codeBlockChars;
            HorizontalRule = horizontalRule;
            HeadingOptions = headerOptions;
            CodeBlockOptions = codeBlockOptions;
            TableFormatting = tableFormatting;
            UseTablePadding = useTablePadding;
            UseTableOuterPipe = useTableOuterPipe;
            AllowLinkWithoutUrl = allowLinkWithoutUrl;
            CloseHeading = closeHeading;
            IndentChars = indentChars;
            ShouldBeEscaped = shouldBeEscaped ?? MarkdownEscaper.ShouldBeEscaped;
        }

        public static MarkdownSettings Default { get; } = new MarkdownSettings();

        public string BoldDelimiter { get; }

        public string AlternativeBoldDelimiter
        {
            get { return (BoldDelimiter == "**") ? "__" : "**"; }
        }

        public string ItalicDelimiter { get; }

        public string AlternativeItalicDelimiter
        {
            get { return (ItalicDelimiter == "*") ? "_" : "*"; }
        }

        public string StrikethroughDelimiter { get; }

        public ListItemStyle ListItemStyle { get; }

        public string TableDelimiter { get; }

        public string CodeDelimiter { get; }

        public string CodeBlockChars { get; }

        public string HorizontalRule { get; }

        public EmptyLineOptions HeadingOptions { get; }

        internal bool EmptyLineBeforeHeading
        {
            get { return (HeadingOptions & EmptyLineOptions.EmptyLineBefore) != 0; }
        }

        internal bool EmptyLineAfterHeading
        {
            get { return (HeadingOptions & EmptyLineOptions.EmptyLineAfter) != 0; }
        }

        public EmptyLineOptions CodeBlockOptions { get; }

        internal bool EmptyLineBeforeCodeBlock
        {
            get { return (CodeBlockOptions & EmptyLineOptions.EmptyLineBefore) != 0; }
        }

        internal bool EmptyLineAfterCodeBlock
        {
            get { return (CodeBlockOptions & EmptyLineOptions.EmptyLineAfter) != 0; }
        }

        public TableFormatting TableFormatting { get; }

        public bool UseTablePadding { get; }

        public bool UseTableOuterPipe { get; }

        public bool AllowLinkWithoutUrl { get; }

        public bool CloseHeading { get; }

        public string IndentChars { get; }

        public Func<char, bool> ShouldBeEscaped { get; }
    }
}
