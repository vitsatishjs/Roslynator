// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownSettings
    {
        public MarkdownSettings(
            string boldDelimiter = "*",
            string italicDelimiter = "*",
            string strikethroughDelimiter = "~~",
            string listItemStart = "*",
            string tableDelimiter = "|",
            string inlineCodeDelimiter = "`",
            string codeBlockChars = "```",
            string horizontalRule = "___",
            EmptyLineOptions headerOptions = EmptyLineOptions.AddEmptyLineBeforeAndAfter,
            EmptyLineOptions codeBlockOptions =  EmptyLineOptions.AddEmptyLineBeforeAndAfter,
            bool useTablePadding = true,
            bool useTableOuterPipe = true,
            TableFormatting tableFormatting = TableFormatting.Header,
            string indentChars = "\t",
            MarkdownEscaper escaper = null)
        {
            BoldDelimiter = boldDelimiter;
            ItalicDelimiter = italicDelimiter;
            StrikethroughDelimiter = strikethroughDelimiter;
            ListItemStart = listItemStart;
            TableDelimiter = tableDelimiter;
            InlineCodeDelimiter = inlineCodeDelimiter;
            CodeBlockChars = codeBlockChars;
            HorizontalRule = horizontalRule;
            HeaderOptions = headerOptions;
            CodeBlockOptions = codeBlockOptions;
            TableFormatting = tableFormatting;
            UseTablePadding = useTablePadding;
            UseTableOuterPipe = useTableOuterPipe;
            IndentChars = indentChars;
            Escaper = escaper ?? MarkdownEscaper.Default;
        }

        public static MarkdownSettings Default { get; } = new MarkdownSettings();

        public string BoldDelimiter { get; }

        public string ItalicDelimiter { get; }

        public string StrikethroughDelimiter { get; }

        public string ListItemStart { get; }

        public string TableDelimiter { get; }

        public string InlineCodeDelimiter { get; }

        public string CodeBlockChars { get; }

        public string HorizontalRule { get; }

        public EmptyLineOptions HeaderOptions { get; }

        internal bool AddEmptyLineBeforeHeader
        {
            get { return (HeaderOptions & EmptyLineOptions.AddEmptyLineBefore) != 0; }
        }

        internal bool AddEmptyLineAfterHeader
        {
            get { return (HeaderOptions & EmptyLineOptions.AddEmptyLineAfter) != 0; }
        }

        public EmptyLineOptions CodeBlockOptions { get; }

        internal bool AddEmptyLineBeforeCodeBlock
        {
            get { return (CodeBlockOptions & EmptyLineOptions.AddEmptyLineBefore) != 0; }
        }

        internal bool AddEmptyLineAfterCodeBlock
        {
            get { return (CodeBlockOptions & EmptyLineOptions.AddEmptyLineAfter) != 0; }
        }

        public TableFormatting TableFormatting { get; }

        internal bool FormatTableHeader
        {
            get { return (TableFormatting & TableFormatting.Header) != 0; }
        }

        internal bool FormatTableContent
        {
            get { return (TableFormatting & TableFormatting.Content) != 0; }
        }

        public bool UseTablePadding { get; }

        public bool UseTableOuterPipe { get; }

        public string IndentChars { get; }

        public MarkdownEscaper Escaper { get; }

        public string Escape(string value)
        {
            return Escaper.Escape(value);
        }

        internal string EscapeIf(bool condition, string value)
        {
            return (condition) ? Escaper.Escape(value) : value;
        }

        public bool ShouldBeEscaped(char value)
        {
            return Escaper.ShouldBeEscaped(value);
        }
    }
}
