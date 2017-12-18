// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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

        internal int Length
        {
            get { return _sb.Length; }
        }

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

        public void WriteIndentation()
        {
            for (int i = 0; i < IndentLevel; i++)
                Write(Settings.IndentChars);
        }

        public void WriteText(MarkdownText text)
        {
            text.WriteTo(this);
        }

        public void WriteBold(object value)
        {
            WriteBoldDelimiter();
            Write(value);
            WriteBoldDelimiter();
        }

        public void WriteBold(BoldText text)
        {
            text.WriteTo(this);
        }

        public void WriteBoldDelimiter()
        {
            Write(Settings.BoldDelimiter);
        }

        public void WriteItalic(object value)
        {
            WriteItalicDelimiter();
            Write(value);
            WriteItalicDelimiter();
        }

        public void WriteItalic(ItalicText text)
        {
            text.WriteTo(this);
        }

        public void WriteItalicDelimiter()
        {
            Write(Settings.ItalicDelimiter);
        }

        public void WriteStrikethrough(object value)
        {
            WriteStrikethroughDelimiter();
            Write(value);
            WriteStrikethroughDelimiter();
        }

        public void WriteStrikethrough(StrikethroughText text)
        {
            text.WriteTo(this);
        }

        public void WriteStrikethroughDelimiter()
        {
            Write(Settings.StrikethroughDelimiter);
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

        public void WriteHeader(int level, object value)
        {
            WriteEmptyLineIf(Settings.AddEmptyLineBeforeHeader);

            WriteHeaderStart(level);

            if (WriteLineIfAny(value))
                WriteEmptyLineIf(Settings.AddEmptyLineAfterHeader);
        }

        public void WriteHeader(Header header)
        {
            header.WriteTo(this);
        }

        public void WriteHeaderStart(int level)
        {
            Write(GetHeaderStart());

            string GetHeaderStart()
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
        }

        public void WriteListItem(object value = null)
        {
            WriteIndentation();
            WriteListItemStart();
            WriteLineIfAny(value);
        }

        public void WriteListItem(ListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteListItemStart()
        {
            Write(Settings.ListItemStart);
            Write(" ");
        }

        public void WriteOrderedListItem(int number, object value = null)
        {
            WriteIndentation();
            WriteOrderedListItemStart(number);
            WriteLineIfAny(value);
        }

        public void WriteOrderedListItem(OrderedListItem item)
        {
            item.WriteTo(this);
        }

        private void WriteOrderedListItemStart(int number)
        {
            Write(number);
            Write(". ");
        }

        public void WriteTaskListItem(object value = null, bool isCompleted = false)
        {
            WriteIndentation();
            WriteTaskListItemStart(isCompleted);
            WriteLineIfAny(value);
        }

        public void WriteTaskListItem(TaskListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteCompletedTaskListItem(string value = null)
        {
            WriteTaskListItem(value, isCompleted: true);
        }

        private void WriteTaskListItemStart(bool isCompleted = false)
        {
            if (isCompleted)
            {
                Write("- [x] ");
            }
            else
            {
                Write("- [ ] ");
            }
        }

        public void WriteImage(string text, string url)
        {
            Write("![");
            WriteMarkdown(text, shouldBeEscaped: f => f == '[' || f == ']');
            Write("](");
            WriteMarkdown(url, shouldBeEscaped: f => f == '(' || f == ')');
            Write(")");
        }

        public void WriteImage(Image image)
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

        public void WriteLink(Link link)
        {
            link.WriteTo(this);
        }

        public void WriteInlineCode(object code)
        {
            WriteInlineCodeDelimiter();
            Write(code);
            WriteInlineCodeDelimiter();
        }

        public void WriteInlineCode(InlineCode code)
        {
            code.WriteTo(this);
        }

        public void WriteInlineCodeDelimiter()
        {
            Write(Settings.InlineCodeDelimiter);
        }

        public void WriteCodeBlock(string code, string language = null)
        {
            WriteEmptyLineIf(Settings.AddEmptyLineBeforeCodeBlock);

            WriteIndentation();
            WriteCodeBlockStart(language);

            WriteWithIndentation(code);

            WriteIndentation();
            WriteCodeBlockEnd();
        }

        public void WriteCodeBlock(CodeBlock codeBlock)
        {
            codeBlock.WriteTo(this);
        }

        public void WriteBlockQuote(string text = null)
        {
            WriteWithIndentation(text, afterIndentation: "> ");
        }

        public void WriteBlockQuote(BlockQuote blockQuote)
        {
            blockQuote.WriteTo(this);
        }

        private void WriteWithIndentation(string code, string afterIndentation = null, bool shouldEndWithNewLine = true)
        {
            if (IndentLevel == 0
                && afterIndentation == null)
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

                        Write(afterIndentation);
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

        private void WriteCodeBlockStart(string language = null)
        {
            WriteCodeBlockChars();
            Write(language);
            WriteLine();
        }

        private void WriteCodeBlockEnd()
        {
            WriteCodeBlockChars();
            WriteLine();

            WriteEmptyLineIf(Settings.AddEmptyLineAfterCodeBlock);
        }

        public void WriteCodeBlockChars()
        {
            Write(Settings.CodeBlockChars);
        }

        public void WriteHorizonalRule()
        {
            WriteLine(Settings.HorizontalRule);
        }

        public void WriteTable(Table table)
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

                if (Settings.FormatTableHeader)
                {
                    int width = name.Length;
                    int minimalWidth = width;

                    if (Settings.FormatTableHeader)
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

                if (Settings.FormatTableHeader)
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

                if (Settings.FormatTableContent)
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
                if (Settings.UseTableOuterPipe)
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
            if (Settings.UseTableOuterPipe)
                WriteTableDelimiter();
        }

        private void WriteTablePadding()
        {
            if (Settings.UseTablePadding)
                Write(" ");
        }

        public void WriteTableDelimiter()
        {
            Write(Settings.TableDelimiter);
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

        public void WriteMarkdown(string value, bool escape = true)
        {
            Write(value, escape: escape);
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

        internal void WriteMarkdownIf(bool condition, string value, bool escape = true)
        {
            if (condition)
                WriteMarkdown(value, escape);
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (!condition)
                return;

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

        internal void WriteLineIf(bool condition)
        {
            if (condition)
                WriteLine();
        }

        private bool WriteLineIfAny(object value)
        {
            int length = Length;

            Write(value);

            if (length != Length)
            {
                WriteLine();
                return true;
            }

            return false;
        }
    }
}