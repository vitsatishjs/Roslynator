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

        private bool AddEmptyLineBeforeHeader => Settings.AddEmptyLineBeforeHeader;

        private bool AddEmptyLineAfterHeader => Settings.AddEmptyLineAfterHeader;

        private bool AddEmptyLineBeforeCodeBlock => Settings.AddEmptyLineBeforeCodeBlock;

        private bool AddEmptyLineAfterCodeBlock => Settings.AddEmptyLineAfterCodeBlock;

        private bool FormatTableHeader => Settings.FormatTableHeader;

        private bool FormatTableContent => Settings.FormatTableContent;

        private bool UseTableOuterPipe => Settings.UseTableOuterPipe;

        private bool UseTablePadding => Settings.UseTablePadding;

        public void IncreaseIndent()
        {
            IndentLevel++;
        }

        public void DecreaseIndent()
        {
            if (IndentLevel == 0)
                throw new InvalidOperationException($"{nameof(IndentLevel)} cannot be less than 0.");

            IndentLevel++;
        }

        internal void WriteIndentationIf(bool condition)
        {
            if (condition)
                WriteIndentation();
        }

        public void WriteIndentation()
        {
            for (int i = 0; i < IndentLevel; i++)
                Write(IndentChars);
        }

        public void WriteBold(object value)
        {
            WriteDelimiter(BoldDelimiter, value);
        }

        public void WriteBold(object value1, object value2)
        {
            WriteDelimiter(BoldDelimiter, value1, value2);
        }

        public void WriteBold(object value1, object value2, object value3)
        {
            WriteDelimiter(BoldDelimiter, value1, value2, value3);
        }

        public void Write(BoldText text)
        {
            text.WriteTo(this);
        }

        public void WriteBoldDelimiter()
        {
            Write(BoldDelimiter);
        }

        public void WriteItalic(object value)
        {
            WriteDelimiter(ItalicDelimiter, value);
        }

        public void WriteItalic(object value1, object value2)
        {
            WriteDelimiter(ItalicDelimiter, value1, value2);
        }

        public void WriteItalic(object value1, object value2, object value3)
        {
            WriteDelimiter(ItalicDelimiter, value1, value2, value3);
        }

        public void Write(ItalicText text)
        {
            text.WriteTo(this);
        }

        public void WriteItalicDelimiter()
        {
            Write(ItalicDelimiter);
        }

        public void WriteBoldItalic(object value)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, value);
        }

        public void WriteBoldItalic(object value1, object value2)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, value1, value2);
        }

        public void WriteBoldItalic(object value1, object value2, object value3)
        {
            WriteDelimiters(BoldDelimiter, AlternativeItalicDelimiter, value1, value2, value3);
        }

        public void Write(BoldItalicText text)
        {
            text.WriteTo(this);
        }

        public void WriteStrikethrough(object value)
        {
            WriteDelimiter(StrikethroughDelimiter, value);
        }

        public void WriteStrikethrough(object value1, object value2)
        {
            WriteDelimiter(StrikethroughDelimiter, value1, value2);
        }

        public void WriteStrikethrough(object value1, object value2, object value3)
        {
            WriteDelimiter(StrikethroughDelimiter, value1, value2, value3);
        }

        public void Write(StrikethroughText text)
        {
            text.WriteTo(this);
        }

        public void WriteStrikethroughDelimiter()
        {
            Write(StrikethroughDelimiter);
        }

        public void WriteHeader1(object value = null)
        {
            WriteHeader(1, value);
        }

        public void WriteHeader2(object value = null)
        {
            WriteHeader(2, value);
        }

        public void WriteHeader3(object value = null)
        {
            WriteHeader(3, value);
        }

        public void WriteHeader4(object value = null)
        {
            WriteHeader(4, value);
        }

        public void WriteHeader5(object value = null)
        {
            WriteHeader(5, value);
        }

        public void WriteHeader6(object value = null)
        {
            WriteHeader(6, value);
        }

        public void WriteHeader(int level, object value = null)
        {
            WriteLineMarkdown(HeaderStart(level), value, indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader);
        }

        public void WriteHeader(int level, object value1, object value2)
        {
            WriteLineMarkdown(HeaderStart(level), value1, value2, indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader);
        }

        public void WriteHeader(int level, object value1, object value2, object value3)
        {
            WriteLineMarkdown(HeaderStart(level), value1, value2, value3, indent: false, emptyLineBefore: AddEmptyLineBeforeHeader, emptyLineAfter: AddEmptyLineAfterHeader);
        }

        public void Write(Header header)
        {
            header.WriteTo(this);
        }

        public void WriteHeaderStart(int level)
        {
            Write(HeaderStart(level));
        }

        public void WriteListItem(object value = null)
        {
            WriteItem(ListItemStart, value);
        }

        public void WriteListItem(object value1, object value2)
        {
            WriteItem(ListItemStart, value1, value2);
        }

        public void WriteListItem(object value1, object value2, object value3)
        {
            WriteItem(ListItemStart, value1, value2, value3);
        }

        public void Write(ListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteListItemStart()
        {
            Write(ListItemStart);
        }

        public void WriteOrderedListItem(int number, object value = null)
        {
            WriteItem(OrderedListItemStart(number), value);
        }

        public void WriteOrderedListItem(int number, object value1, object value2)
        {
            WriteItem(OrderedListItemStart(number), value1, value2);
        }

        public void WriteOrderedListItem(int number, object value1, object value2, object value3)
        {
            WriteItem(OrderedListItemStart(number), value1, value2, value3);
        }

        public void Write(OrderedListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteOrderedListItemStart(int number)
        {
            Write(number);
            Write(". ");
        }

        public void WriteTaskListItem(object value = null, bool isCompleted = false)
        {
            WriteItem(TaskListItemStart(isCompleted), value);
        }

        public void WriteTaskListItem(object value1, object value2, bool isCompleted = false)
        {
            WriteItem(TaskListItemStart(isCompleted), value1, value2);
        }

        public void WriteTaskListItem(object value1, object value2, object value3, bool isCompleted = false)
        {
            WriteItem(TaskListItemStart(isCompleted), value1, value2, value3);
        }

        public void Write(TaskListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteCompletedTaskListItem(object value = null)
        {
            WriteTaskListItem(value, isCompleted: true);
        }

        public void WriteCompletedTaskListItem(object value1, object value2)
        {
            WriteTaskListItem(value1, value2, isCompleted: true);
        }

        public void WriteCompletedTaskListItem(object value1, object value2, object value3)
        {
            WriteTaskListItem(value1, value2, value3, isCompleted: true);
        }

        public void WriteTaskListItemStart(bool isCompleted = false)
        {
            Write(TaskListItemStart(isCompleted));
        }

        public void WriteItem(string prefix, object value)
        {
            WriteLineMarkdown(prefix, value, indent: true, emptyLineBefore: false, emptyLineAfter: false);
        }

        public void WriteItem(string prefix, object value1, object value2)
        {
            WriteLineMarkdown(prefix, value1, value2, indent: true, emptyLineBefore: false, emptyLineAfter: false);
        }

        public void WriteItem(string prefix, object value1, object value2, object value3)
        {
            WriteLineMarkdown(prefix, value1, value2, value3, indent: true, emptyLineBefore: false, emptyLineAfter: false);
        }

        public void WriteImage(string text, string url)
        {
            Write("![");
            WriteMarkdown(text, shouldBeEscaped: f => f == '[' || f == ']');
            Write("](");
            WriteMarkdown(url, shouldBeEscaped: f => f == '(' || f == ')');
            Write(")");
        }

        public void Write(Image image)
        {
            image.WriteTo(this);
        }

        public void WriteLink(string text, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                WriteMarkdown(text);
            }
            else
            {
                Write("[");
                WriteMarkdown(text, shouldBeEscaped: f => f == '[' || f == ']');
                Write("](");
                WriteMarkdown(url, shouldBeEscaped: f => f == '(' || f == ')');
                Write(")");
            }
        }

        public void Write(Link link)
        {
            link.WriteTo(this);
        }

        public void WriteCode(object value)
        {
            WriteDelimiter(CodeDelimiter, value);
        }

        public void WriteCode(object value1, object value2)
        {
            WriteDelimiter(CodeDelimiter, value1, value2);
        }

        public void WriteCode(object value1, object value2, object value3)
        {
            WriteDelimiter(CodeDelimiter, value1, value2, value3);
        }

        public void Write(CodeText code)
        {
            code.WriteTo(this);
        }

        public void WriteCodeDelimiter()
        {
            Write(CodeDelimiter);
        }

        public void WriteCodeBlock(string code, string language = null)
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
        }

        public void Write(CodeBlock codeBlock)
        {
            codeBlock.WriteTo(this);
        }

        public void WriteQuoteBlock(string text = null)
        {
            WriteWithIndentation(text, prefix: "> ");
        }

        public void Write(QuoteBlock blockQuote)
        {
            blockQuote.WriteTo(this);
        }

        private void WriteWithIndentation(string code, string prefix = null, bool shouldEndWithNewLine = true)
        {
            if (IndentLevel == 0
                && prefix == null)
            {
                Write(code);
            }
            else
            {
                int length = code.Length;

                for (int i = 0; i < length; i++)
                {
                    char ch = code[i];

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
                            ch = code[i + 1];

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
                char last = code[code.Length - 1];

                if (last != 10
                    && last != 13)
                {
                    WriteLine();
                }
            }
        }

        public void WriteCodeBlockChars()
        {
            Write(CodeBlockChars);
        }

        public void WriteHorizonalRule()
        {
            WriteLine(Settings.HorizontalRule);
        }

        public void Write(Table table)
        {
            table.WriteTo(this);
        }

        public void WriteTableHeader(params TableHeader[] headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int length = headers.Length;

            if (length == 0)
                return;

            WriteTableHeader(headers, length);
        }

        public void WriteTableHeader(IList<TableHeader> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int count = headers.Count;

            if (count == 0)
                return;

            WriteTableHeader(headers, count);
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

        public void WriteComment(string value)
        {
            Write("<!-- ");
            Write(value);
            Write(" -->");
        }

        public override void Write(object value)
        {
            Write(value, escape: true);
        }

        public void Write(object value, bool escape)
        {
            if (value == null)
                return;

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
        }

        public void Write(string value, bool escape)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (escape)
            {
                WriteMarkdown(value, Settings.ShouldBeEscaped);
            }
            else
            {
                Write(value);
            }
        }

        private void WriteDelimiter(string delimiter, object value)
        {
            Write(delimiter);
            WriteMarkdown(value);
            Write(delimiter);
        }

        private void WriteDelimiter(string delimiter, object value1, object value2)
        {
            Write(delimiter);
            WriteMarkdown(value1, value2);
            Write(delimiter);
        }

        private void WriteDelimiter(string delimiter, object value1, object value2, object value3)
        {
            Write(delimiter);
            WriteMarkdown(value1, value2, value3);
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

        private void WriteDelimiters(string delimiter, string delimiter2, object value1, object value2)
        {
            Write(delimiter);
            Write(delimiter2);
            WriteMarkdown(value1, value2);
            Write(delimiter2);
            Write(delimiter);
        }

        private void WriteDelimiters(string delimiter, string delimiter2, object value1, object value2, object value3)
        {
            Write(delimiter);
            Write(delimiter2);
            WriteMarkdown(value1, value2, value3);
            Write(delimiter2);
            Write(delimiter);
        }

        public void WriteLineMarkdown(string prefix, object value, bool indent = false, bool emptyLineBefore = false, bool emptyLineAfter = false)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(value), emptyLineAfter);
        }

        public void WriteLineMarkdown(string prefix, object value1, object value2, bool indent = false, bool emptyLineBefore = false, bool emptyLineAfter = false)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(value1, value2), emptyLineAfter);
        }

        public void WriteLineMarkdown(string prefix, object value1, object value2, object value3, bool indent = false, bool emptyLineBefore = false, bool emptyLineAfter = false)
        {
            WriteEmptyLineIf(emptyLineBefore);
            WriteIndentationIf(indent);
            Write(prefix);
            WriteLineIf(WriteMarkdownInternal(value1, value2, value3), emptyLineAfter);
        }

        public void WriteMarkdown(string value, bool escape = true)
        {
            Write(value, escape: escape);
        }

        public void WriteMarkdown(object value, bool escape = true)
        {
            Write(value, escape: escape);
        }

        public void WriteMarkdown(object value1, object value2, bool escape = true)
        {
            Write(value1, escape: escape);
            Write(value2, escape: escape);
        }

        public void WriteMarkdown(object value1, object value2, object value3, bool escape = true)
        {
            Write(value1, escape: escape);
            Write(value2, escape: escape);
            Write(value3, escape: escape);
        }

        internal bool WriteMarkdownInternal(object value)
        {
            int length = Length;

            WriteMarkdown(value);

            return length != Length;
        }

        internal bool WriteMarkdownInternal(object value1, object value2)
        {
            int length = Length;

            WriteMarkdown(value1, value2);

            return length != Length;
        }

        internal bool WriteMarkdownInternal(object value1, object value2, object value3)
        {
            int length = Length;

            WriteMarkdown(value1, value2, value3);

            return length != Length;
        }

        public void WriteMarkdown(string value, Func<char, bool> shouldBeEscaped)
        {
            if (string.IsNullOrEmpty(value))
                return;

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
                return;
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