// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Roslynator.Markdown.MarkdownFactory;

namespace Roslynator.Markdown
{
    public class MarkdownBuilder
    {
        private MarkdownSettings _settings;

        public MarkdownBuilder(MarkdownSettings settings = null)
            : this(new StringBuilder(), settings)
        {
        }

        public MarkdownBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            Settings = settings ?? MarkdownSettings.Default;
            StringBuilder = sb;
        }

        public MarkdownSettings Settings
        {
            get { return _settings; }
            set { _settings = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public int IndentLevel { get; private set; }

        public int QuoteLevel { get; private set; }

        public StringBuilder StringBuilder { get; }

        public int Length => StringBuilder.Length;

        private string BoldDelimiter => BoldDelimiter(Settings.BoldStyle);

        private string ItalicDelimiter => ItalicDelimiter(Settings.ItalicStyle);

        private string AlternativeItalicDelimiter => ItalicDelimiter(Settings.AlternativeItalicStyle);

        private ListItemStyle ListItemStyle => Settings.ListItemStyle;

        private string ListItemStart => ListItemStart(ListItemStyle);

        private string IndentChars => Settings.IndentChars;

        private bool AddEmptyLineBeforeHeading => Settings.EmptyLineBeforeHeading;

        private bool AddEmptyLineAfterHeading => Settings.EmptyLineAfterHeading;

        private bool AddEmptyLineBeforeCodeBlock => Settings.EmptyLineBeforeCodeBlock;

        private bool AddEmptyLineAfterCodeBlock => Settings.EmptyLineAfterCodeBlock;

        private TableOptions TableOptions => Settings.TableOptions;

        private bool FormatTableHeader => (TableOptions & TableOptions.FormatHeader) != 0;

        private bool FormatTableContent => (TableOptions & TableOptions.FormatContent) != 0;

        private bool TableOuterPipe => Settings.TableOuterPipe;

        private bool TablePadding => Settings.TablePadding;

        private bool AllowLinkWithoutUrl => Settings.AllowLinkWithoutUrl;

        private bool UnderlineHeading1 => Settings.UnderlineHeading1;

        private bool UnderlineHeading2 => Settings.UnderlineHeading2;

        private bool CloseHeading => Settings.CloseHeading;

        public char this[int index]
        {
            get { return StringBuilder[index]; }
        }

        public MarkdownBuilder IncreaseIndentLevel()
        {
            IndentLevel++;
            return this;
        }

        public MarkdownBuilder DecreaseIndentLevel()
        {
            if (IndentLevel == 0)
                throw new InvalidOperationException($"{nameof(IndentLevel)} cannot be less than 0.");

            IndentLevel--;
            return this;
        }

        public MarkdownBuilder IncreaseQuoteLevel()
        {
            QuoteLevel++;
            return this;
        }

        public MarkdownBuilder DecreaseQuoteLevel()
        {
            if (QuoteLevel == 0)
                throw new InvalidOperationException($"{nameof(QuoteLevel)} cannot be less than 0.");

            QuoteLevel--;
            return this;
        }

        public MarkdownBuilder AppendIndentation()
        {
            for (int i = 0; i < IndentLevel; i++)
                AppendRaw(IndentChars);

            return this;
        }

        public MarkdownBuilder AppendBold(string value)
        {
            return AppendDelimiter(BoldDelimiter, value);
        }

        public MarkdownBuilder AppendBold(object value)
        {
            return AppendDelimiter(BoldDelimiter, value);
        }

        public MarkdownBuilder AppendBold(params object[] values)
        {
            return AppendDelimiter(BoldDelimiter, values);
        }

        public MarkdownBuilder AppendBoldDelimiter()
        {
            return AppendRaw(BoldDelimiter);
        }

        public MarkdownBuilder AppendItalic(string value)
        {
            return AppendDelimiter(ItalicDelimiter, value);
        }

        public MarkdownBuilder AppendItalic(object value)
        {
            return AppendDelimiter(ItalicDelimiter, value);
        }

        public MarkdownBuilder AppendItalic(params object[] values)
        {
            return AppendDelimiter(ItalicDelimiter, values);
        }

        public MarkdownBuilder AppendItalicDelimiter()
        {
            return AppendRaw(ItalicDelimiter);
        }

        public MarkdownBuilder AppendStrikethrough(string value)
        {
            return AppendDelimiter(StrikethroughDelimiter, value);
        }

        public MarkdownBuilder AppendStrikethrough(object value)
        {
            return AppendDelimiter(StrikethroughDelimiter, value);
        }

        public MarkdownBuilder AppendStrikethrough(params object[] values)
        {
            return AppendDelimiter(StrikethroughDelimiter, values);
        }

        public MarkdownBuilder AppendStrikethroughDelimiter()
        {
            return AppendRaw(StrikethroughDelimiter);
        }

        public MarkdownBuilder AppendCode(string value)
        {
            AppendRaw(CodeDelimiter);

            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == CodeDelimiterChar)
                    AppendRaw(" ");

                Append(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);

                if (value[value.Length - 1] == CodeDelimiterChar)
                    AppendRaw(" ");
            }

            AppendRaw(CodeDelimiter);

            return this;
        }

