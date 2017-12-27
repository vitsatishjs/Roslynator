// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Roslynator.Markdown
{
    public static class MarkdownFactory
    {
        public static string StrikethroughDelimiter => "~~";

        public static string CodeDelimiter => "`";

        internal static char CodeDelimiterChar => '`';

        public static string TableDelimiter => "|";

        public static string CodeBlockChars => "```";

        public static string QuoteBlockStart => "> ";

        public static MarkdownText Text(string value, EmphasisOptions options = EmphasisOptions.None)
        {
            return new MarkdownText(value, options, escape: true);
        }

        public static MarkdownText RawText(string value, EmphasisOptions options = EmphasisOptions.None)
        {
            return new MarkdownText(value, options, escape: false);
        }

        public static MarkdownText Bold(string value)
        {
            return Text(value, EmphasisOptions.Bold);
        }

        public static string BoldDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "**";

            if (style == EmphasisStyle.Underscore)
                return "__";

            throw new ArgumentException("", nameof(style));
        }

        public static MarkdownText Italic(string value)
        {
            return Text(value, EmphasisOptions.Italic);
        }

        public static string ItalicDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "*";

            if (style == EmphasisStyle.Underscore)
                return "_";

            throw new ArgumentException("", nameof(style));
        }

        public static MarkdownText Strikethrough(string value)
        {
            return Text(value, EmphasisOptions.Strikethrough);
        }

        public static MarkdownText Code(string value)
        {
            return Text(value, EmphasisOptions.Code);
        }

        public static MarkdownJoin Join(string separator, IEnumerable<object> values, bool escape = true)
        {
            return new MarkdownJoin(separator, values, escape);
        }

        public static Heading Heading(string value, int level)
        {
            return new Heading(value, level);
        }

        public static Heading Heading1(string value = null)
        {
            return new Heading(value, 1);
        }

        public static Heading Heading2(string value = null)
        {
            return new Heading(value, 2);
        }

        public static Heading Heading3(string value = null)
        {
            return new Heading(value, 3);
        }

        public static Heading Heading4(string value = null)
        {
            return new Heading(value, 4);
        }

        public static Heading Heading5(string value = null)
        {
            return new Heading(value, 5);
        }

        public static Heading Heading6(string value = null)
        {
            return new Heading(value, 6);
        }

        public static string HeadingStart(int level)
        {
            switch (level)
            {
                case 1:
                    return "# ";
                case 2:
                    return "## ";
                case 3:
                    return "### ";
                case 4:
                    return "#### ";
                case 5:
                    return "##### ";
                case 6:
                    return "###### ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Header level cannot be less than 1 or greater than 6");
            }
        }

        public static string HeadingEnd(int level)
        {
            switch (level)
            {
                case 1:
                    return " #";
                case 2:
                    return " ##";
                case 3:
                    return " ###";
                case 4:
                    return " ####";
                case 5:
                    return " #####";
                case 6:
                    return " ######";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Header level cannot be less than 1 or greater than 6");
            }
        }

        public static ListItem ListItem(string value = null)
        {
            return new ListItem(value);
        }

        public static string ListItemStart(ListItemStyle style)
        {
            if (style == ListItemStyle.Asterisk)
                return "* ";

            if (style == ListItemStyle.Plus)
                return "+ ";

            if (style == ListItemStyle.Minus)
                return "- ";

            throw new ArgumentException("", nameof(style));
        }

        public static OrderedListItem OrderedListItem(int number, string value = null)
        {
            return new OrderedListItem(number, value);
        }

        public static string OrderedListItemStart(int number)
        {
            return number.ToString() + ". ";
        }

        public static TaskListItem TaskListItem(string value = null)
        {
            return new TaskListItem(value);
        }

        public static TaskListItem CompletedTaskListItem(string value = null)
        {
            return new TaskListItem(value, isCompleted: true);
        }

        public static string TaskListItemStart(bool isCompleted = false)
        {
            if (isCompleted)
            {
                return "- [x] ";
            }
            else
            {
                return "- [ ] ";
            }
        }

        public static Link Link(string text, string url)
        {
            return new Link(text, url);
        }

        public static Image Image(string text, string url)
        {
            return new Image(text, url);
        }

        public static CodeBlock CodeBlock(string value, string language = null)
        {
            return new CodeBlock(value, language);
        }

        public static QuoteBlock QuoteBlock(string value)
        {
            return new QuoteBlock(value);
        }

        public static MarkdownText HorizontalRule()
        {
            return RawText(MarkdownSettings.Default.HorizontalRule);
        }

        public static Table Table()
        {
            return new Table();
        }

        public static Table Table(params TableColumn[] columns)
        {
            Table table = Table();

            table.AddColumns(columns);

            return table;
        }

        public static Table Table(TableColumn column1, TableColumn column2)
        {
            Table table = Table();

            table.AddColumns(column1, column2);

            return table;
        }

        public static Table Table(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            Table table = Table();

            table.AddColumns(column1, column2, column3);

            return table;
        }

        public static Table Table(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            Table table = Table();

            table.AddColumns(column1, column2, column3, column4);

            return table;
        }

        public static Table Table(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            var table = new Table();

            table.AddColumns(column1, column2, column3, column4, column5);

            return table;
        }

        public static TableColumnCollection TableHeader(TableColumn column)
        {
            return new TableColumnCollection() { column };
        }

        public static TableColumnCollection TableHeader(TableColumn column1, TableColumn column2)
        {
            return new TableColumnCollection() { column1, column2 };
        }

        public static TableColumnCollection TableHeader(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            return new TableColumnCollection() { column1, column2, column3 };
        }

        public static TableColumnCollection TableHeader(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            return new TableColumnCollection() { column1, column2, column3, column4 };
        }

        public static TableColumnCollection TableHeader(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            return new TableColumnCollection() { column1, column2, column3, column4, column5 };
        }

        public static TableColumn TableHeader(string name, Alignment alignment = Alignment.Left)
        {
            return new TableColumn(name, alignment);
        }

        public static List<object> TableRow(object value)
        {
            return new List<object>() { value };
        }

        public static List<object> TableRow(object value1, object value2)
        {
            return new List<object>() { value1, value2 };
        }

        public static List<object> TableRow(object value1, object value2, object value3)
        {
            return new List<object>() { value1, value2, value3 };
        }

        public static List<object> TableRow(object value1, object value2, object value3, object value4)
        {
            return new List<object>() { value1, value2, value3, value4 };
        }

        public static List<object> TableRow(object value1, object value2, object value3, object value4, object value5)
        {
            return new List<object>() { value1, value2, value3, value4, value5 };
        }
    }
}
