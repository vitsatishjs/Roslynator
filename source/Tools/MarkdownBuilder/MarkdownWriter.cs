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

#pragma warning disable CA1814

namespace Pihrtsoft.Markdown
{
    //TODO: WriteStart, WriteEnd
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;
        private bool _startOfLine;
        private bool _emptyLine;
        private bool _pendingEmptyLine;

        private int _headingPosition = -1;
        private int _headingLevel = -1;

        private IReadOnlyList<TableColumnInfo> _tableColumns;
        private int _tableRowIndex = -1;
        private int _tableColumnIndex = -1;
        private int _tableCellPosition = -1;

        private readonly Stack<MarkdownKind> _containers = new Stack<MarkdownKind>();

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
            _startOfLine = true;
        }

        public virtual MarkdownWriterSettings Settings { get; }

        public MarkdownFormat Format
        {
            get { return Settings.Format; }
        }

        private MarkdownKind CurrentKind
        {
            get { return (_containers.Count > 0) ? _containers.Peek() : MarkdownKind.None; }
        }

        public int QuoteLevel { get; private set; }

        internal int ListLevel { get; private set; }

        protected internal abstract int Length { get; set; }

        private TableColumnInfo CurrentColumn => _tableColumns[_tableColumnIndex];

        private bool IsLastColumn => _tableColumnIndex == _tableColumns.Count - 1;

        private bool IsFirstColumn => _tableColumnIndex == 0;

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
            Length = 0;
            _startOfLine = true;
        }

        private void PushCheck(MarkdownKind kind)
        {
            Check(kind);
            _containers.Push(kind);
        }

        private void Pop(MarkdownKind kind)
        {
            //TODO: 
            if (kind == MarkdownKind.None)
            {
            }

            _containers.Pop();
        }

        internal void Check(MarkdownKind kind)
        {
            //TODO: 
            if (kind == MarkdownKind.None)
            {
            }
        }

        public MarkdownWriter WriteBoldStart()
        {
            PushCheck(MarkdownKind.Bold);
            WriteRaw(BoldDelimiter(Format.BoldStyle));
            return this;
        }

        public MarkdownWriter WriteBoldEnd()
        {
            WriteRaw(BoldDelimiter(Format.BoldStyle));
            Pop(MarkdownKind.Bold);
            return this;
        }

        public MarkdownWriter WriteBold(string text)
        {
            WriteBoldStart();
            Write(text);
            WriteBoldEnd();
            return this;
        }

        public MarkdownWriter WriteItalicStart()
        {
            PushCheck(MarkdownKind.Italic);
            WriteRaw(ItalicDelimiter(Format.ItalicStyle));
            return this;
        }

        public MarkdownWriter WriteItalicEnd()
        {
            WriteRaw(ItalicDelimiter(Format.ItalicStyle));
            Pop(MarkdownKind.Italic);
            return this;
        }

        public MarkdownWriter WriteItalic(string text)
        {
            WriteItalicStart();
            Write(text);
            WriteItalicEnd();
            return this;
        }

        public MarkdownWriter WriteStrikethroughStart()
        {
            PushCheck(MarkdownKind.Strikethrough);
            WriteRaw(StrikethroughDelimiter);
            return this;
        }

        public MarkdownWriter WriteStrikethroughEnd()
        {
            WriteRaw(StrikethroughDelimiter);
            Pop(MarkdownKind.Strikethrough);
            return this;
        }

        public MarkdownWriter WriteStrikethrough(string text)
        {
            WriteStrikethroughStart();
            Write(text);
            WriteStrikethroughEnd();
            return this;
        }

        public MarkdownWriter WriteInlineCode(string text)
        {
            Check(MarkdownKind.InlineCode);
            WriteRaw(CodeDelimiter);

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == CodeDelimiterChar)
                    WriteRaw(" ");

                Write(text, ch => ch == CodeDelimiterChar, CodeDelimiterChar);

                if (text[text.Length - 1] == CodeDelimiterChar)
                    WriteRaw(" ");
            }

            WriteRaw(CodeDelimiter);
            return this;
        }

        public MarkdownWriter WriteHeadingStart(int level)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            PushCheck(MarkdownKind.Heading);

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeHeading);

            if (!underline)
            {
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
                WriteRaw(" ");
            }

            return this;
        }

        public MarkdownWriter WriteHeadingEnd()
        {
            int level = _headingLevel;
            _headingLevel = -1;

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            if (!underline
                && Format.CloseHeading)
            {
                WriteRaw(" ");
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
            }

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? '=' : '-', Length - _headingPosition);
                _headingPosition = -1;
                WriteLine();
            }

            PendingLineIf(Format.EmptyLineAfterHeading);
            return this;
        }

        public MarkdownWriter WriteHeading1(string text)
        {
            return WriteHeading(1, text);
        }

        public MarkdownWriter WriteHeading2(string text)
        {
            return WriteHeading(2, text);
        }

        public MarkdownWriter WriteHeading3(string text)
        {
            return WriteHeading(3, text);
        }

        public MarkdownWriter WriteHeading4(string text)
        {
            return WriteHeading(4, text);
        }

        public MarkdownWriter WriteHeading5(string text)
        {
            return WriteHeading(5, text);
        }

        public MarkdownWriter WriteHeading6(string text)
        {
            return WriteHeading(6, text);
        }

        public MarkdownWriter WriteHeading(int level, string content)
        {
            WriteHeadingStart(level);
            Write(content);
            WriteHeadingEnd();
            return this;
        }

        public MarkdownWriter WriteBulletItemStart()
        {
            PushCheck(MarkdownKind.BulletItem);
            WriteLineIfNecessary();
            WriteRaw(BulletItemStart(Format.BulletListStyle));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteBulletItemEnd()
        {
            Pop(MarkdownKind.BulletItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteBulletItem(string text)
        {
            WriteBulletItemStart();
            Write(text);
            WriteBulletItemEnd();
            return this;
        }

        public MarkdownWriter WriteOrderedItemStart(int number)
        {
            Error.ThrowOnInvalidItemNumber(number);
            PushCheck(MarkdownKind.OrderedItem);
            WriteLineIfNecessary();
            WriteValue(number);
            WriteRaw(OrderedItemStart(Format.OrderedListStyle));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteOrderedItemEnd()
        {
            Pop(MarkdownKind.OrderedItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteOrderedItem(int number, string text)
        {
            Error.ThrowOnInvalidItemNumber(number);

            WriteOrderedItemStart(number);
            Write(text);
            WriteOrderedItemEnd();
            return this;
        }

        public MarkdownWriter WriteTaskItemStart(bool isCompleted = false)
        {
            PushCheck(MarkdownKind.TaskItem);
            WriteLineIfNecessary();
            WriteRaw(TaskItemStart(isCompleted: isCompleted));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteCompletedTaskItemStart()
        {
            return WriteTaskItemStart(isCompleted: true);
        }

        public MarkdownWriter WriteTaskItemEnd()
        {
            Pop(MarkdownKind.TaskItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteTaskItem(string text)
        {
            WriteTaskItemStart();
            Write(text);
            WriteTaskItemEnd();
            return this;
        }

        public MarkdownWriter WriteCompletedTaskItem(string text)
        {
            WriteCompletedTaskItemStart();
            Write(text);
            WriteTaskItemEnd();
            return this;
        }

        public MarkdownWriter WriteImage(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Image);
            WriteRaw("!");
            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Link);
            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLinkOrText(string text, string url = null, string title = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                WriteLink(text, url, title);
            }
            else
            {
                Write(text);
            }

            return this;
        }

        private MarkdownWriter WriteLinkCore(string text, string url, string title)
        {
            WriteSquareBrackets(text);
            WriteRaw("(");
            Write(url, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            WriteLinkTitle(title);
            WriteRaw(")");
            return this;
        }

        public MarkdownWriter WriteAutolink(string url)
        {
            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Autolink);
            WriteAngleBrackets(url);
            return this;
        }

        public MarkdownWriter WriteImageReference(string text, string label)
        {
            Check(MarkdownKind.ImageReference);
            WriteRaw("!");
            WriteLinkReferenceCore(text, label);
            return this;
        }

        public MarkdownWriter WriteLinkReference(string text, string label = null)
        {
            Check(MarkdownKind.LinkReference);
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

            Check(MarkdownKind.Label);
            WriteLineIfNecessary();
            WriteSquareBrackets(label);
            WriteRaw(": ");
            WriteAngleBrackets(url);
            WriteLinkTitle(title);
            WriteLineIfNecessary();
            return this;
        }

        private void WriteLinkTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                WriteRaw(" ");

                WriteRaw("\"");
                Write(title, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                WriteRaw("\"");
            }
        }

        private void WriteSquareBrackets(string text)
        {
            WriteRaw("[");
            Write(text, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkText);
            WriteRaw("]");
        }

        private void WriteAngleBrackets(string text)
        {
            WriteRaw("<");
            Write(text, shouldBeEscaped: ch => ch == '<' || ch == '>');
            WriteRaw(">");
        }

        public MarkdownWriter WriteIndentedCodeBlock(string text)
        {
            PushCheck(MarkdownKind.IndentedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);
            WriteRaw(text);
            WriteLineIfNecessary();
            Pop(MarkdownKind.IndentedCodeBlock);
            PendingLineIf(Format.EmptyLineAfterCodeBlock);

            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            Check(MarkdownKind.FencedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);

            WriteCodeFence();
            WriteRaw(info);
            WriteLine();

            WriteRaw(text);
            WriteLineIfNecessary();

            WriteCodeFence();
            WriteLine();

            PendingLineIf(Format.EmptyLineAfterCodeBlock);

            return this;

            MarkdownWriter WriteCodeFence()
            {
                switch (Format.CodeFenceStyle)
                {
                    case CodeFenceStyle.Backtick:
                        return WriteRaw("```");
                    case CodeFenceStyle.Tilde:
                        return WriteRaw("~~~");
                    default:
                        throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CodeFenceStyle));
                }
            }
        }

        public void WriteBlockQuoteStart()
        {
            PushCheck(MarkdownKind.BlockQuote);
            WriteLineIfNecessary();
            QuoteLevel++;
        }

        public void WriteBlockQuoteEnd()
        {
            WriteLineIfNecessary();
            QuoteLevel--;
            Pop(MarkdownKind.BlockQuote);
        }

        public MarkdownWriter WriteBlockQuote(string text)
        {
            QuoteLevel++;
            Write(text);
            WriteLineIfNecessary();
            QuoteLevel--;
            return this;
        }

        public MarkdownWriter WriteHorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            Error.ThrowOnInvalidHorizontalRuleCount(count);

            Check(MarkdownKind.HorizontalRule);

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

        public MarkdownWriter WriteTableStart(IReadOnlyList<TableColumnInfo> columns)
        {
            PushCheck(MarkdownKind.Table);
            WriteLineIfNecessary();
            PendingLineIf(Format.EmptyLineBeforeTable);
            _tableColumns = columns;
            return this;
        }

        public MarkdownWriter WriteTableEnd()
        {
            _tableRowIndex = -1;
            _tableColumns = null;
            PendingLineIf(Format.EmptyLineAfterTable);
            Pop(MarkdownKind.Table);
            return this;
        }

        protected internal abstract IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows);

        internal MarkdownWriter WriteTableRow(MElement content)
        {
            WriteTableRowStart();

            if (content is MContainer container)
            {
                foreach (MElement element in container.Elements())
                    WriteCell(element);
            }
            else
            {
                WriteCell(content);
            }

            WriteTableRowEnd();
            return this;
        }

        public void WriteTableRowStart()
        {
            PushCheck(MarkdownKind.TableRow);
            _tableRowIndex++;
        }

        public void WriteTableRowEnd()
        {
            if (Format.TableOuterDelimiter
                || (_tableRowIndex == 0 && CurrentColumn.IsWhiteSpace))
            {
                WriteTableColumnSeparator();
            }

            WriteLine();
            _tableColumnIndex = -1;

            if (_tableRowIndex == 0)
                WriteTableHeaderSeparator();

            Pop(MarkdownKind.TableRow);
        }

        internal void WriteCell(MElement cell)
        {
            WriteTableCellStart();
            Write(cell);
            WriteTableCellEnd();
        }

        public void WriteTableCellStart()
        {
            PushCheck(MarkdownKind.TableColumn);
            _tableColumnIndex++;

            if (IsFirstColumn)
            {
                if (Format.TableOuterDelimiter
                    || IsLastColumn
                    || CurrentColumn.IsWhiteSpace)
                {
                    WriteTableColumnSeparator();
                }
            }
            else
            {
                WriteTableColumnSeparator();
            }

            if (_tableRowIndex == 0)
            {
                if (Format.TablePadding)
                {
                    WriteRaw(" ");
                }
                else if (Format.FormatTableHeader
                     && CurrentColumn.Alignment == Alignment.Center)
                {
                    WriteRaw(" ");
                }
            }
            else if (Format.TablePadding)
            {
                WriteRaw(" ");
            }

            _tableCellPosition = Length;
        }

        public void WriteTableCellEnd()
        {
            if (!IsLastColumn
                || Format.TableOuterDelimiter)
            {
                if (_tableRowIndex == 0)
                {
                    if (Format.FormatTableHeader)
                        WritePadRight(Length - _tableCellPosition);
                }
                else if (Format.FormatTableContent)
                {
                    WritePadRight(Length - _tableCellPosition);
                }
            }

            if (_tableRowIndex == 0)
            {
                if (Format.TablePadding)
                {
                    if (!CurrentColumn.IsWhiteSpace)
                        WriteRaw(" ");
                }
                else if (Format.FormatTableHeader
                     && CurrentColumn.Alignment != Alignment.Left)
                {
                    WriteRaw(" ");
                }
            }
            else if (Format.TablePadding)
            {
                if (Length - _tableCellPosition > 0)
                    WriteRaw(" ");
            }

            _tableCellPosition = -1;
            Pop(MarkdownKind.TableColumn);
        }

        public void WriteTableHeaderSeparator()
        {
            WriteLineIfNecessary();

            WriteTableRowStart();

            int count = _tableColumns.Count;

            for (int i = 0; i < count; i++)
            {
                _tableColumnIndex = i;

                if (IsFirstColumn)
                {
                    if (Format.TableOuterDelimiter
                        || IsLastColumn
                        || CurrentColumn.IsWhiteSpace)
                    {
                        WriteTableColumnSeparator();
                    }
                }
                else
                {
                    WriteTableColumnSeparator();
                }

                if (CurrentColumn.Alignment == Alignment.Center)
                {
                    WriteRaw(":");
                }
                else if (Format.TablePadding)
                {
                    WriteRaw(" ");
                }

                WriteRaw("---");

                if (Format.FormatTableHeader)
                    WritePadRight(3, '-');

                if (CurrentColumn.Alignment != Alignment.Left)
                {
                    WriteRaw(":");
                }
                else if (Format.TablePadding)
                {
                    WriteRaw(" ");
                }
            }

            WriteTableRowEnd();
        }

        private MarkdownWriter WriteTableColumnSeparator()
        {
            return WriteRaw(TableDelimiter);
        }

        private void WritePadRight(int width, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(CurrentColumn.Width, Math.Max(width, 3));

            WriteRaw(paddingChar, totalWidth - width);
        }

        public MarkdownWriter WriteCharReference(int number)
        {
            Check(MarkdownKind.CharReference);
            WriteRaw("&#");

            if (Format.CharReferenceFormat == CharReferenceFormat.Hexadecimal)
            {
                WriteRaw("x");
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

            WriteRaw(";");
            return this;
        }

        public MarkdownWriter WriteEntityReference(string name)
        {
            Check(MarkdownKind.EntityReference);
            WriteRaw("&");
            WriteRaw(name);
            WriteRaw(";");
            return this;
        }

        public MarkdownWriter WriteComment(string text)
        {
            PushCheck(MarkdownKind.Comment);
            WriteRaw("<!-- ");
            WriteRaw(text);
            WriteRaw(" -->");
            Pop(MarkdownKind.Comment);
            return this;
        }

        internal MarkdownWriter Write(object value)
        {
            if (value == null)
                return this;

            if (value is MElement element)
                return element.WriteTo(this);

            if (value is string s)
                return Write(s);

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

            return Write(value.ToString());
        }

        public MarkdownWriter Write(string value)
        {
            return Write(value, MarkdownEscaper.ShouldBeEscaped);
        }

        internal MarkdownWriter Write(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            if (value == null)
                return this;

            OnBeforeWrite();

            int length = value.Length;

            int lastIndex = 0;

            int i = 0;
            while (i < length)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    WriteLine(i);
                    lastIndex = ++i;
                }
                else if (ch == 13
                    && (i == length - 1 || value[i + 1] != 10))
                {
                    WriteLine(i);
                    lastIndex = ++i;
                }
                else if (shouldBeEscaped(ch))
                {
                    OnBeforeWrite();
                    WriteString(value, lastIndex, i - lastIndex);
                    WriteValue(escapingChar);
                    WriteValue(ch);
                    lastIndex = ++i;
                }
                else
                {
                    i++;

                }
            }

            OnBeforeWrite();
            WriteString(value, lastIndex, value.Length - lastIndex);
            return this;

            void WriteLine(int index)
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

                OnBeforeWrite();
                WriteString(value, lastIndex, index - lastIndex);

                if (Settings.NewLineHandling == NewLineHandling.Replace)
                {
                    this.WriteLine();
                }
                else
                {
                    OnAfterWriteLine();
                }
            }
        }

        public MarkdownWriter WriteLine(string value)
        {
            Write(value);
            WriteLine();
            return this;
        }

        internal void WriteLineIfNecessary()
        {
            if (!_startOfLine)
                WriteLine();
        }

        private void PendingLineIf(bool condition)
        {
            if (condition)
                _pendingEmptyLine = true;
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            if (Length > 0
                && !_emptyLine)
            {
                WriteLine();
            }
        }

        private MarkdownWriter WriteRaw(char value)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            OnBeforeWrite();
            WriteValue(value);
            return this;
        }

        private MarkdownWriter WriteRaw(char value, int repeatCount)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            OnBeforeWrite();

            for (int i = 0; i < repeatCount; i++)
                WriteValue(value);

            return this;
        }

        public MarkdownWriter WriteRaw(string value)
        {
            Write(value, _ => false);
            return this;
        }

        public virtual MarkdownWriter WriteLine()
        {
            WriteString(Settings.NewLineChars);
            OnAfterWriteLine();
            return this;
        }

        private void OnAfterWriteLine()
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

        private void OnBeforeWrite()
        {
            if (_pendingEmptyLine)
            {
                WriteIndentation();
                WriteLine();
            }
            else if (_startOfLine)
            {
                WriteIndentation();
                _startOfLine = false;
                _emptyLine = false;
            }
        }

        private void WriteIndentation()
        {
            if (CurrentKind == MarkdownKind.Comment)
                return;

            for (int i = 0; i < QuoteLevel; i++)
                WriteString(BlockQuoteStart);

            for (int i = 0; i < ListLevel; i++)
                WriteString("  ");

            if (CurrentKind == MarkdownKind.IndentedCodeBlock)
                WriteString("    ");
        }

        public abstract void Flush();

        protected abstract void WriteString(string value);

        public abstract void WriteString(string value, int startIndex, int count);

        protected abstract void WriteValue(char value);

        protected virtual void WriteValue(string value)
        {
            if (value == null)
                return;

            WriteString(value);
        }

        public virtual void WriteValue(bool value)
        {
            WriteString((value) ? "true" : "false");
        }

        public virtual void WriteValue(int value)
        {
            WriteString(value.ToString(null, CultureInfo.InvariantCulture));
        }

        public virtual void WriteValue(long value)
        {
            WriteString(value.ToString(null, CultureInfo.InvariantCulture));
        }

        public virtual void WriteValue(float value)
        {
            WriteString(value.ToString(null, CultureInfo.InvariantCulture));
        }

        public virtual void WriteValue(double value)
        {
            WriteString(value.ToString(null, CultureInfo.InvariantCulture));
        }

        public virtual void WriteValue(decimal value)
        {
            WriteString(value.ToString(null, CultureInfo.InvariantCulture));
        }
    }
}