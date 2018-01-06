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

        public static string QuoteBlockStart => "> ";

        public static MText NewLine { get; } = new MText(Environment.NewLine);

        public static MEmphasis Text(EmphasisOptions options, object content)
        {
            return new MEmphasis(options, content);
        }

        public static MEmphasis Text(EmphasisOptions options, params object[] content)
        {
            return new MEmphasis(options, content);
        }

        public static MEmphasis Bold(object content)
        {
            return Text(EmphasisOptions.Bold, content);
        }

        public static MEmphasis Bold(params object[] content)
        {
            return Text(EmphasisOptions.Bold, content);
        }

        public static string BoldDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "**";

            if (style == EmphasisStyle.Underscore)
                return "__";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        public static MEmphasis Italic(object content)
        {
            return Text(EmphasisOptions.Italic, content);
        }

        public static MEmphasis Italic(params object[] content)
        {
            return Text(EmphasisOptions.Italic, content);
        }

        public static string ItalicDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "*";

            if (style == EmphasisStyle.Underscore)
                return "_";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        public static MEmphasis Strikethrough(object content)
        {
            return Text(EmphasisOptions.Strikethrough, content);
        }

        public static MEmphasis Strikethrough(params object[] content)
        {
            return Text(EmphasisOptions.Strikethrough, content);
        }

        public static CodeText Code(string text)
        {
            return new CodeText(text);
        }

        public static IEnumerable<MElement> Join(MElement separator, params MElement[] values)
        {
            return Join(separator, (IEnumerable<MElement>)values);
        }

        public static IEnumerable<MElement> Join(MElement separator, IEnumerable<MElement> values)
        {
            bool addSeparator = false;

            foreach (MElement value in values)
            {
                if (addSeparator)
                {
                    yield return separator;
                }
                else
                {
                    addSeparator = true;
                }

                yield return value;
            }
        }

        public static Heading Heading(int level, object content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading(int level, params object[] content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading1(object content)
        {
            return new Heading(1, content);
        }

        public static Heading Heading1(params object[] content)
        {
            return new Heading(1, content);
        }

        public static Heading Heading2(object content)
        {
            return new Heading(2, content);
        }

        public static Heading Heading2(params object[] content)
        {
            return new Heading(2, content);
        }

        public static Heading Heading3(object content)
        {
            return new Heading(3, content);
        }

        public static Heading Heading3(params object[] content)
        {
            return new Heading(3, content);
        }

        public static Heading Heading4(object content)
        {
            return new Heading(4, content);
        }

        public static Heading Heading4(params object[] content)
        {
            return new Heading(4, content);
        }

        public static Heading Heading5(object content)
        {
            return new Heading(5, content);
        }

        public static Heading Heading5(params object[] content)
        {
            return new Heading(5, content);
        }

        public static Heading Heading6(object content)
        {
            return new Heading(6, content);
        }

        public static Heading Heading6(params object[] content)
        {
            return new Heading(6, content);
        }

        internal static char HeadingStartChar(HeadingStyle style)
        {
            switch (style)
            {
                case HeadingStyle.NumberSign:
                    return '#';
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
            }
        }

        public static BulletListItem ListItem(object content)
        {
            return new BulletListItem(content);
        }

        public static BulletListItem ListItem(params object[] content)
        {
            return new BulletListItem(content);
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

        public static OrderedListItem OrderedListItem(int number, object content)
        {
            return new OrderedListItem(number, content);
        }

        public static OrderedListItem OrderedListItem(int number, params object[] content)
        {
            return new OrderedListItem(number, content);
        }

        public static string OrderedListItemStart(int number)
        {
            return number.ToString() + ". ";
        }

        public static TaskListItem TaskListItem(bool isCompleted, object content)
        {
            return new TaskListItem(isCompleted, content);
        }

        public static TaskListItem TaskListItem(bool isCompleted, params object[] content)
        {
            return new TaskListItem(isCompleted, content);
        }

        public static TaskListItem CompletedTaskListItem(object content)
        {
            return TaskListItem(isCompleted: true, content: content);
        }

        public static TaskListItem CompletedTaskListItem(params object[] content)
        {
            return TaskListItem(isCompleted: true, content: content);
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

        public static MElement LinkOrText(string text, string url = null, string title = null)
        {
            if (string.IsNullOrEmpty(url))
                return new MText(text);

            return new Link(text, url, title);
        }

        public static FencedCodeBlock FencedCodeBlock(string value, string info = null)
        {
            return new FencedCodeBlock(value, info);
        }

        public static IndentedCodeBlock IndentedCodeBlock(string value)
        {
            return new IndentedCodeBlock(value);
        }

        public static QuoteBlock QuoteBlock(object content)
        {
            return new QuoteBlock(content);
        }

        public static QuoteBlock QuoteBlock(params object[] content)
        {
            return new QuoteBlock(content);
        }

        public static HorizontalRule HorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            return new HorizontalRule(style, count, space);
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

        public static HtmlEntity HtmlEntity(int number)
        {
            return new HtmlEntity(number);
        }

        public static Table<T> Table<T>()
        {
            return new Table<T>();
        }

        public static Table<T> Table<T>(
            IEnumerable<TableColumn> columns,
            IEnumerable<Func<T, object>> selectors,
            params T[] items)
        {
            return Table(columns, selectors, (IEnumerable<T>)items);
        }

        public static Table<T> Table<T>(
            IEnumerable<TableColumn> columns,
            IEnumerable<Func<T, object>> selectors,
            IEnumerable<T> items)
        {
            Table<T> table = Table<T>();

            table.AddColumns(columns);

            foreach (Func<T, object> selector in selectors)
                table.Selectors.Add(selector);

            foreach (T item in items)
                table.Rows.Add(item);

            return table;
        }

        public static Table<T> Table<T>(params TableColumn[] columns)
        {
            Table<T> table = Table<T>();

            table.AddColumns(columns);

            return table;
        }

        public static Table<T> Table<T>(TableColumn column)
        {
            Table<T> table = Table<T>();

            table.Columns.Add(column);

            return table;
        }

        public static Table<T> Table<T>(TableColumn column1, TableColumn column2)
        {
            Table<T> table = Table<T>();

            table.AddColumns(column1, column2);

            return table;
        }

        public static Table<T> Table<T>(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            Table<T> table = Table<T>();

            table.AddColumns(column1, column2, column3);

            return table;
        }

        public static Table<T> Table<T>(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            Table<T> table = Table<T>();

            table.AddColumns(column1, column2, column3, column4);

            return table;
        }

        public static Table<T> Table<T>(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            var table = new Table<T>();

            table.AddColumns(column1, column2, column3, column4, column5);

            return table;
        }

        public static SimpleTable SimpleTable()
        {
            return new SimpleTable();
        }

        public static SimpleTable SimpleTable(IEnumerable<TableColumn> columns, params TableRow[] rows)
        {
            return SimpleTable(columns, (IEnumerable<TableRow>)rows);
        }

        public static SimpleTable SimpleTable(IEnumerable<TableColumn> columns, IEnumerable<TableRow> rows)
        {
            SimpleTable table = SimpleTable();

            table.AddColumns(columns);

            foreach (TableRow row in rows)
                table.Rows.Add(row);

            return table;
        }

        public static SimpleTable SimpleTable(params TableColumn[] columns)
        {
            SimpleTable table = SimpleTable();

            table.AddColumns(columns);

            return table;
        }

        public static SimpleTable SimpleTable(TableColumn column)
        {
            SimpleTable table = SimpleTable();

            table.Columns.Add(column);

            return table;
        }

        public static SimpleTable SimpleTable(TableColumn column1, TableColumn column2)
        {
            SimpleTable table = SimpleTable();

            table.AddColumns(column1, column2);

            return table;
        }

        public static SimpleTable SimpleTable(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            SimpleTable table = SimpleTable();

            table.AddColumns(column1, column2, column3);

            return table;
        }

        public static SimpleTable SimpleTable(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            SimpleTable table = SimpleTable();

            table.AddColumns(column1, column2, column3, column4);

            return table;
        }

        public static SimpleTable SimpleTable(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            var table = new SimpleTable();

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

        //TODO: test
        public static TableRow TableRow(params object[] values)
        {
            var row = new TableRow();

            foreach (object value in values)
                row.Add(value);

            return row;
        }

        public static TableRow TableRow(object value)
        {
            return new TableRow() { value };
        }

        public static TableRow TableRow(object value1, object value2)
        {
            return new TableRow() { value1, value2 };
        }

        public static TableRow TableRow(object value1, object value2, object value3)
        {
            return new TableRow() { value1, value2, value3 };
        }

        public static TableRow TableRow(object value1, object value2, object value3, object value4)
        {
            return new TableRow() { value1, value2, value3, value4 };
        }

        public static TableRow TableRow(object value1, object value2, object value3, object value4, object value5)
        {
            return new TableRow() { value1, value2, value3, value4, value5 };
        }
    }
}
