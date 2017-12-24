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
        public MarkdownBuilder(MarkdownSettings settings = null)
            : this(new StringBuilder(), settings)
        {
        }

        public MarkdownBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            Settings = settings ?? MarkdownSettings.Default;
            StringBuilder = sb;
        }

        public MarkdownSettings Settings { get; }

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
            AppendDelimiter(BoldDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendBold(object value)
        {
            AppendDelimiter(BoldDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendBold(params object[] values)
        {
            AppendDelimiter(BoldDelimiter, values);
            return this;
        }

        public MarkdownBuilder AppendBoldDelimiter()
        {
            AppendRaw(BoldDelimiter);
            return this;
        }

        public MarkdownBuilder AppendItalic(string value)
        {
            AppendDelimiter(ItalicDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendItalic(object value)
        {
            AppendDelimiter(ItalicDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendItalic(params object[] values)
        {
            AppendDelimiter(ItalicDelimiter, values);
            return this;
        }

        public MarkdownBuilder AppendItalicDelimiter()
        {
            AppendRaw(ItalicDelimiter);
            return this;
        }

        public MarkdownBuilder AppendStrikethrough(string value)
        {
            AppendDelimiter(StrikethroughDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendStrikethrough(object value)
        {
            AppendDelimiter(StrikethroughDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendStrikethrough(params object[] values)
        {
            AppendDelimiter(StrikethroughDelimiter, values);
            return this;
        }

        public MarkdownBuilder AppendStrikethroughDelimiter()
        {
            AppendRaw(StrikethroughDelimiter);
            return this;
        }

        //TODO: code contains `
        public MarkdownBuilder AppendCode(string value)
        {
            AppendDelimiter(CodeDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendCode(object value)
        {
            AppendDelimiter(CodeDelimiter, value);
            return this;
        }

        public MarkdownBuilder AppendCode(params object[] values)
        {
            AppendDelimiter(CodeDelimiter, values);
            return this;
        }

        public MarkdownBuilder AppendCodeDelimiter()
        {
            AppendRaw(CodeDelimiter);
            return this;
        }

        public MarkdownBuilder AppendHeading1(object value = null)
        {
            AppendHeading(1, value);
            return this;
        }

        public MarkdownBuilder AppendHeading2(object value = null)
        {
            AppendHeading(2, value);
            return this;
        }

        public MarkdownBuilder AppendHeading3(object value = null)
        {
            AppendHeading(3, value);
            return this;
        }

        public MarkdownBuilder AppendHeading4(object value = null)
        {
            AppendHeading(4, value);
            return this;
        }

        public MarkdownBuilder AppendHeading5(object value = null)
        {
            AppendHeading(5, value);
            return this;
        }

        public MarkdownBuilder AppendHeading6(object value = null)
        {
            AppendHeading(6, value);
            return this;
        }

        public MarkdownBuilder AppendHeading(int level, string value = null)
        {
            return AppendLineMarkdown(
                prefix: HeadingStart(level),
                suffix: (CloseHeading) ? HeadingEnd(level) : null,
                indent: false,
                emptyLineBefore: AddEmptyLineBeforeHeading,
                emptyLineAfter: AddEmptyLineAfterHeading,
                value: value);
        }

        public MarkdownBuilder AppendHeading(int level, object value = null)
        {
            return AppendLineMarkdown(
                prefix: HeadingStart(level),
                suffix: (CloseHeading) ? HeadingEnd(level) : null,
                indent: false,
                emptyLineBefore: AddEmptyLineBeforeHeading,
                emptyLineAfter: AddEmptyLineAfterHeading,
                value: value);
        }

        public MarkdownBuilder AppendHeading(int level, params object[] values)
        {
            return AppendLineMarkdown(
                prefix: HeadingStart(level),
                suffix: (CloseHeading) ? HeadingEnd(level) : null,
                indent: false,
                emptyLineBefore: AddEmptyLineBeforeHeading,
                emptyLineAfter: AddEmptyLineAfterHeading,
                values: values);
        }

        public MarkdownBuilder AppendHeadingStart(int level)
        {
            AppendRaw(HeadingStart(level));
            return this;
        }

        public MarkdownBuilder AppendListItem(string value)
        {
            return AppendItem(ListItemStart, value);
        }

        public MarkdownBuilder AppendListItem(object value)
        {
            return AppendItem(ListItemStart, value);
        }

        public MarkdownBuilder AppendListItem(params object[] values)
        {
            return AppendItem(ListItemStart, values);
        }

        public MarkdownBuilder AppendListItemStart()
        {
            AppendRaw(ListItemStart);
            return this;
        }

        public MarkdownBuilder AppendOrderedListItem(int number, string value)
        {
            return AppendItem(OrderedListItemStart(number), value);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, object value)
        {
            return AppendItem(OrderedListItemStart(number), value);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, params object[] values)
        {
            return AppendItem(OrderedListItemStart(number), values);
        }

        public MarkdownBuilder AppendOrderedListItemStart(int number)
        {
            StringBuilder.Append(number);
            AppendRaw(". ");
            return this;
        }

        public MarkdownBuilder AppendTaskListItem(string value)
        {
            return AppendItem(TaskListItemStart(), value);
        }

        public MarkdownBuilder AppendTaskListItem(object value)
        {
            return AppendItem(TaskListItemStart(), value);
        }

        public MarkdownBuilder AppendTaskListItem(params object[] values)
        {
            return AppendItem(TaskListItemStart(), values);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(string value)
        {
            return AppendItem(TaskListItemStart(isCompleted: true), value);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(object value)
        {
            return AppendItem(TaskListItemStart(isCompleted: true), value);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(params object[] values)
        {
            return AppendItem(TaskListItemStart(isCompleted: true), values);
        }

        public MarkdownBuilder AppendTaskListItemStart(bool isCompleted = false)
        {
            AppendRaw(TaskListItemStart(isCompleted));
            return this;
        }

        public MarkdownBuilder AppendItem(string prefix, string value)
        {
            AppendLineMarkdown(prefix, suffix: null, indent: true, emptyLineBefore: false, emptyLineAfter: false, value: value);
            return this;
        }

        public MarkdownBuilder AppendItem(string prefix, object value)
        {
            AppendLineMarkdown(prefix, suffix: null, indent: true, emptyLineBefore: false, emptyLineAfter: false, value: value);
            return this;
        }

        public MarkdownBuilder AppendItem(string prefix, params object[] values)
        {
            AppendLineMarkdown(prefix, suffix: null, indent: true, emptyLineBefore: false, emptyLineAfter: false, values: values);
            return this;
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
            Append(url, shouldBeEscaped: f => f == '(' || f == ')');
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
            Append(value, shouldBeEscaped: f => f == '"');
            AppendRaw("\"");
        }

        private void AppendSquareBrackets(string value)
        {
            AppendRaw("[");
            Append(value, shouldBeEscaped: f => f == '[' || f == ']');
            AppendRaw("]");
        }

        private void AppendAngleBrackets(string value)
        {
            AppendRaw("<");
            Append(value, shouldBeEscaped: f => f == '<' || f == '>');
            AppendRaw(">");
        }

        public MarkdownBuilder AppendCodeBlock(string code, bool indent)
        {
            if (!indent)
                return AppendCodeBlock(code, language: null);

            AppendLineStart(addEmptyLine: AddEmptyLineBeforeCodeBlock, indent: true, prefix: "    ");
            AppendBlock(code, prefix: "    ", escape: false);
            AppendEmptyLineIf(AddEmptyLineAfterCodeBlock);
            return this;
        }

        public MarkdownBuilder AppendCodeBlock(string code, string language = null)
        {
            AppendLineStart(addEmptyLine: AddEmptyLineBeforeCodeBlock, indent: true, prefix: CodeBlockChars);
            AppendLineRaw(language);
            AppendBlock(code, escape: false);
            AppendLineStart(addEmptyLine: false, indent: true, prefix: CodeBlockChars);
            AppendLineEnd(suffix: null, addEmptyLine: AddEmptyLineAfterCodeBlock);
            return this;
        }

        public MarkdownBuilder AppendQuoteBlock(string value = null)
        {
            AppendBlock(value, prefix: "> ");
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
                            AppendLineStart(addEmptyLine: false, indent: true, prefix: prefix);
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
                            AppendLineStart(addEmptyLine: false, indent: true, prefix: prefix);
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
            AppendRaw(CodeBlockChars);
            return this;
        }

        public MarkdownBuilder AppendHorizonalRule()
        {
            AppendLineStart();
            AppendLineRaw(Settings.HorizontalRule);
            return this;
        }

        public MarkdownBuilder AppendTable(IEnumerable<TableHeader> headers, IEnumerable<IList<object>> rows)
        {
            return AppendTable(new TableHeaderCollection(headers), rows.ToList());
        }

        public MarkdownBuilder AppendTable(IList<TableHeader> headers, IList<IList<object>> rows)
        {
            int columnCount = headers.Count;

            if (columnCount == 0)
                return this;

            if (FormatTableContent)
            {
                List<int> widths = CalculateWidths(headers, rows, columnCount);

                AppendTableHeader(headers, columnCount, widths);
                AppendTableRows(rows, columnCount, widths);
            }
            else
            {
                AppendTableHeader(headers, columnCount);
                AppendTableRows(rows, columnCount);
            }

            return this;
        }

        private List<int> CalculateWidths<T>(IList<TableHeader> headers, IList<T> items, IList<Func<T, object>> valueProviders)
        {
            List<int> widths = GetHeadersWidths(headers);

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
            IEnumerable<TableHeader> headers,
            IEnumerable<T> rows,
            IEnumerable<Func<T, object>> valueProviders)
        {
            return AppendTable(new TableHeaderCollection(headers), rows.ToList(), valueProviders.ToList());
        }

        public MarkdownBuilder AppendTable<T>(IList<TableHeader> headers, IList<T> items, IList<Func<T, object>> valueProviders)
        {
            int columnCount = headers.Count;

            if (columnCount == 0)
                return this;

            if (FormatTableContent)
            {
                List<int> widths = CalculateWidths(headers, items, valueProviders);

                AppendTableHeader(headers, columnCount, widths);
                AppendTableRows(items, valueProviders, columnCount, widths);
            }
            else
            {
                AppendTableHeader(headers, columnCount);
                AppendTableRows(items, valueProviders, columnCount);
            }

            return this;
        }

        private List<int> CalculateWidths(IList<TableHeader> headers, IList<IList<object>> rows, int columnCount)
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

            List<int> maxWidths = GetHeadersWidths(headers);

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = i; j < count; j += columnCount)
                {
                    maxWidths[i] = Math.Max(maxWidths[i], widths[j]);
                }
            }

            return maxWidths;
        }

        private static List<int> GetHeadersWidths(IList<TableHeader> headers)
        {
            var widths = new List<int>(headers.Count);

            foreach (TableHeader header in headers)
                widths.Add(header.Name.Length);

            return widths;
        }

        public MarkdownBuilder AppendTableHeader(params TableHeader[] headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int length = headers.Length;

            if (length == 0)
                return this;

            AppendTableHeader(headers, length);
            return this;
        }

        public MarkdownBuilder AppendTableHeader(IList<TableHeader> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int count = headers.Count;

            if (count == 0)
                return this;

            AppendTableHeader(headers, count);
            return this;
        }

        internal void AppendTableHeader(IList<TableHeader> headers, int columnCount, IList<int> widths = null)
        {
            for (int i = 0; i < columnCount; i++)
            {
                AppendTableRowStart(i);

                AppendTablePadding();

                string name = headers[i].Name;

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
                TableHeader header = headers[i];

                AppendTableRowStart(i);

                if (header.Alignment == Alignment.Center)
                {
                    AppendRaw(":");
                }
                else
                {
                    AppendTablePadding();
                }

                AppendRaw("---");

                if (FormatTableHeader)
                    AppendPadRight(3, widths?[i] ?? headers[i].Name.Length, 3, '-');

                if (header.Alignment != Alignment.Left)
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

        private void AppendDelimiter(string delimiter, string value)
        {
            AppendRaw(delimiter);
            Append(value);
            AppendRaw(delimiter);
        }

        private void AppendDelimiter(string delimiter, object value)
        {
            AppendRaw(delimiter);
            Append(value);
            AppendRaw(delimiter);
        }

        private void AppendDelimiter(string delimiter, params object[] values)
        {
            AppendRaw(delimiter);
            AppendRange(values);
            AppendRaw(delimiter);
        }

        private MarkdownBuilder AppendLineMarkdown(string prefix, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, string value)
        {
            AppendLineStart(emptyLineBefore, indent, prefix);

            if (AppendInternal(value) > 0)
                AppendLineEnd(suffix, emptyLineAfter);

            return this;
        }

        private MarkdownBuilder AppendLineMarkdown(string prefix, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, object value)
        {
            AppendLineStart(emptyLineBefore, indent, prefix);

            if (AppendInternal(value) > 0)
                AppendLineEnd(suffix, emptyLineAfter);

            return this;
        }

        private MarkdownBuilder AppendLineMarkdown(string prefix, string suffix, bool indent, bool emptyLineBefore, bool emptyLineAfter, params object[] values)
        {
            AppendLineStart(emptyLineBefore, indent, prefix);

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

        public MarkdownBuilder Append(string value, EmphasisOptions options)
        {
            return Append(value, options, escape: true);
        }

        public MarkdownBuilder Append(string value, EmphasisOptions options, bool escape)
        {
            AppendDelimiter(options);
            Append(value, escape);
            AppendDelimiter(options);
            return this;
        }

        internal MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped)
        {
            MarkdownEscaper.Escape(value, shouldBeEscaped, StringBuilder);

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

        private void AppendLineStart(bool addEmptyLine = false, bool indent = false, string prefix = null)
        {
            AppendEmptyLineIf(addEmptyLine);

            if (indent)
                AppendIndentation();

            for (int i = 1; i <= QuoteLevel; i++)
                AppendRaw("> ");

            AppendRaw(prefix);
        }

        private void AppendLineEnd(string suffix, bool addEmptyLine)
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
            int length = Length;

            if (length == 0)
                return;

            if (this[length - 1] != '\n')
                AppendLine();

            if (addEmptyLine)
                AppendEmptyLine();
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

        public MarkdownBuilder AppendRaw(string value)
        {
            StringBuilder.Append(value);
            return this;
        }

        public MarkdownBuilder AppendLine()
        {
            StringBuilder.AppendLine();
            return this;
        }

        public override string ToString()
        {
            return StringBuilder.ToString();
        }
    }
}