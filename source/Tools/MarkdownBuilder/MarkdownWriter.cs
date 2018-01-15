// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Pihrtsoft.Markdown.Linq;
using static Pihrtsoft.Markdown.Linq.MFactory;
using static Pihrtsoft.Markdown.TextUtility;

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;
        private bool _startOfDocument = true;
        private bool _startOfLine = true;
        private bool _emptyLine;
        private bool _pendingEmptyLine;

        private bool _indentedCodeBlock;
        private int _listItemLevel;

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
        }

        public virtual MarkdownWriterSettings Settings { get; internal set; }

        public MarkdownFormat Format
        {
            get { return Settings.Format; }
        }

        public int QuoteLevel { get; private set; }

        protected internal abstract int Length { get; set; }

        public static MarkdownWriter Create(StringBuilder output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownTextWriter(new StringWriter(output, CultureInfo.InvariantCulture), settings);
        }

        public static MarkdownWriter Create(TextWriter output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownTextWriter(output, settings);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Close();

                _disposed = true;
            }
        }

        public virtual void Close()
        {
            Dispose();
        }

        internal virtual void Reset()
        {
            _startOfDocument = true;
            _startOfLine = true;
        }

        public MarkdownWriter WriteBold(object content)
        {
            return WriteDelimiter(BoldDelimiter(Format.BoldStyle), content);
        }

        public MarkdownWriter WriteBold(params object[] content)
        {
            return WriteDelimiter(BoldDelimiter(Format.BoldStyle), content);
        }

        public MarkdownWriter WriteItalic(object content)
        {
            return WriteDelimiter(ItalicDelimiter(Format.ItalicStyle), content);
        }

        public MarkdownWriter WriteItalic(params object[] content)
        {
            return WriteDelimiter(ItalicDelimiter(Format.ItalicStyle), content);
        }

        public MarkdownWriter WriteStrikethrough(object content)
        {
            return WriteDelimiter(StrikethroughDelimiter, content);
        }

        public MarkdownWriter WriteStrikethrough(params object[] content)
        {
            return WriteDelimiter(StrikethroughDelimiter, content);
        }

        private MarkdownWriter WriteDelimiter(string delimiter, object value)
        {
            WriteSyntax(delimiter);
            Write(value);
            WriteSyntax(delimiter);

            return this;
        }

        public MarkdownWriter WriteInlineCode(string value)
        {
            WriteSyntax(CodeDelimiter);

            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == CodeDelimiterChar)
                    WriteSpace();

                Write(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);

                if (value[value.Length - 1] == CodeDelimiterChar)
                    WriteSpace();
            }

            WriteSyntax(CodeDelimiter);
            return this;
        }

        public MarkdownWriter WriteHeading1(object content)
        {
            return WriteHeading(1, content);
        }

        public MarkdownWriter WriteHeading1(params object[] content)
        {
            return WriteHeading(1, content);
        }

        public MarkdownWriter WriteHeading2(object content)
        {
            return WriteHeading(2, content);
        }

        public MarkdownWriter WriteHeading2(params object[] content)
        {
            return WriteHeading(2, content);
        }

        public MarkdownWriter WriteHeading3(object content)
        {
            return WriteHeading(3, content);
        }

        public MarkdownWriter WriteHeading3(params object[] content)
        {
            return WriteHeading(3, content);
        }

        public MarkdownWriter WriteHeading4(object content)
        {
            return WriteHeading(4, content);
        }

        public MarkdownWriter WriteHeading4(params object[] content)
        {
            return WriteHeading(4, content);
        }

        public MarkdownWriter WriteHeading5(object content)
        {
            return WriteHeading(5, content);
        }

        public MarkdownWriter WriteHeading5(params object[] content)
        {
            return WriteHeading(5, content);
        }

        public MarkdownWriter WriteHeading6(object content)
        {
            return WriteHeading(6, content);
        }

        public MarkdownWriter WriteHeading6(params object[] content)
        {
            return WriteHeading(6, content);
        }

        public MarkdownWriter WriteHeading(int level, object content)
        {
            return WriteHeadingCore(level, content);
        }

        public MarkdownWriter WriteHeading(int level, params object[] content)
        {
            return WriteHeadingCore(level, content);
        }

        private MarkdownWriter WriteHeadingCore(int level, object content)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeHeading);

            if (!underline)
            {
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
                WriteSpace();
            }

            int length = WriteGetLength(content);

            if (length > 0
                && !underline
                && Format.CloseHeading)
            {
                WriteSpace();
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
            }

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? '=' : '-', length);
                WriteLine();
            }

            if (Format.EmptyLineAfterHeading)
                _pendingEmptyLine = true;

            return this;
        }

        public MarkdownWriter WriteBulletItem(object content)
        {
            return WriteItem(prefix1: null, prefix2: BulletItemStart(Format.BulletListStyle), content: content);
        }

        public MarkdownWriter WriteBulletItem(params object[] content)
        {
            return WriteBulletItem((object)content);
        }

        public MarkdownWriter WriteOrderedItem(int number, object content)
        {
            Error.ThrowOnInvalidItemNumber(number);

            return WriteItem(prefix1: number.ToString(), prefix2: OrderedItemStart(Format.OrderedListStyle), content: content);
        }

        public MarkdownWriter WriteOrderedItem(int number, params object[] content)
        {
            return WriteOrderedItem(number, (object)content);
        }

        public MarkdownWriter WriteTaskItem(object content)
        {
            return WriteItem(prefix1: null, prefix2: TaskItemStart(), content: content);
        }

        public MarkdownWriter WriteTaskItem(params object[] content)
        {
            return WriteTaskItem((object)content);
        }

        public MarkdownWriter WriteCompletedTaskItem(object content)
        {
            return WriteItem(prefix1: null, prefix2: TaskItemStart(isCompleted: true), content: content);
        }

        public MarkdownWriter WriteCompletedTaskItem(params object[] content)
        {
            return WriteCompletedTaskItem((object)content);
        }

        private MarkdownWriter WriteItem(string prefix1, string prefix2, object content)
        {
            WriteLineIfNecessary();
            WriteSyntax(prefix1);
            WriteSyntax(prefix2);

            _listItemLevel++;

            Write(content);
            WriteLineIfNecessary();

            _listItemLevel--;
            return this;
        }

        public MarkdownWriter WriteBulletItems(params MElement[] content)
        {
            return WriteBulletItems((IEnumerable<MElement>)content);
        }

        public MarkdownWriter WriteBulletItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is MBulletItem item)
                {
                    WriteBulletItem(item.TextOrElements());
                }
                else
                {
                    WriteBulletItem(element);
                }
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteOrderedItems(params MElement[] content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            for (int i = 0; i < content.Length; i++)
            {
                MElement element = content[i];

                if (element is MBulletItem item)
                {
                    WriteOrderedItem(i + 1, item.TextOrElements());
                }
                else
                {
                    WriteOrderedItem(i + 1, element);
                }
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteOrderedItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            int number = 1;
            foreach (MElement element in content)
            {
                if (element is MBulletItem item)
                {
                    WriteOrderedItem(number, item.TextOrElements());
                }
                else
                {
                    WriteOrderedItem(number, element);
                }

                number++;
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteTaskItems(params MElement[] content)
        {
            return WriteTaskItems((IEnumerable<MElement>)content);
        }

        public MarkdownWriter WriteTaskItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is MBulletItem item)
                {
                    WriteTaskItem(item.TextOrElements());
                }
                else
                {
                    WriteTaskItem(element);
                }
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteImage(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            WriteSyntax("!");
            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLinkOrText(string text, string url = null, string title = null)
        {
            return Write(LinkOrText(text, url, title));
        }

        private MarkdownWriter WriteLinkCore(string text, string url, string title)
        {
            WriteSquareBrackets(text);
            WriteSyntax("(");
            Write(url, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            WriteLinkTitle(title);
            WriteSyntax(")");
            return this;
        }

        public MarkdownWriter WriteAutolink(string url)
        {
            Error.ThrowOnInvalidUrl(url);

            WriteAngleBrackets(url);
            return this;
        }

        public MarkdownWriter WriteImageReference(string text, string label)
        {
            WriteSyntax("!");
            WriteLinkReferenceCore(text, label);
            return this;
        }

        public MarkdownWriter WriteLinkReference(string text, string label = null)
        {
            WriteLinkReferenceCore(text, label);
            return this;
        }

        private MarkdownWriter WriteLinkReferenceCore(string text, string label = null)
        {
            WriteSquareBrackets(text);
            WriteSquareBrackets(label);
            return this;
        }

        public MarkdownWriter WriteLabel(string label, string url, string title = null)
        {
            Error.ThrowOnInvalidUrl(url);

            WriteLineIfNecessary();
            WriteSquareBrackets(label);
            WriteSyntax(": ");
            WriteAngleBrackets(url);
            WriteLinkTitle(title);
            WriteLineIfNecessary();
            return this;
        }

        private void WriteLinkTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                WriteSpace();

                WriteSyntax("\"");
                Write(title, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                WriteSyntax("\"");
            }
        }

        private void WriteSquareBrackets(string value)
        {
            WriteSyntax("[");
            Write(value, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkText);
            WriteSyntax("]");
        }

        private void WriteAngleBrackets(string value)
        {
            WriteSyntax("<");
            Write(value, shouldBeEscaped: ch => ch == '<' || ch == '>');
            WriteSyntax(">");
        }

        public MarkdownWriter WriteIndentedCodeBlock(string text)
        {
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);
            _indentedCodeBlock = true;
            WriteRaw(text);
            WriteLineIfNecessary();
            _indentedCodeBlock = false;

            if (Format.EmptyLineAfterCodeBlock)
                _pendingEmptyLine = true;

            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);

            WriteCodeFence();
            WriteSyntax(info);
            WriteLine();

            WriteRaw(text);
            WriteLineIfNecessary();

            WriteCodeFence();
            WriteLine();

            if (Format.EmptyLineAfterCodeBlock)
                _pendingEmptyLine = true;

            return this;
        }

        private MarkdownWriter WriteCodeFence()
        {
            switch (Format.CodeFenceStyle)
            {
                case CodeFenceStyle.Backtick:
                    return WriteSyntax("```");
                case CodeFenceStyle.Tilde:
                    return WriteSyntax("~~~");
                default:
                    throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CodeFenceStyle));
            }
        }

        public MarkdownWriter WriteBlockQuote(object content)
        {
            QuoteLevel++;
            Write(content);
            WriteLineIfNecessary();
            QuoteLevel--;
            return this;
        }

        public MarkdownWriter WriteBlockQuote(params object[] content)
        {
            return WriteBlockQuote((object)content);
        }

        public MarkdownWriter WriteHorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            Error.ThrowOnInvalidHorizontalRuleCount(count);

            WriteLineIfNecessary();

            char ch = HorizontalRuleChar(style);

            bool isFirst = true;

            for (int i = 0; i < count; i++)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    WriteRaw(space);
                }

                WriteRaw(ch);
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteTable(params MElement[] rows)
        {
            return WriteTable((IEnumerable<MElement>)rows);
        }

        public MarkdownWriter WriteTable(IEnumerable<MElement> rows)
        {
            List<TableColumnInfo> columns = AnalyzeTable(rows);

            if (columns == null)
                return this;

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    WriteTableHeader(en.Current, columns);

                    while (en.MoveNext())
                        WriteTableRow(en.Current, columns);
                }
            }

            return this;
        }

        protected abstract List<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows);

        internal void WriteTableHeader(MElement content, IList<TableColumnInfo> columns)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            bool isFirst = true;
            bool isLast = false;

            int i = 0;

            if (content is MContainer container)
            {
                using (IEnumerator<MElement> en = container.Elements().GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        MElement curr = en.Current;

                        isLast = !en.MoveNext();

                        WriteCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                WriteCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                WriteCell(content);
            }

            WriteLine();
            WriteTableHeaderSeparator(columns);

            void WriteCell(MElement cellContent)
            {
                Alignment alignment = columns[i].Alignment;

                WriteTableCellStart(isFirst, isLast, columns[i].IsWhiteSpace);

                if (Format.TablePadding)
                {
                    WriteSpace();
                }
                else if (Settings.FormatTableHeader
                    && alignment == Alignment.Center)
                {
                    WriteSpace();
                }

                int width = WriteGetLength(cellContent);

                if (Settings.FormatTableHeader)
                {
                    int minimalWidth = Math.Max(width, 3);

                    WritePadRight(width, columns[i].Width, minimalWidth);

                    if (!Format.TablePadding
                        && alignment != Alignment.Left)
                    {
                        WriteSpace();
                    }
                }

                WriteTableCellEnd(isLast, columns[i].IsWhiteSpace);
            }
        }

        private void WriteTableHeaderSeparator(IList<TableColumnInfo> columns)
        {
            int count = columns.Count;

            for (int i = 0; i < count; i++)
            {
                TableColumnInfo column = columns[i];

                bool isLast = i == count - 1;

                WriteTableCellStart(i == 0, isLast, column.IsWhiteSpace);

                if (column.Alignment == Alignment.Center)
                {
                    WriteSyntax(":");
                }
                else
                {
                    WriteTablePadding();
                }

                WriteSyntax("---");

                if (Settings.FormatTableHeader)
                    WritePadRight(3, columns[i].Width, 3, '-');

                if (column.Alignment != Alignment.Left)
                {
                    WriteSyntax(":");
                }
                else
                {
                    WriteTablePadding();
                }

                if (isLast)
                {
                    if (Format.TableOuterDelimiter
                        || columns[i].IsWhiteSpace)
                    {
                        WriteSyntax(TableDelimiter);
                    }
                }
            }

            WriteLine();
        }

        internal MarkdownWriter WriteTableRow(MElement content, List<TableColumnInfo> columns = null)
        {
            bool isFirst = true;
            bool isLast = false;

            int i = 0;

            if (content is MContainer container)
            {
                using (IEnumerator<MElement> en = container.Elements().GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        MElement curr = en.Current;

                        isLast = !en.MoveNext();

                        WriteCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                WriteCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                WriteCell(content);
            }

            WriteLine();
            return this;

            void WriteCell(MElement cell)
            {
                WriteTableCellStart(isFirst, isLast);
                WriteTablePadding();

                int length = WriteGetLength(cell);

                if (Settings.FormatTableContent)
                    WritePadRight(length, columns[i].Width);

                WriteTableCellEnd(isLast);
            }
        }

        private void WriteTableCellStart(bool isFirst, bool isLast, bool isWhiteSpace = false)
        {
            if (isFirst)
            {
                WriteLineIfNecessary();

                if (Format.TableOuterDelimiter
                    || isLast
                    || isWhiteSpace)
                {
                    WriteSyntax(TableDelimiter);
                }
            }
            else
            {
                WriteSyntax(TableDelimiter);
            }
        }

        private void WriteTableCellEnd(bool isLast, bool isWhiteSpace = false)
        {
            if (isLast)
            {
                if (Format.TableOuterDelimiter)
                {
                    WriteTablePadding();
                    WriteSyntax(TableDelimiter);
                }
                else if (isWhiteSpace)
                {
                    WriteSyntax(TableDelimiter);
                }
            }
            else
            {
                WriteTablePadding();
            }
        }

        private void WriteTablePadding()
        {
            if (Format.TablePadding)
                WriteSpace();
        }

        private void WritePadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
                WriteRaw(paddingChar);
        }

        public MarkdownWriter WriteCharReference(int number)
        {
            WriteSyntax("&#");

            if (Format.CharReferenceFormat == CharReferenceFormat.Hexadecimal)
            {
                WriteSyntax("x");
                WriteRaw(number.ToString("x", CultureInfo.InvariantCulture));
            }
            else if (Format.CharReferenceFormat == CharReferenceFormat.Decimal)
            {
                WriteRaw(number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CharReferenceFormat));
            }

            WriteSyntax(";");
            return this;
        }

        public MarkdownWriter WriteEntityReference(string name)
        {
            WriteSyntax("&");
            WriteSyntax(name);
            WriteSyntax(";");
            return this;
        }

        public MarkdownWriter WriteComment(string value)
        {
            WriteSyntax("<!-- ");
            WriteRaw(value);
            WriteSyntax(" -->");
            return this;
        }

        private int WriteGetLength(object content)
        {
            int length = Length;
            Write(content);
            return Length - length;
        }

        public MarkdownWriter Write(char value)
        {
            return Write(value, escape: true);
        }

        public MarkdownWriter Write(char value, bool escape)
        {
            if (escape
                && MarkdownEscaper.ShouldBeEscaped(value))
            {
                return WriteRaw('\\');
            }

            return WriteRaw(value);
        }

        public MarkdownWriter Write(string value)
        {
            return Write(value, escape: true);
        }

        public MarkdownWriter Write(string value, bool escape)
        {
            if (escape)
            {
                return Write(value, MarkdownEscaper.ShouldBeEscaped);
            }
            else
            {
                return WriteRaw(value);
            }
        }

        internal MarkdownWriter Write(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            if (value == null)
                return this;

            BeforeWrite();

            int length = value.Length;

            bool f = false;

            for (int i = 0; i < length; i++)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    WriteLine(0, i);
                    f = true;
                }
                else if (ch == 13
                    && (i == length - 1 || value[i + 1] != 10))
                {
                    WriteLine(0, i);
                    f = true;
                }
                else if (shouldBeEscaped(ch))
                {
                    WriteEscapedChar(0, i, ch);
                    f = true;
                }

                if (f)
                {
                    f = false;

                    i++;
                    int lastIndex = i;

                    while (i < value.Length)
                    {
                        ch = value[i];

                        if (ch == 10)
                        {
                            WriteLine(lastIndex, i);
                            f = true;
                        }
                        else if (ch == 13
                            && (i == length - 1 || value[i + 1] != 10))
                        {
                            WriteLine(0, i);
                            f = true;
                        }
                        else if (shouldBeEscaped(ch))
                        {
                            WriteEscapedChar(lastIndex, i, ch);
                            f = true;
                        }

                        if (f)
                        {
                            f = false;
                            i++;
                            lastIndex = i;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    BeforeWrite();
                    WriteString(value, lastIndex, value.Length - lastIndex);
                    return this;
                }
            }

            BeforeWrite();
            WriteString(value);
            return this;

            void WriteLine(int startIndex, int index)
            {
                if (Settings.NewLineHandling == NewLineHandling.Replace)
                {
                    index--;
                    if (index > 0
                        && value[index - 1] == '\r')
                    {
                        index--;
                    }
                }

                BeforeWrite();
                WriteString(value, startIndex, index - startIndex);

                if (Settings.NewLineHandling == NewLineHandling.Replace)
                {
                    this.WriteLine();
                }
                else
                {
                    AfterWriteLine();
                }
            }

            void WriteEscapedChar(int startIndex, int index, char ch)
            {
                BeforeWrite();
                WriteString(value, startIndex, index - startIndex);
                WriteValue(escapingChar);
                WriteValue(ch);
            }
        }

        public MarkdownWriter Write(object value)
        {
            if (value == null)
                return this;

            if (value is MElement element)
                return element.WriteTo(this);

            if (value is string s)
                return Write(s, escape: true);

            if (value is object[] arr)
            {
                foreach (object item in arr)
                    Write(item);

                return this;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    Write(item);

                return this;
            }

            return Write(value.ToString(), escape: true);
        }

        public MarkdownWriter WriteLine(string value, bool escape = true)
        {
            Write(value, escape: escape);
            WriteLine();
            return this;
        }

        public MarkdownWriter WriteLineRaw(string value)
        {
            return WriteLine(value, escape: false);
        }

        private void WriteLineIfNecessary()
        {
            if (!_startOfLine)
                WriteLine();
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            if (!_startOfDocument
                && !_emptyLine)
            {
                WriteLine();
            }
        }

        private MarkdownWriter WriteRaw(char value)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();
            WriteValue(value);
            return this;
        }

        private MarkdownWriter WriteRaw(char value, int repeatCount)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();

            for (int i = 0; i < repeatCount; i++)
                WriteValue(value);

            return this;
        }

        private MarkdownWriter WriteSyntax(string value)
        {
            BeforeWrite();
            WriteString(value);
            return this;
        }

        public MarkdownWriter WriteRaw(string value)
        {
            Write(value, _ => false);
            return this;
        }

        public MarkdownWriter WriteRaw(int value)
        {
            BeforeWrite();
            WriteValue(value);
            return this;
        }

        public virtual MarkdownWriter WriteLine()
        {
            WriteString(Settings.NewLineChars);
            AfterWriteLine();
            return this;
        }

        private void AfterWriteLine()
        {
            _pendingEmptyLine = false;

            if (_startOfLine)
            {
                _emptyLine = true;
            }
            else
            {
                _startOfLine = true;
            }
        }

        private void BeforeWrite()
        {
            if (_pendingEmptyLine)
            {
                WriteIndentation();
                WriteLine();
            }
            else if (_startOfLine)
            {
                WriteIndentation();
                _startOfDocument = false;
                _startOfLine = false;
                _emptyLine = false;
            }
        }

        private void WriteIndentation()
        {
            for (int i = 0; i < QuoteLevel; i++)
                WriteString(BlockQuoteStart);

            for (int i = 0; i < _listItemLevel; i++)
                WriteString("  ");

            if (_indentedCodeBlock)
                WriteString("    ");
        }

        internal MarkdownWriter WriteSpace()
        {
            return WriteSyntax(" ");
        }

        protected abstract void WriteString(string value);

        protected abstract void WriteString(string value, int startIndex, int count);

        protected abstract void WriteValue(char value);

        protected abstract void WriteValue(int value);

        protected abstract void WriteValue(uint value);

        protected abstract void WriteValue(long value);

        protected abstract void WriteValue(ulong value);

        protected abstract void WriteValue(float value);

        protected abstract void WriteValue(double value);

        protected abstract void WriteValue(decimal value);
    }
}