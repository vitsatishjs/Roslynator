// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Roslynator.Utilities.Markdown.MarkdownFactory;

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownWriter : StringWriter, IMarkdownWriter
    {
        public MarkdownWriter(MarkdownSettings settings = null)
        {
            Settings = settings ?? MarkdownSettings.Default;
        }

        public MarkdownWriter(IFormatProvider formatProvider, MarkdownSettings settings = null)
            : base(formatProvider)
        {
            Settings = settings ?? MarkdownSettings.Default;
        }

        public MarkdownWriter(StringBuilder sb, MarkdownSettings settings = null)
            : base(sb)
        {
            Settings = settings ?? MarkdownSettings.Default;
        }

        public MarkdownWriter(StringBuilder sb, IFormatProvider formatProvider, MarkdownSettings settings = null)
            : base(sb, formatProvider)
        {
            Settings = settings ?? MarkdownSettings.Default;
        }

        public MarkdownSettings Settings { get; }

        public int IndentLevel { get; private set; }

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

        public void WriteBlockQuote(string value = null)
        {
            WriteWithIndentation(value, prefix: "> ");
        }

        public void WriteBold(string value)
        {
            WriteBoldDelimiter();
            Write(value);
            WriteBoldDelimiter();
        }

        public void WriteBoldDelimiter()
        {
            Write(Settings.BoldDelimiter);
        }

        public void WriteCodeBlock(string code, string language = null)
        {
            WriteIndentation();
            WriteCodeBlockStart(language);

            if (string.IsNullOrEmpty(code))
            {
                WriteCodeBlockEnd();
                return;
            }

            WriteWithIndentation(code);

            WriteIndentation();
            WriteCodeBlockEnd();
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

        public void WriteCSharpCodeBlock(string code)
        {
            WriteCodeBlock(code, LanguageIdentifiers.CSharp);
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
        }

        public void WriteCodeBlockChars()
        {
            Write(Settings.CodeBlockChars);
        }

        public void WriteHeader(MarkdownHeader header)
        {
            header.WriteTo(this);
        }

        public void WriteHeader(string value, int level)
        {
            WriteHeaderStart(level);
            WriteLineMarkdownIf(!string.IsNullOrEmpty(value), value);
        }

        public void WriteHeaderStart(int level)
        {
            Write(HeaderStart(level));
        }

        public void WriteHeader1(string value = null)
        {
            WriteHeader(value, 1);
        }

        public void WriteHeader2(string value = null)
        {
            WriteHeader(value, 2);
        }

        public void WriteHeader3(string value = null)
        {
            WriteHeader(value, 3);
        }

        public void WriteHeader4(string value = null)
        {
            WriteHeader(value, 4);
        }

        public void WriteHeader5(string value = null)
        {
            WriteHeader(value, 5);
        }

        public void WriteHeader6(string value = null)
        {
            WriteHeader(value, 6);
        }

        public void WriteHorizonalRule()
        {
            WriteLine(Settings.HorizontalRule);
        }

        public void WriteImage(string text, string url)
        {
            Write("![");
            WriteMarkdown(text);
            Write("](");
            Write(url);
            Write(")");
        }

        public void WriteInlineCode(string code)
        {
            WriteInlineCodeDelimiter();
            Write(code);
            WriteInlineCodeDelimiter();
        }

        public void WriteInlineCodeDelimiter()
        {
            Write(Settings.InlineCodeDelimiter);
        }

        public void WriteItalic(string value)
        {
            WriteItalicDelimiter();
            Write(value);
            WriteItalicDelimiter();
        }

        public void WriteItalicDelimiter()
        {
            Write(Settings.ItalicDelimiter);
        }

        public void WriteLink(string text, string url)
        {
            Write("[");
            WriteMarkdown(text);
            Write("](");
            Write(url);
            Write(")");
        }

        public void WriteListItem(string value = null)
        {
            WriteIndentation();

            WriteListItemStart();
            WriteLineMarkdownIf(!string.IsNullOrEmpty(value), value);
        }

        public void WriteListItemStart()
        {
            Write(Settings.ListItemStart);
            Write(" ");
        }

        public void WriteOrderedListItem(int number, string value = null)
        {
            WriteIndentation();

            WriteOrderedListItemStart(number);
            WriteLineMarkdownIf(!string.IsNullOrEmpty(value), value);
        }

        private void WriteOrderedListItemStart(int number)
        {
            Write(number);
            Write(". ");
        }

        public void WriteStrikethrough(string value)
        {
            WriteStrikethroughDelimiter();
            WriteMarkdown(value);
            WriteStrikethroughDelimiter();
        }

        public void WriteStrikethroughDelimiter()
        {
            Write(Settings.StrikethroughDelimiter);
        }

        public void WriteTableHeader(params MarkdownTableHeader[] headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            int length = headers.Length;

            if (length == 0)
                return;

            WriteTableHeader(headers, length);
        }

        internal void WriteTableHeader(IList<MarkdownTableHeader> headers, int length, List<int> widths = null)
        {
            for (int i = 0; i < length; i++)
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

                WriteTableCellEnd(i, length);
            }

            WriteLine();

            for (int i = 0; i < length; i++)
            {
                MarkdownTableHeader header = headers[i];

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

                if (i == length - 1)
                    WriteTableOuterPipe();
            }

            WriteLine();
        }

        private void WritePadRight(int width, int? proposedwidth, int minimalwidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedwidth ?? width, minimalwidth);

            for (int j = width; j < totalWidth; j++)
            {
                base.Write(paddingChar);
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

        public void WriteTableRow(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            int length = values.Length;

            if (length == 0)
                return;

            WriteTableRow(values, length);
        }

        internal void WriteTableRow(IList<object> values, int length, List<int> widths = null)
        {
            for (int i = 0; i < length; i++)
            {
                WriteTableRowStart(i);
                WriteTablePadding();

                if (values[i] is IMarkdown markdown)
                {
                    WriteTableCellContent(markdown, widths?[i]);
                }
                else
                {
                    string value = values[i]?.ToString() ?? "";
                    WriteMarkdown(value);

                    if (Settings.FormatTableContent)
                        WritePadRight(value.Length, widths?[i]);
                }

                WriteTableCellEnd(i, length);
            }

            WriteLine();
        }

        public void WriteTableRow(params MarkdownText[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            int length = values.Length;

            if (length == 0)
                return;

            WriteTableRow(values, length);
        }

        internal void WriteTableRow(IList<MarkdownText> values, int length, List<int> widths = null)
        {
            for (int i = 0; i < length; i++)
            {
                WriteTableRowStart(i);
                WriteTablePadding();

                WriteTableCellContent(values[i], widths?[i]);

                WriteTableCellEnd(i, length);
            }

            WriteLine();
        }

        private void WriteTableCellContent<T>(T value, int? proposedWidth) where T : IMarkdown
        {
            StringBuilder sb = GetStringBuilder();

            int length = sb.Length;

            value.WriteTo(this);

            length = sb.Length - length;

            if (Settings.FormatTableContent)
                WritePadRight(length, proposedWidth);
        }

        public void WriteTableRowStart()
        {
            if (Settings.UseTableOuterPipe)
            {
                WriteTableDelimiter();
                WriteTablePadding();
            }
        }

        private void WriteTableDelimiter()
        {
            Write(Settings.TableDelimiter);
        }

        public void WriteTableRowEnd()
        {
            if (Settings.UseTableOuterPipe)
            {
                WriteTablePadding();
                WriteTableDelimiter();
            }

            WriteLine();
        }

        public void WriteTableRowSeparator()
        {
            WriteTablePadding();
            WriteTableDelimiter();
            WriteTablePadding();
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

        public void WriteTaskListItem(string value = null, bool isCompleted = false)
        {
            WriteTaskListItemStart(isCompleted);
            WriteMarkdownIf(!string.IsNullOrEmpty(value), value);
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

        public void WriteComment(string value)
        {
            Write("<!-- ");
            Write(value);
            Write(" -->");
        }

        public void WriteMarkdown(BoldText text)
        {
            text.WriteTo(this);
        }

        public void WriteMarkdown(CodeBlock codeBlock)
        {
            codeBlock.WriteTo(this);
        }

        public void WriteMarkdown(InlineCode inlineCode)
        {
            inlineCode.WriteTo(this);
        }

        public void WriteMarkdown(ItalicText text)
        {
            text.WriteTo(this);
        }

        public void WriteMarkdown(ListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteMarkdown(MarkdownHeader header)
        {
            header.WriteTo(this);
        }

        public void WriteMarkdown(MarkdownImage image)
        {
            image.WriteTo(this);
        }

        public void WriteMarkdown(MarkdownLink link)
        {
            link.WriteTo(this);
        }

        public void WriteMarkdown(MarkdownText text)
        {
            text.WriteTo(this);
        }

        public void WriteMarkdown(OrderedListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteMarkdown(StrikethroughText text)
        {
            text.WriteTo(this);
        }

        public void WriteMarkdown(TaskListItem item)
        {
            item.WriteTo(this);
        }

        public void WriteJoin<T>(string separator, IEnumerable<T> values) where T : IMarkdown
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            bool isFirst = true;

            foreach (T value in values)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    Write(separator);
                }

                value.WriteTo(this);
            }
        }

        public override void Write(object value)
        {
            if (value is IMarkdown markdown)
            {
                markdown.WriteTo(this);
            }
            else
            {
                base.Write(value);
            }
        }

        public void WriteMarkdown(string value, bool escape = true)
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

        internal void WriteLineMarkdownIf(bool condition, string value, bool escape = true)
        {
            if (condition)
            {
                WriteMarkdown(value, escape);
                WriteLine();
            }
        }

        internal void WriteLineIf(bool condition)
        {
            if (condition)
                WriteLine();
        }
    }
}