        public MarkdownBuilder AppendCode(object value)
        {
            return AppendCode(value, Array.Empty<object>());
        }

        public MarkdownBuilder AppendCode(params object[] values)
        {
            return AppendCode(null, values);
        }

        private MarkdownBuilder AppendCode(object value, object[] additionalValues)
        {
            CodeMarkdownBuilder mb = CodeMarkdownBuilderCache.GetInstance();

            mb.Append(value);
            mb.AppendRange(additionalValues);

            string s = CodeMarkdownBuilderCache.GetResultAndFree(mb);

            AppendRaw(CodeDelimiter);

            if (s.Length > 0)
            {
                if (s[0] == CodeDelimiterChar)
                    AppendRaw(" ");

                AppendRaw(s);

                if (s[s.Length - 1] == CodeDelimiterChar)
                    AppendRaw(" ");
            }

            AppendRaw(CodeDelimiter);
            return this;
        }

        public MarkdownBuilder AppendCodeDelimiter()
        {
            return AppendRaw(CodeDelimiter);
        }

        public MarkdownBuilder AppendHeading1(string value)
        {
            return AppendHeading(1, value);
        }

        public MarkdownBuilder AppendHeading1(object value = null)
        {
            return AppendHeading(1, value);
        }

        public MarkdownBuilder AppendHeading1(params object[] value)
        {
            return AppendHeading(1, value);
        }

        public MarkdownBuilder AppendHeading2(string value)
        {
            return AppendHeading(2, value);
        }

        public MarkdownBuilder AppendHeading2(object value = null)
        {
            return AppendHeading(2, value);
        }

        public MarkdownBuilder AppendHeading2(params object[] value)
        {
            return AppendHeading(2, value);
        }

        public MarkdownBuilder AppendHeading3(string value)
        {
            return AppendHeading(3, value);
        }

        public MarkdownBuilder AppendHeading3(object value = null)
        {
            return AppendHeading(3, value);
        }

        public MarkdownBuilder AppendHeading3(params object[] value)
        {
            return AppendHeading(3, value);
        }

        public MarkdownBuilder AppendHeading4(string value)
        {
            return AppendHeading(4, value);
        }

        public MarkdownBuilder AppendHeading4(object value = null)
        {
            return AppendHeading(4, value);
        }

        public MarkdownBuilder AppendHeading4(params object[] value)
        {
            return AppendHeading(4, value);
        }

        public MarkdownBuilder AppendHeading5(string value)
        {
            return AppendHeading(5, value);
        }

        public MarkdownBuilder AppendHeading5(object value = null)
        {
            return AppendHeading(5, value);
        }

        public MarkdownBuilder AppendHeading5(params object[] value)
        {
            return AppendHeading(5, value);
        }

        public MarkdownBuilder AppendHeading6(string value)
        {
            return AppendHeading(6, value);
        }

        public MarkdownBuilder AppendHeading6(object value = null)
        {
            return AppendHeading(6, value);
        }

        public MarkdownBuilder AppendHeading6(params object[] value)
        {
            return AppendHeading(6, value);
        }

        public MarkdownBuilder AppendHeading(int level, string value = null)
        {
            return AppendHeading(level, value, Array.Empty<object>());
        }

        public MarkdownBuilder AppendHeading(int level, object value = null)
        {
            return AppendHeading(level, value, Array.Empty<object>());
        }

        public MarkdownBuilder AppendHeading(int level, params object[] values)
        {
            return AppendHeading(level, null, values);
        }

