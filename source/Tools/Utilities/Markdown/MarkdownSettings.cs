// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownSettings
    {
        public const string DefaultBlockCodeChars = "```";

        public const string DefaultInlineCodeDelimiter = "`";

        public const string DefaultEmphasisDelimiter = "*";

        public const string DefaultStrikethoughDelimiter = "~~";

        public const string DefaultListItemStart = "*";

        public const string DefaultTableDelimiter = "|";

        public const string DefaultIndentChars = "\t";

        public const string DefaultHorizontalRule = "___";

        public MarkdownSettings(
            string boldDelimiter = DefaultEmphasisDelimiter,
            string italicDelimiter = DefaultEmphasisDelimiter,
            string strikethroughDelimiter = DefaultStrikethoughDelimiter,
            string listItemStart = DefaultListItemStart,
            string tableDelimiter = DefaultTableDelimiter,
            string inlineCodeDelimiter = DefaultInlineCodeDelimiter,
            string codeBlockChars = DefaultBlockCodeChars,
            string horizontalRule = DefaultHorizontalRule,
            bool useTablePadding = true,
            bool useTableOuterPipe = true,
            MarkdownTableFormatting tableFormatting = MarkdownTableFormatting.Header,
            string indentChars = DefaultIndentChars,
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

        public MarkdownTableFormatting TableFormatting { get; }

        internal bool FormatTableHeader
        {
            get { return (TableFormatting & MarkdownTableFormatting.Header) != 0; }
        }

        internal bool FormatTableContent
        {
            get { return (TableFormatting & MarkdownTableFormatting.Content) != 0; }
        }

        public bool UseTablePadding { get; }

        public bool UseTableOuterPipe { get; }

        public string IndentChars { get; }

        public MarkdownEscaper Escaper { get; }

        internal string EscapeIf(bool condition, string value)
        {
            return (condition) ? Escaper.Escape(value) : value;
        }

        internal bool ShouldBeEscaped(char value)
        {
            return Escaper.ShouldBeEscaped(value);
        }
    }
}
