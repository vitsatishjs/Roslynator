// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using static Roslynator.Utilities.Markdown.MarkdownFactory;

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownWriter : StringWriter
    {
        private readonly StringBuilder _sb;

        public MarkdownWriter(MarkdownSettings settings = null)
            : this(new StringBuilder(), settings)
        {
        }

        public MarkdownWriter(IFormatProvider formatProvider, MarkdownSettings settings = null)
            : this(new StringBuilder(), formatProvider, settings)
        {
        }

        public MarkdownWriter(StringBuilder sb, MarkdownSettings settings = null)
            : this(sb, CultureInfo.CurrentCulture, settings)
        {
        }

        public MarkdownWriter(StringBuilder sb, IFormatProvider formatProvider, MarkdownSettings settings = null)
            : base(sb, formatProvider)
        {
            Settings = settings ?? MarkdownSettings.Default;
            _sb = GetStringBuilder();
        }

        public MarkdownSettings Settings { get; }

        public int IndentLevel { get; private set; }

        internal int Length => _sb.Length;

        private string BoldDelimiter => Settings.BoldDelimiter;

        private string ItalicDelimiter => Settings.ItalicDelimiter;

        private string AlternativeItalicDelimiter => Settings.AlternativeItalicDelimiter;

        private string StrikethroughDelimiter => Settings.StrikethroughDelimiter;

        private string CodeDelimiter => Settings.CodeDelimiter;

        private string TableDelimiter => Settings.TableDelimiter;

        private string ListItemStart => Settings.ListItemStart;

        private string CodeBlockChars => Settings.CodeBlockChars;

        private string IndentChars => Settings.IndentChars;

        private bool AddEmptyLineBeforeHeader => Settings.EmptyLineBeforeHeader;

        private bool AddEmptyLineAfterHeader => Settings.EmptyLineAfterHeader;

        private bool AddEmptyLineBeforeCodeBlock => Settings.EmptyLineBeforeCodeBlock;

        private bool AddEmptyLineAfterCodeBlock => Settings.EmptyLineAfterCodeBlock;

        private bool FormatTableHeader => Settings.FormatTableHeader;

        private bool FormatTableContent => Settings.FormatTableContent;

        private bool UseTableOuterPipe => Settings.UseTableOuterPipe;

        private bool UseTablePadding => Settings.UseTablePadding;

        public MarkdownWriter IncreaseIndent()
        {
            IndentLevel++;
            return this;
        }

        public MarkdownWriter DecreaseIndent()
        {
            if (IndentLevel == 0)
                throw new InvalidOperationException($"{nameof(IndentLevel)} cannot be less than 0.");

            IndentLevel++;
            return this;
        }

        internal MarkdownWriter WriteIndentationIf(bool condition)
        {
            if (condition)
                WriteIndentation();

            return this;
        }

        public MarkdownWriter WriteIndentation()
        {
            for (int i = 0; i < IndentLevel; i++)
                Write(IndentChars);

            return this;
        }

        public MarkdownWriter Write(BoldText text)
        {
            text.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(ItalicText text)
        {
            text.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(BoldItalicText text)
        {
            text.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(StrikethroughText text)
        {
            text.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(CodeText code)
        {
            code.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(Header header)
        {
            header.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(ListItem item)
        {
            item.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(OrderedListItem item)
        {
            item.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(TaskListItem item)
        {
            item.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(Image image)
        {
            image.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(Link link)
        {
            link.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(CodeBlock codeBlock)
        {
            codeBlock.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(QuoteBlock blockQuote)
        {
            blockQuote.WriteTo(this);
            return this;
        }

        public MarkdownWriter Write(Table table)
        {
            WriteTable(table.Headers, table.Rows);
            return this;
        }

        public MarkdownWriter WriteBold(string value)
        {
            WriteDelimiter(BoldDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteBold(object value)
        {
            WriteDelimiter(BoldDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteBold(params object[] values)
        {
            WriteDelimiter(BoldDelimiter, values);
            return this;
        }

        public MarkdownWriter WriteBoldDelimiter()
        {
            Write(BoldDelimiter);
            return this;
        }

        public MarkdownWriter WriteItalic(string value)
        {
            WriteDelimiter(ItalicDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteItalic(object value)
        {
            WriteDelimiter(ItalicDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteItalic(params object[] values)
        {
            WriteDelimiter(ItalicDelimiter, values);
            return this;
        }

        public MarkdownWriter WriteItalicDelimiter()
        {
            Write(ItalicDelimiter);
            return this;
        }

        public MarkdownWriter WriteBoldItalic(string value)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteBoldItalic(object value)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteBoldItalic(params object[] values)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, values);
            return this;
        }

        public MarkdownWriter WriteStrikethrough(string value)
        {
            WriteDelimiter(StrikethroughDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteStrikethrough(object value)
        {
            WriteDelimiter(StrikethroughDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteStrikethrough(params object[] values)
        {
            WriteDelimiter(StrikethroughDelimiter, values);
            return this;
        }

        public MarkdownWriter WriteStrikethroughDelimiter()
        {
            Write(StrikethroughDelimiter);
            return this;
        }

        public MarkdownWriter WriteCode(string value)
        {
            WriteDelimiter(CodeDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteCode(object value)
        {
            WriteDelimiter(CodeDelimiter, value);
            return this;
        }

        public MarkdownWriter WriteCode(params object[] values)
        {
            WriteDelimiter(CodeDelimiter, values);
            return this;
        }

        public MarkdownWriter WriteCodeDelimiter()
        {
            Write(CodeDelimiter);
            return this;
        }

        public MarkdownWriter WriteHeader1(object value = null)
        {
            WriteHeader(1, value);
            return this;
        }

        public MarkdownWriter WriteHeader2(object value = null)
        {
            WriteHeader(2, value);
            return this;
        }

        public MarkdownWriter WriteHeader3(object value = null)
        {
            WriteHeader(3, value);
            return this;
        }

        public MarkdownWriter WriteHeader4(object value = null)
        {
            WriteHeader(4, value);
            return this;
        }

        public MarkdownWriter WriteHeader5(object value = null)
        {
            WriteHeader(5, value);
            return this;
        }

        public MarkdownWriter WriteHeader6(object value = null)
        {
            WriteHeader(6, value);
            return this;
        }

        public MarkdownWriter WriteHeader(int level, string value = null)
        {
            WriteLineMarkdown(HeaderStart(level), indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader, value: value);
            return this;
        }

        public MarkdownWriter WriteHeader(int level, object value = null)
        {
            WriteLineMarkdown(HeaderStart(level), indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader, value: value);
            return this;
        }

        public MarkdownWriter WriteHeader(int level, params object[] values)
        {
            WriteLineMarkdown(HeaderStart(level), indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader, values: values);
            return this;
        }

        public MarkdownWriter WriteHeaderStart(int level)
        {
            Write(HeaderStart(level));
            return this;
        }

        public MarkdownWriter WriteListItem(string value)
        {
            WriteItem(ListItemStart, value);
            return this;
        }

        public MarkdownWriter WriteListItem(object value)
        {
            WriteItem(ListItemStart, value);
            return this;
        }

        public MarkdownWriter WriteListItem(params object[] values)
        {
            WriteItem(ListItemStart, values);
            return this;
        }

        public MarkdownWriter WriteListItemStart()
        {
            Write(ListItemStart);
            return this;
        }

        public MarkdownWriter WriteOrderedListItem(int number, string value)
        {
            WriteItem(OrderedListItemStart(number), value);
            return this;
        }

        public MarkdownWriter WriteOrderedListItem(int number, object value)
        {
            WriteItem(OrderedListItemStart(number), value);
            return this;
        }

        public MarkdownWriter WriteOrderedListItem(int number, params object[] values)
        {
            WriteItem(OrderedListItemStart(number), values);
            return this;
        }

        public MarkdownWriter WriteOrderedListItemStart(int number)
        {
            Write(number);
            Write(". ");
            return this;
        }

        public MarkdownWriter WriteTaskListItem(string value)
        {
            WriteItem(TaskListItemStart(), value);
            return this;
        }

        public MarkdownWriter WriteTaskListItem(object value)
        {
            WriteItem(TaskListItemStart(), value);
            return this;
        }

        public MarkdownWriter WriteTaskListItem(params object[] values)
        {
            WriteItem(TaskListItemStart(), values);
            return this;
        }

        public MarkdownWriter WriteCompletedTaskListItem(string value)
        {
            WriteItem(TaskListItemStart(isCompleted: true), value);
            return this;
        }

        public MarkdownWriter WriteCompletedTaskListItem(object value)
        {
            WriteItem(TaskListItemStart(isCompleted: true), value);
            return this;
        }

        public MarkdownWriter WriteCompletedTaskListItem(params object[] values)
        {
            WriteItem(TaskListItemStart(isCompleted: true), values);
            return this;
        }

        public MarkdownWriter WriteTaskListItemStart(bool isCompleted = false)
        {
            Write(TaskListItemStart(isCompleted));
            return this;
        }

        public MarkdownWriter WriteItem(string prefix, string value)
        {
            WriteLineMarkdown(prefix, indent: true, emptyLineBefore: false, emptyLineAfter: false, value: value);
            return this;
        }

        public MarkdownWriter WriteItem(string prefix, object value)
        {
            WriteLineMarkdown(prefix, indent: true, emptyLineBefore: false, emptyLineAfter: false, value: value);
            return this;
        }

        public MarkdownWriter WriteItem(string prefix, params object[] values)
        {
            WriteLineMarkdown(prefix, indent: true, emptyLineBefore: false, emptyLineAfter: false, values: values);
            return this;
        }

        public MarkdownWriter WriteImage(string text, string url)
        {
            WriteImageOrLink(text, url, isImage: true);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                WriteMarkdown(text);
            }
            else
            {
                WriteImageOrLink(text, url, isImage: false);
            }

            return this;
        }

        private void WriteImageOrLink(string text, string url, bool isImage)
        {
            if (isImage)
                Write("!");

            Write("[");
            WriteMarkdown(text, shouldBeEscaped: f => f == '[' || f == ']');
            Write("](");
            WriteMarkdown(url, shouldBeEscaped: f => f == '(' || f == ')');
            Write(")");
        }

        public MarkdownWriter WriteCodeBlock(string code, string language = null)
        {
            WriteEmptyLineIf(AddEmptyLineBeforeCodeBlock);
            WriteIndentation();
            WriteCodeBlockChars();
            WriteLine(language);
            WriteWithIndentation(code);
            WriteIndentation();
            WriteCodeBlockChars();
            WriteLine();
            WriteEmptyLineIf(AddEmptyLineAfterCodeBlock);
            return this;
        }

        public MarkdownWriter WriteQuoteBlock(string value = null)
        {
            WriteWithIndentation(value, prefix: "> ");
            return this;
        }

        private void WriteWithIndentation(string value, string prefix = null, bool shouldEndWithNewLine = true)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (IndentLevel == 0
                && prefix == null)
            {
                Write(value);
            }
            else
            {
                int length = value.Length;

                for (int i = 0; i < length; i++)
                {
                    char ch = value[i];

                    if (ch == 10)
                    {
                        Write(ch);

                        if (i + 1 < length)
                        {
                            WriteIndentation();
                        }

                        Write(prefix);
                    }
                    else if (ch == 13)
                    {
                        Write(ch);

                        if (i + 1 < length)
                        {
                            ch = value[i + 1];

                            if (ch == 10)
                            {
                                Write(ch);
                                i++;
                            }
                        }

                        if (i + 1 < length)
                            WriteIndentation();
                    }
                }
            }

            if (shouldEndWithNewLine)
            {
                char last = value[value.Length - 1];

                if (last != 10
                    && last != 13)
                {
                    WriteLine();
                }
            }
        }

        public MarkdownWriter WriteCodeBlockChars()
        {
            Write(CodeBlockChars);
            return this;
        }

        public MarkdownWriter WriteHorizonalRule()
        {
            WriteLine(Settings.HorizontalRule);
            return this;
        }

        public MarkdownWriter WriteTable(TableHeader header1, TableHeader header2, IEnumerable<TableRow> rows)
        {
            WriteTable(new TableHeaderCollection() { header1, header2 }, rows);
            return this;
        }

        public MarkdownWriter WriteTable(TableHeader header1, TableHeader header2, TableHeader header3, IEnumerable<TableRow> rows)
        {
            WriteTable(new TableHeaderCollection() { header1, header2, header3 }, rows);
            return this;
        }

        public MarkdownWriter WriteTable(TableHeader header1, TableHeader header2, TableHeader header3, TableHeader header4, IEnumerable<TableRow> rows)
        {
            WriteTable(new TableHeaderCollection() { header1, header2, header3, header4 }, rows);
            return this;
        }

        public MarkdownWriter WriteTable(TableHeader header1, TableHeader header2, TableHeader header3, TableHeader header4, TableHeader header5, IEnumerable<TableRow> rows)
        {
            WriteTable(new TableHeaderCollection() { header1, header2, header3, header4, header5 }, rows);
            return this;
        }

        public MarkdownWriter WriteTable(IEnumerable<TableHeader> headers, IEnumerable<TableRow> rows)
        {
            WriteTable(new TableHeaderCollection(headers), new TableRowCollection(rows));
            return this;
        }

        public MarkdownWriter WriteTable(TableHeaderCollection headers, IEnumerable<TableRow> rows)
        {
            WriteTable(headers, new TableRowCollection(rows));
            return this;
        }

        public MarkdownWriter WriteTable(TableHeaderCollection headers, TableRowCollection rows)
        {
            int columnCount = headers.Count;

            if (columnCount == 0)
                return this;

            if (!FormatTableContent)
            {
                WriteTableHeader(headers, columnCount);
                WriteTableRows(rows, columnCount);
            }
            else
            {
                bool formatHeader = FormatTableHeader;

                List<int> widths = CalculateWidths((formatHeader) ? headers : null, rows, columnCount);

                WriteTableHeader(headers, columnCount, (formatHeader) ? widths : null);
                WriteTableRows(rows, columnCount, widths);
            }

            return this;
        }

        private List<int> CalculateWidths(TableHeaderCollection headers, TableRowCollection rows, int columnCount)
        {
            var widths = new List<int>();

            var sb = new StringBuilder();

            int index = 0;

            using (var mw = new MarkdownWriter(sb, FormatProvider, Settings))
            {
                if (headers != null)
                {
                    foreach (TableHeader header in headers)
                    {
                        mw.WriteMarkdown(header.Name);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                foreach (TableRow row in rows)
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        mw.Write(row[i]);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                int count = widths.Count;

                var maxWidths = new List<int>();

                for (int i = 0; i < columnCount; i++)
                {
                    maxWidths.Add(0);

                    for (int j = i; j < count; j += columnCount)
                    {
                        maxWidths[i] = Math.Max(maxWidths[i], widths[j]);
                    }
                }

                return maxWidths;
            }
        }

        public MarkdownWriter WriteTableHeader(params TableHeader[] headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int length = headers.Length;

            if (length == 0)
                return this;

            WriteTableHeader(headers, length);
            return this;
        }

        public MarkdownWriter WriteTableHeader(IList<TableHeader> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int count = headers.Count;

            if (count == 0)
                return this;

            WriteTableHeader(headers, count);
            return this;
        }

        internal void WriteTableHeader(IList<TableHeader> headers, int columnCount, IList<int> widths = null)
        {
            for (int i = 0; i < columnCount; i++)
            {
                WriteTableRowStart(i);

                WriteTablePadding();

                string name = headers[i].Name;

                WriteMarkdown(name);

                if (FormatTableHeader)
                {
                    int width = name.Length;
                    int minimalWidth = width;

                    if (FormatTableHeader)
                        minimalWidth = Math.Max(minimalWidth, 3);

                    WritePadRight(width, widths?[i], minimalWidth);
                }

                WriteTableCellEnd(i, columnCount);
            }

            WriteLine();

            for (int i = 0; i < columnCount; i++)
            {
                TableHeader header = headers[i];

                WriteTableRowStart(i);

                if (header.Alignment == Alignment.Center)
                {
                    Write(":");
                }
                else
                {
                    WriteTablePadding();
                }

                Write("---");

                if (FormatTableHeader)
                    WritePadRight(3, widths?[i] ?? headers[i].Name.Length, 3, '-');

                if (header.Alignment != Alignment.Left)
                {
                    Write(":");
                }
                else
                {
                    WriteTablePadding();
                }

                if (i == columnCount - 1)
                    WriteTableOuterPipe();
            }

            WriteLine();
        }

        internal void WriteTableRows(IList<TableRow> rows, int columnCount, List<int> widths = null)
        {
            foreach (TableRow row in rows)
                WriteTableRow(row, columnCount, widths);
        }

        internal void WriteTableRow(TableRow row, int columnCount, IList<int> widths = null)
        {
            for (int i = 0; i < columnCount; i++)
            {
                WriteTableRowStart(i);
                WriteTablePadding();

                object value = row[i];
                int? proposedWidth = widths?[i];

                int length = Length;

                Write(value);

                length = Length - length;

                if (FormatTableContent)
                    WritePadRight(length, proposedWidth);

                WriteTableCellEnd(i, columnCount);
            }

            WriteLine();
        }

        private void WriteTableRowStart(int index)
        {
            if (index == 0)
            {
                WriteTableOuterPipe();
            }
            else
            {
                WriteTableDelimiter();
            }
        }

        private void WriteTableCellEnd(int index, int length)
        {
            if (index == length - 1)
            {
                if (UseTableOuterPipe)
                {
                    WriteTablePadding();
                    WriteTableDelimiter();
                }
            }
            else
            {
                WriteTablePadding();
            }
        }

        private void WriteTableOuterPipe()
        {
            if (UseTableOuterPipe)
                WriteTableDelimiter();
        }

        private void WriteTablePadding()
        {
            if (UseTablePadding)
                Write(" ");
        }

        public void WriteTableDelimiter()
        {
            Write(TableDelimiter);
        }

        private void WritePadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
            {
                Write(paddingChar);
            }
        }

        public MarkdownWriter WriteComment(string value)
        {
            Write("<!-- ");
            Write(value);
            Write(" -->");
            return this;
        }

        public MarkdownWriter WriteMarkdown(string value, bool escape = true)
        {
            if (string.IsNullOrEmpty(value))
                return this;

            if (escape)
            {
                WriteMarkdown(value, Settings.ShouldBeEscaped);
            }
            else
            {
                Write(value);
            }

            return this;
        }

        public override void Write(object value)
        {
            WriteMarkdown(value, escape: true);
        }

        public MarkdownWriter WriteMarkdown(object value, bool escape = true)
        {
            if (value == null)
                return this;

            if (value is IMarkdown markdown)
            {
                markdown.WriteTo(this);
            }
            else if (value is IFormattable formattable)
            {
                WriteMarkdown(formattable.ToString(format: null, formatProvider: FormatProvider), escape: escape);
            }
            else
            {
                WriteMarkdown(value.ToString(), escape: escape);
            }

            return this;
        }

        private void WriteDelimiter(string delimiter, string value)
        {
            Write(delimiter);
            WriteMarkdown(value);
            Write(delimiter);
        }

        private void WriteDelimiter(string delimiter, object value)
        {
            Write(delimiter);
            WriteMarkdown(value);
            Write(delimiter);
        }

        private void WriteDelimiter(string delimiter, params object[] values)
        {
            Write(delimiter);
            WriteMarkdown(values);
            Write(delimiter);
        }

        private void WriteDelimiters(string delimiter, string delimiter2, string value)
        {
            Write(delimiter);
            Write(delimiter2);
            WriteMarkdown(value);
            Write(delimiter2);
            Write(delimiter);
        }

        private void WriteDelimiters(string delimiter, string delimiter2, object value)
        {
            Write(delimiter);
            Write(delimiter2);
            WriteMarkdown(value);
            Write(delimiter2);
            Write(delimiter);
        }

        private void WriteDelimiters(string delimiter, string delimiter2, params object[] values)
        {
            Write(delimiter);
            Write(delimiter2);
            WriteMarkdown(values);
            Write(delimiter2);
            Write(delimiter);
        }

        private void WriteLineMarkdown(string prefix, bool indent, bool emptyLineBefore, bool emptyLineAfter, string value)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(value), emptyLineAfter);
        }

        private void WriteLineMarkdown(string prefix, bool indent, bool emptyLineBefore, bool emptyLineAfter, object value)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(value), emptyLineAfter);
        }

        private void WriteLineMarkdown(string prefix, bool indent, bool emptyLineBefore, bool emptyLineAfter, params object[] values)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(values), emptyLineAfter);
        }

        internal bool WriteMarkdownInternal(string value)
        {
            int length = Length;
            WriteMarkdown(value);
            return length != Length;
        }

        internal bool WriteMarkdownInternal(object value)
        {
            int length = Length;
            WriteMarkdown(value);
            return length != Length;
        }

        internal bool WriteMarkdownInternal(params object[] values)
        {
            int length = Length;
            WriteMarkdown(values);
            return length != Length;
        }

        internal MarkdownWriter WriteMarkdown(string value, Func<char, bool> shouldBeEscaped)
        {
            if (string.IsNullOrEmpty(value))
                return this;

            int length = value.Length;

            int firstIndex = -1;
            int i = 0;

            while (i < length)
            {
                if (shouldBeEscaped(value[i]))
                {
                    firstIndex = i;
                    break;
                }

                i++;
            }

            if (firstIndex == -1)
            {
                Write(value);
                return this;
            }

            i = 0;

            while (i < firstIndex)
            {
                Write(value[i]);
                i++;
            }

            Write('\\');
            Write(value[i]);
            i++;

            while (i < length)
            {
                if (shouldBeEscaped(value[i]))
                    Write('\\');

                Write(value[i]);
                i++;
            }

            return this;
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            int length = Length;

            if (length == 0)
                return;

            int index = length - 1;

            char ch = _sb[index];

            if (ch == '\n'
                && --index >= 0)
            {
                ch = _sb[index];

                if (ch == '\r')
                {
                    if (--index >= 0)
                    {
                        ch = _sb[index];

                        if (ch == '\n')
                            return;
                    }
                }
                else if (ch == '\n')
                {
                    return;
                }
            }

            WriteLine();
        }

        private void WriteLineIf(bool condition, bool addEmptyLine = false)
        {
            if (!condition)
                return;

            int length = Length;

            if (length == 0)
                return;

            if (_sb[length - 1] != '\n')
                WriteLine();

            if (addEmptyLine)
                WriteEmptyLine();
        }
    }
}