        private MarkdownBuilder AppendHeading(int level, object value, object[] additionalValues)
        {
            bool underline = (level == 1 && UnderlineHeading1)
                || (level == 2 && UnderlineHeading2);

            AppendEmptyLineIf(AddEmptyLineBeforeHeading);
            AppendQuoteIndentation();

            if (!underline)
                AppendRaw(HeadingStart(level));

            int length = Length;

            Append(value);
            AppendRange(additionalValues);

            length = Length - length;

            if (length == 0)
                return this;

            if (underline
                && CloseHeading)
            {
                AppendRaw(HeadingEnd(level));
            }

            AppendLineIfNecessary();

            if (underline)
            {
                AppendQuoteIndentation();
                AppendRaw((level == 1) ? '=' : '-', length);
                AppendLine();
            }

            AppendEmptyLineIf(AddEmptyLineAfterHeading);
            return this;
        }

        public MarkdownBuilder AppendHeadingStart(int level)
        {
            return AppendRaw(HeadingStart(level));
        }

        public MarkdownBuilder AppendListItem(string value)
        {
            return AppendItem(prefix1: null, prefix2: ListItemStart, value: value);
        }

        public MarkdownBuilder AppendListItem(object value)
        {
            return AppendItem(prefix1: null, prefix2: ListItemStart, value: value);
        }

        public MarkdownBuilder AppendListItem(params object[] values)
        {
            return AppendItem(prefix1: null, prefix2: ListItemStart, values: values);
        }

        public MarkdownBuilder AppendListItemStart()
        {
            return AppendRaw(ListItemStart);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, string value)
        {
            return AppendItem(prefix1: number.ToString(), prefix2: ". ", value: value);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, object value)
        {
            return AppendItem(prefix1: number.ToString(), prefix2: ". ", value: value);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, params object[] values)
        {
            return AppendItem(prefix1: number.ToString(), prefix2: ". ", values: values);
        }

        public MarkdownBuilder AppendOrderedListItemStart(int number)
        {
            AppendRaw(number);
            return AppendRaw(". ");
        }

