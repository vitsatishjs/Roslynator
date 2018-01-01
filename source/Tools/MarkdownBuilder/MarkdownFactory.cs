// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown
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

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
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

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
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
                    throw new ArgumentOutOfRangeException(nameof(level), level, ErrorMessages.HeadingLevelMustBeInRangeFromOneToSix);
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
                    throw new ArgumentOutOfRangeException(nameof(level), level, ErrorMessages.HeadingLevelMustBeInRangeFromOneToSix);
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

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        public static OrderedListItem OrderedListItem(int number, string value = null)
        {
            return new OrderedListItem(number, value);
        }

        public static string OrderedListItemStart(int number)
        {
            return number.ToString() + ". ";
        }

        public static TaskListItem TaskListItem(string value = null, bool isCompleted = false)
        {
            return new TaskListItem(value, isCompleted);
        }

        public static TaskListItem CompletedTaskListItem(string value = null)
        {
            return TaskListItem(value, isCompleted: true);
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

        public static Image Image(string text, string url, string title = null)
        {
            return new Image(text, url, title);
        }

        public static Link Link(string text, string url, string title = null)
        {
            return new Link(text, url, title);
        }

        //TODO: TextOrLink
        public static IMarkdown LinkOrText(string text, string url = null, string title = null)
        {
            if (url == null)
                return Text(text);

            return new Link(text, url, title);
        }

        public static CodeBlock CodeBlock(string value, string language = null)
        {
            return new CodeBlock(value, language);
        }

        public static QuoteBlock QuoteBlock(string value)
        {
            return new QuoteBlock(value);
        }

        public static HorizontalRule HorizontalRule(HorizontalRuleStyle style, int count = 3, bool addSpaces = true)
        {
            return new HorizontalRule(style, count, addSpaces);
        }

        public static char HorizontalRuleChar(HorizontalRuleStyle style)
        {
            switch (style)
            {
                case HorizontalRuleStyle.Hyphen:
                    return '-';
                case HorizontalRuleStyle.Asterisk:
                    return '*';
                case HorizontalRuleStyle.Underscore:
                    return '_';
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
            }
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

        public static Table Table(TableColumn column)
        {
            Table table = Table();

            table.Columns.Add(column);

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

        public static TableColumnCollection TableHeader()
        {
            return new TableColumnCollection();
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

        public static TableColumnCollection TableHeader(params TableColumn[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            TableColumnCollection tableHeader = TableHeader();

            for (int i = 0; i < columns.Length; i++)
                tableHeader.Add(columns[i]);

            return tableHeader;
        }

        public static TableColumn TableColumn(string name, Alignment alignment = Alignment.Left)
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