        public MarkdownBuilder AppendTaskListItem(string value)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(), value: value);
        }

        public MarkdownBuilder AppendTaskListItem(object value)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(), value: value);
        }

        public MarkdownBuilder AppendTaskListItem(params object[] values)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(), values: values);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(string value)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(isCompleted: true), value: value);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(object value)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(isCompleted: true), value: value);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(params object[] values)
        {
            return AppendItem(prefix1: null, prefix2: TaskListItemStart(isCompleted: true), values: values);
        }

        public MarkdownBuilder AppendTaskListItemStart(bool isCompleted = false)
        {
            return AppendRaw(TaskListItemStart(isCompleted));
        }

        private MarkdownBuilder AppendItem(object prefix1, string prefix2, string value)
        {
            return AppendLineMarkdown(
                prefix1: prefix1,
                prefix2: prefix2,
                suffix: null,
                indent: true,
                emptyLineBefore: false,
                emptyLineAfter: false,
                value: value);
        }

        private MarkdownBuilder AppendItem(object prefix1, string prefix2, object value)
        {
            return AppendLineMarkdown(
                prefix1: prefix1,
                prefix2: prefix2,
                suffix: null,
                indent: true,
                emptyLineBefore: false,
                emptyLineAfter: false,
                value: value);
        }

        private MarkdownBuilder AppendItem(object prefix1, string prefix2, params object[] values)
        {
            return AppendLineMarkdown(
                prefix1: prefix1,
                prefix2: prefix2,
                suffix: null,
                indent: true,
                emptyLineBefore: false,
                emptyLineAfter: false,
                values: values);
        }

        public MarkdownBuilder AppendImage(string text, string url, string title = null)
        {
            AppendRaw("!");
            AppendLinkCore(text, url, title);
            return this;
        }

        public MarkdownBuilder AppendLink(string text, string url, string title = null)
        {
            if (url == null)
            {
                if (!AllowLinkWithoutUrl)
                    throw new ArgumentNullException(nameof(url));

                return Append(text);
            }

            if (url.Length == 0)
            {
                if (!AllowLinkWithoutUrl)
                    throw new ArgumentException("Url cannot be empty.", nameof(url));

                return Append(text);
            }

            return AppendLinkCore(text, url, title);
        }

        private MarkdownBuilder AppendLinkCore(string text, string url, string title)
        {
            AppendSquareBrackets(text);
            AppendRaw("(");
            Append(url, shouldBeEscaped: ch => ch == '(' || ch == ')');
            AppendLinkTitle(title);
            AppendRaw(")");
            return this;
        }

        public MarkdownBuilder AppendImageReference(string text, string label)
        {
            AppendRaw("!");
            AppendLinkReference(text, label);
            return this;
        }

        public MarkdownBuilder AppendLinkReference(string text, string label = null)
        {
            AppendSquareBrackets(text);
            AppendSquareBrackets(label);
            return this;
        }

        public MarkdownBuilder AppendLabel(string label, string url, string title = null)
        {
            AppendSquareBrackets(label);
            AppendRaw(": ");
            AppendAngleBrackets(url);
            AppendLinkTitle(title);
            return this;
        }

        private void AppendLinkTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                AppendRaw(" ");
                AppendDoubleQuotes(title);
            }
        }

        private void AppendDoubleQuotes(string value)
        {
            AppendRaw("\"");
            Append(value, shouldBeEscaped: ch => ch == '"');
            AppendRaw("\"");
        }

        private void AppendSquareBrackets(string value)
        {
            AppendRaw("[");
            Append(value, shouldBeEscaped: ch => ch == '[' || ch == ']');
            AppendRaw("]");
        }

        private void AppendAngleBrackets(string value)
        {
            AppendRaw("<");
            Append(value, shouldBeEscaped: ch => ch == '<' || ch == '>');
            AppendRaw(">");
        }

        public MarkdownBuilder AppendCodeBlock(string code, bool indent)
        {
            if (!indent)
                return AppendCodeBlock(code, language: null);

            AppendLineStart(addEmptyLine: AddEmptyLineBeforeCodeBlock, indent: true, prefix2: "    ");
            AppendBlock(code, prefix: "    ", escape: false);
            AppendEmptyLineIf(AddEmptyLineAfterCodeBlock);
            return this;
        }

        public MarkdownBuilder AppendCodeBlock(string code, string language = null)
        {
            AppendLineStart(addEmptyLine: AddEmptyLineBeforeCodeBlock, indent: true, prefix2: CodeBlockChars);
            AppendLineRaw(language);
            AppendBlock(code, escape: false);
            AppendLineStart(addEmptyLine: false, indent: true, prefix2: CodeBlockChars);
            AppendLineEnd(suffix: null, addEmptyLine: AddEmptyLineAfterCodeBlock);
            return this;
        }

        //TODO: overload
        public MarkdownBuilder AppendQuoteBlock(string value = null)
        {
            AppendBlock(value, prefix: QuoteBlockStart);
            return this;
        }

        private void AppendBlock(string value, string prefix = null, bool shouldEndWithNewLine = true, bool escape = true)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (IndentLevel == 0
                && QuoteLevel == 0
                && prefix == null)
            {
                Append(value, escape);
            }
            else
            {
                int length = value.Length;

                for (int i = 0; i < length; i++)
                {
                    char ch = value[i];

                    if (ch == 10)
                    {
                        AppendRaw(ch);

                        if (i + 1 < length)
                            AppendLineStart(addEmptyLine: false, indent: true, prefix2: prefix);
                    }
                    else if (ch == 13)
                    {
                        AppendRaw(ch);

                        if (i + 1 < length)
                        {
                            ch = value[i + 1];

                            if (ch == 10)
                            {
                                AppendRaw(ch);
                                i++;
                            }
                        }

                        if (i + 1 < length)
                            AppendLineStart(addEmptyLine: false, indent: true, prefix2: prefix);
                    }
                }
            }

            if (shouldEndWithNewLine)
            {
                char last = value[value.Length - 1];

                if (last != 10
                    && last != 13)
                {
                    AppendLine();
                }
            }
        }

        public MarkdownBuilder AppendCodeBlockChars()
        {
            return AppendRaw(CodeBlockChars);
        }

        public MarkdownBuilder AppendHorizonalRule()
        {
            AppendLineStart();
            AppendLineRaw(Settings.HorizontalRule);
            return this;
        }

        public MarkdownBuilder AppendTable(IEnumerable<TableColumn> columns, IEnumerable<IList<object>> rows)
        {
            return AppendTable(new TableColumnCollection(columns), rows.ToList());
        }

        public MarkdownBuilder AppendTable(IList<TableColumn> columns, IList<IList<object>> rows)
        {
            int columnCount = columns.Count;

            if (columnCount == 0)
                return this;

            if (FormatTableContent)
            {
                List<int> widths = CalculateWidths(columns, rows, columnCount);

                AppendTableHeader(columns, columnCount, widths);
                AppendTableRows(rows, columnCount, widths);
            }
            else
            {
                AppendTableHeader(columns, columnCount);
                AppendTableRows(rows, columnCount);
            }

            return this;
        }

        private List<int> CalculateWidths<T>(IList<TableColumn> columns, IList<T> items, IList<Func<T, object>> valueProviders)
        {
            List<int> widths = GetColumnsWidths(columns);

            int index = 0;

            var mb = new MarkdownBuilder(Settings);

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    mb.Append(valueProviders[j](items[j]));
                    widths[i] = Math.Max(widths[i], mb.Length - index);
                    index = mb.Length;
                }
            }

            return widths;
        }

        public MarkdownBuilder AppendTable<T>(
            IEnumerable<TableColumn> columns,
            IEnumerable<T> rows,
            IEnumerable<Func<T, object>> valueProviders)
        {
            return AppendTable(new TableColumnCollection(columns), rows.ToList(), valueProviders.ToList());
        }

        public MarkdownBuilder AppendTable<T>(IList<TableColumn> columns, IList<T> items, IList<Func<T, object>> valueProviders)
        {
            int columnCount = columns.Count;

            if (columnCount == 0)
                return this;

            if (FormatTableContent)
            {
                List<int> widths = CalculateWidths(columns, items, valueProviders);

                AppendTableHeader(columns, columnCount, widths);
                AppendTableRows(items, valueProviders, columnCount, widths);
            }
            else
            {
                AppendTableHeader(columns, columnCount);
                AppendTableRows(items, valueProviders, columnCount);
            }

            return this;
        }

        private List<int> CalculateWidths(IList<TableColumn> columns, IList<IList<object>> rows, int columnCount)
        {
            var widths = new List<int>();

            int index = 0;

            var mb = new MarkdownBuilder(Settings);

            foreach (IList<object> row in rows)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    mb.Append(row[i]);
                    widths.Add(mb.Length - index);
                    index = mb.Length;
                }
            }

            int count = widths.Count;

            List<int> maxWidths = GetColumnsWidths(columns);

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = i; j < count; j += columnCount)
                {
                    maxWidths[i] = Math.Max(maxWidths[i], widths[j]);
                }
            }

            return maxWidths;
        }

        private static List<int> GetColumnsWidths(IList<TableColumn> columns)
        {
            var widths = new List<int>(columns.Count);

            foreach (TableColumn column in columns)
                widths.Add(column.Name.Length);

            return widths;
        }

        public MarkdownBuilder AppendTableHeader(params TableColumn[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            int length = columns.Length;

            if (length == 0)
                return this;

            AppendTableHeader(columns, length);
            return this;
        }

        public MarkdownBuilder AppendTableHeader(IList<TableColumn> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            int count = columns.Count;

            if (count == 0)
                return this;

            AppendTableHeader(columns, count);
            return this;
        }

        internal void AppendTableHeader(IList<TableColumn> columns, int columnCount, IList<int> widths = null)
        {
            for (int i = 0; i < columnCount; i++)
            {
                AppendTableRowStart(i);

                AppendTablePadding();

                string name = columns[i].Name;

                Append(name);

                if (FormatTableHeader)
                {
                    int width = name.Length;
                    int minimalWidth = width;

                    if (FormatTableHeader)
                        minimalWidth = Math.Max(minimalWidth, 3);

                    AppendPadRight(width, widths?[i], minimalWidth);
                }

                AppendTableCellEnd(i, columnCount);
            }

            AppendLine();

            for (int i = 0; i < columnCount; i++)
            {
                TableColumn column = columns[i];

                AppendTableRowStart(i);

                if (column.Alignment == Alignment.Center)
                {
                    AppendRaw(":");
                }
                else
                {
                    AppendTablePadding();
                }

                AppendRaw("---");

                if (FormatTableHeader)
                    AppendPadRight(3, widths?[i] ?? columns[i].Name.Length, 3, '-');

                if (column.Alignment != Alignment.Left)
                {
                    AppendRaw(":");
                }
                else
                {
                    AppendTablePadding();
                }

                if (i == columnCount - 1)
                    AppendTableOuterPipe();
            }

            AppendLine();
        }

        internal void AppendTableRows(IList<IList<object>> rows, int columnCount, List<int> widths = null)
        {
            foreach (IList<object> row in rows)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    AppendTableRowStart(i);
                    AppendTablePadding();

                    int length = AppendInternal(row[i]);

                    if (FormatTableContent)
                        AppendPadRight(length, widths?[i]);

                    AppendTableCellEnd(i, columnCount);
                }

                AppendLine();
            }
        }

        internal void AppendTableRows<T>(IList<T> items, IList<Func<T, object>> valueProviders, int columnCount, List<int> widths = null)
        {
            foreach (T item in items)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    AppendTableRowStart(i);
                    AppendTablePadding();

                    int length = AppendInternal(valueProviders[i](item));

                    if (FormatTableContent)
                        AppendPadRight(length, widths?[i]);

                    AppendTableCellEnd(i, columnCount);
                }

                AppendLine();
            }
        }

        private void AppendTableRowStart(int index)
        {
            if (index == 0)
            {
                AppendLineStart();
                AppendTableOuterPipe();
            }
            else
            {
                AppendTableDelimiter();
            }
        }

        private void AppendTableCellEnd(int index, int length)
        {
            if (index == length - 1)
            {
                if (TableOuterPipe)
                {
                    AppendTablePadding();
                    AppendTableDelimiter();
                }
            }
            else
            {
                AppendTablePadding();
            }
        }

        private void AppendTableOuterPipe()
        {
            if (TableOuterPipe)
                AppendTableDelimiter();
        }

        private void AppendTablePadding()
        {
            if (TablePadding)
                AppendRaw(" ");
        }

        public void AppendTableDelimiter()
        {
            AppendRaw(TableDelimiter);
        }

        private void AppendPadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
            {
                AppendRaw(paddingChar);
            }
        }

        public MarkdownBuilder AppendComment(string value)
        {
            AppendRaw("<!-- ");
            AppendRaw(value);
            AppendRaw(" -->");
            return this;
        }

        public void AppendDelimiter(EmphasisOptions options)
        {
            if (options == EmphasisOptions.None)
                return;

            if ((options & EmphasisOptions.Bold) != 0)
            {
                Append(BoldDelimiter);

                if ((options & EmphasisOptions.Italic) != 0)
                    Append(AlternativeItalicDelimiter);
            }
            else if ((options & EmphasisOptions.Italic) != 0)
            {
                Append(ItalicDelimiter);
            }

            if ((options & EmphasisOptions.Strikethrough) != 0)
                Append(StrikethroughDelimiter);

            if ((options & EmphasisOptions.Code) != 0)
                Append(CodeDelimiter);
        }

        private MarkdownBuilder AppendDelimiter(string delimiter, string value)
        {
            AppendRaw(delimiter);
            Append(value);
            AppendRaw(delimiter);
            return this;
        }

        private MarkdownBuilder AppendDelimiter(string delimiter, object value)
        {
            AppendRaw(delimiter);
            Append(value);
            AppendRaw(delimiter);
            return this;
        }

        private MarkdownBuilder AppendDelimiter(string delimiter, params object[] values)
        {
            AppendRaw(delimiter);
            AppendRange(values);
            AppendRaw(delimiter);
            return this;
        }

        private MarkdownBuilder AppendLineMarkdown(object prefix1, string prefix2, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, string value)
        {
            AppendLineStart(emptyLineBefore, indent, prefix1, prefix2);

            if (AppendInternal(value) > 0)
                AppendLineEnd(suffix, emptyLineAfter);

            return this;
        }

        private MarkdownBuilder AppendLineMarkdown(object prefix1, string prefix2, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, object value)
        {
            AppendLineStart(emptyLineBefore, indent, prefix1, prefix2);

            if (AppendInternal(value) > 0)
                AppendLineEnd(suffix, emptyLineAfter);

            return this;
        }

        private MarkdownBuilder AppendLineMarkdown(object prefix1, string prefix2, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, params object[] values)
        {
            AppendLineStart(emptyLineBefore, indent, prefix1, prefix2);

            if (AppendInternal(values) > 0)
                AppendLineEnd(suffix, emptyLineAfter);

            return this;
        }

        internal int AppendInternal(string value)
        {
            int length = Length;
            Append(value);
            return Length - length;
        }

        internal int AppendInternal(object value)
        {
            int length = Length;
            Append(value);
            return Length - length;
        }

        internal int AppendInternal(params object[] values)
        {
            int length = Length;
            AppendRange(values);
            return Length - length;
        }

        public MarkdownBuilder Append<TMarkdown>(TMarkdown markdown) where TMarkdown : IMarkdown
        {
            return markdown.AppendTo(this);
        }

        public MarkdownBuilder Append(char value)
        {
            return Append(value, escape: true);
        }

        public MarkdownBuilder Append(char value, bool escape)
        {
            if (escape
                && Settings.ShouldBeEscaped(value))
            {
                return AppendRaw('\\');
            }

            return AppendRaw(value);
        }

        public MarkdownBuilder Append(string value)
        {
            return Append(value, escape: true);
        }

        public MarkdownBuilder Append(string value, bool escape)
        {
            if (escape)
            {
                return Append(value, Settings.ShouldBeEscaped);
            }
            else
            {
                return AppendRaw(value);
            }
        }

        public MarkdownBuilder Append(string value, EmphasisOptions options, bool escape = true)
        {
            AppendDelimiter(options);
            Append(value, escape);
            AppendDelimiter(options);
            return this;
        }

        internal virtual MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            MarkdownEscaper.Escape(value, shouldBeEscaped, StringBuilder, escapingChar);
            return this;
        }

        public MarkdownBuilder Append(object value)
        {
            return Append(value, escape: true);
        }

        public MarkdownBuilder Append(object value, bool escape)
        {
            if (value == null)
                return this;

            if (value is IMarkdown markdown)
            {
                return markdown.AppendTo(this);
            }
            else
            {
                return Append(value.ToString(), escape: escape);
            }
        }

        internal MarkdownBuilder AppendRange(params object[] values)
        {
            foreach (object value in values)
                Append(value);

            return this;
        }

        private void AppendLineStart(bool addEmptyLine = false, bool indent = false, object prefix1 = null, string prefix2 = null)
        {
            AppendEmptyLineIf(addEmptyLine);

            if (indent)
                AppendIndentation();

            AppendQuoteIndentation();

            Append(prefix1, escape: false);
            AppendRaw(prefix2);
        }

        private void AppendQuoteIndentation()
        {
            for (int i = 1; i <= QuoteLevel; i++)
                AppendRaw(QuoteBlockStart);
        }

        private void AppendLineEnd(string suffix = null, bool addEmptyLine = false)
        {
            AppendRaw(suffix);
            AppendLine(addEmptyLine);
        }

        public MarkdownBuilder AppendLine(string value, bool escape = true)
        {
            return Append(value, escape: escape).AppendLine();
        }

        public MarkdownBuilder AppendLineRaw(string value)
        {
            return AppendLine(value, escape: false);
        }

        private void AppendLine(bool addEmptyLine)
        {
            AppendLineIfNecessary();

            if (addEmptyLine)
                AppendEmptyLine();
        }

        private void AppendLineIfNecessary()
        {
            int length = Length;

            if (length == 0)
                return;

            if (this[length - 1] != '\n')
                AppendLine();
        }

        private void AppendEmptyLineIf(bool condition)
        {
            if (condition)
                AppendEmptyLine();
        }

        private void AppendEmptyLine()
        {
            int length = Length;

            if (length == 0)
                return;

            int index = length - 1;

            char ch = this[index];

            if (ch == '\n'
                && --index >= 0)
            {
                ch = this[index];

                if (ch == '\r')
                {
                    if (--index >= 0)
                    {
                        ch = this[index];

                        if (ch == '\n')
                            return;
                    }
                }
                else if (ch == '\n')
                {
                    return;
                }
            }

            AppendLine();
        }

        public MarkdownBuilder AppendRaw(char value)
        {
            StringBuilder.Append(value);
            return this;
        }

        public MarkdownBuilder AppendRaw(char value, int repeatCount)
        {
            StringBuilder.Append(value, repeatCount);
            return this;
        }

        public MarkdownBuilder AppendRaw(string value)
        {
            StringBuilder.Append(value);
            return this;
        }

        public MarkdownBuilder AppendRaw(int value)
        {
            StringBuilder.Append(value);
            return this;
        }

        public MarkdownBuilder AppendLine()
        {
            StringBuilder.AppendLine();
            return this;
        }

        public void Clear()
        {
            StringBuilder.Clear();
        }

        public override string ToString()
        {
            return StringBuilder.ToString();
        }
    }
}