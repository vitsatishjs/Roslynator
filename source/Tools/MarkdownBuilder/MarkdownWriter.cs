// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;
        private bool _indentedCodeBlock;

        private int _lineStartPos;
        private int _emptyLineStartPos = -1;

        private int _headingLevel = -1;

        private IReadOnlyList<TableColumnInfo> _tableColumns;
        private int _tableColumnCount = -1;
        private int _tableRowIndex = -1;
        private int _tableColumnIndex = -1;
        private int _tableCellPos = -1;

        protected State _state;

        private readonly Stack<MarkdownKind> _stack = new Stack<MarkdownKind>();

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
        }

        public virtual WriteState WriteState
        {
            get
            {
                switch (_state)
                {
                    case State.Start:
                        return WriteState.Start;
                    case State.Content:
                        return WriteState.Content;
                    case State.Closed:
                        return WriteState.Closed;
                    case State.Error:
                        return WriteState.Error;
                    default:
                        throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(_state));
                }
            }
        }

        public virtual MarkdownWriterSettings Settings { get; }

        public MarkdownFormat Format => Settings.Format;

        internal NewLineHandling NewLineHandling => Settings.NewLineHandling;

        internal string NewLineChars => Settings.NewLineChars;

        public int QuoteLevel { get; private set; }

        public int ListLevel { get; private set; }

        protected internal abstract int Length { get; set; }

        protected Func<char, bool> ShouldBeEscaped { get; set; } = MarkdownEscaper.ShouldBeEscaped;

        protected char EscapingChar { get; set; } = '\\';

        private TableColumnInfo CurrentColumn => _tableColumns?[_tableColumnIndex] ?? TableColumnInfo.Default;

        private int ColumnCount => _tableColumns?.Count ?? _tableColumnCount;

        private bool IsLastColumn => _tableColumnIndex == ColumnCount - 1;

        private bool IsFirstColumn => _tableColumnIndex == 0;

        public static MarkdownWriter Create(StringBuilder output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownStringWriter(output, CultureInfo.InvariantCulture, settings);
        }

        public static MarkdownWriter Create(TextWriter output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownTextWriter(output, settings);
        }

        public static MarkdownWriter Create(Stream stream, Encoding encoding = null, MarkdownWriterSettings settings = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            return new MarkdownTextWriter(new StreamWriter(stream, encoding ?? Encoding.UTF8), settings);
        }

        private void Push(MarkdownKind kind)
        {
            Check(kind);
            _stack.Push(kind);
        }

        protected void Check(MarkdownKind _)
        {
            try
            {
                if (_state == State.Closed)
                    throw new InvalidOperationException("Cannot write to a closed writer.");

                if (_state == State.Error)
                    throw new InvalidOperationException("Cannot write to a writer in error state.");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        private void Pop(MarkdownKind kind)
        {
            if (_stack.Count == 0)
            {
                //TODO: throw
            }

            if (_stack.Pop() != kind)
            {
                //TODO: throw
            }
        }

        public void WriteStartBold()
        {
            try
            {
                Push(MarkdownKind.Bold);
                WriteRaw(Format.BoldDelimiter);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndBold()
        {
            try
            {
                WriteRaw(Format.BoldDelimiter);
                Pop(MarkdownKind.Bold);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteBold(string text)
        {
            try
            {
                WriteStartBold();
                WriteString(text);
                WriteEndBold();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartItalic()
        {
            try
            {
                Push(MarkdownKind.Italic);
                WriteRaw(Format.ItalicDelimiter);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndItalic()
        {
            try
            {
                WriteRaw(Format.ItalicDelimiter);
                Pop(MarkdownKind.Italic);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteItalic(string text)
        {
            try
            {
                WriteStartItalic();
                WriteString(text);
                WriteEndItalic();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartStrikethrough()
        {
            try
            {
                Push(MarkdownKind.Strikethrough);
                WriteRaw("~~");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndStrikethrough()
        {
            try
            {
                WriteRaw("~~");
                Pop(MarkdownKind.Strikethrough);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStrikethrough(string text)
        {
            try
            {
                WriteStartStrikethrough();
                WriteString(text);
                WriteEndStrikethrough();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteInlineCode(string text)
        {
            try
            {
                Push(MarkdownKind.InlineCode);
                WriteRaw("`");

                if (!string.IsNullOrEmpty(text))
                {
                    if (text[0] == '`')
                        WriteRaw(" ");

                    WriteString(text, MarkdownEscaper.ShouldBeEscapedInInlineCode, '`');

                    if (text[text.Length - 1] == '`')
                        WriteRaw(" ");
                }

                WriteRaw("`");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartHeading(int level)
        {
            try
            {
                Error.ThrowOnInvalidHeadingLevel(level);

                Push(MarkdownKind.Heading);

                _headingLevel = level;

                bool underline = (level == 1 && Format.UnderlineHeading1)
                    || (level == 2 && Format.UnderlineHeading2);

                WriteLine(Format.EmptyLineBeforeHeading);

                if (!underline)
                {
                    WriteRaw(Format.HeadingStart, level);
                    WriteRaw(" ");
                }
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndHeading()
        {
            try
            {
                int level = _headingLevel;
                _headingLevel = -1;

                bool underline = (level == 1 && Format.UnderlineHeading1)
                    || (level == 2 && Format.UnderlineHeading2);

                if (!underline
                    && Format.CloseHeading)
                {
                    WriteRaw(" ");
                    WriteRaw(Format.HeadingStart, level);
                }

                int length = Length - _lineStartPos;

                WriteLineIfNecessary();

                if (underline)
                {
                    WriteRaw((level == 1) ? "=" : "-", length);
                    WriteLine();
                }

                WriteEmptyLineIf(Format.EmptyLineAfterHeading);
                Pop(MarkdownKind.Heading);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteHeading1(string text)
        {
            WriteHeading(1, text);
        }

        public void WriteHeading2(string text)
        {
            WriteHeading(2, text);
        }

        public void WriteHeading3(string text)
        {
            WriteHeading(3, text);
        }

        public void WriteHeading4(string text)
        {
            WriteHeading(4, text);
        }

        public void WriteHeading5(string text)
        {
            WriteHeading(5, text);
        }

        public void WriteHeading6(string text)
        {
            WriteHeading(6, text);
        }

        public void WriteHeading(int level, string text)
        {
            try
            {
                WriteStartHeading(level);
                WriteString(text);
                WriteEndHeading();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartBulletItem()
        {
            try
            {
                Push(MarkdownKind.BulletItem);
                WriteLineIfNecessary();
                WriteRaw(Format.BulletItemStart);
                ListLevel++;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndBulletItem()
        {
            try
            {
                Pop(MarkdownKind.BulletItem);
                ListLevel--;
                WriteLine();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteBulletItem(string text)
        {
            try
            {
                WriteStartBulletItem();
                WriteString(text);
                WriteEndBulletItem();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartOrderedItem(int number)
        {
            try
            {
                Error.ThrowOnInvalidItemNumber(number);
                Push(MarkdownKind.OrderedItem);
                WriteLineIfNecessary();
                WriteValue(number);
                WriteRaw(Format.OrderedItemStart);
                ListLevel++;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndOrderedItem()
        {
            try
            {
                Pop(MarkdownKind.OrderedItem);
                ListLevel--;
                WriteLineIfNecessary();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteOrderedItem(int number, string text)
        {
            try
            {
                Error.ThrowOnInvalidItemNumber(number);
                WriteStartOrderedItem(number);
                WriteString(text);
                WriteEndOrderedItem();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartTaskItem(bool isCompleted = false)
        {
            try
            {
                Push(MarkdownKind.TaskItem);
                WriteLineIfNecessary();

                if (isCompleted)
                {
                    WriteRaw("- [x] ");
                }
                else
                {
                    WriteRaw("- [ ] ");
                }

                ListLevel++;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartCompletedTaskItem()
        {
            WriteStartTaskItem(isCompleted: true);
        }

        public void WriteEndTaskItem()
        {
            try
            {
                Pop(MarkdownKind.TaskItem);
                ListLevel--;
                WriteLineIfNecessary();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteTaskItem(string text)
        {
            try
            {
                WriteStartTaskItem();
                WriteString(text);
                WriteEndTaskItem();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteCompletedTaskItem(string text)
        {
            try
            {
                WriteStartCompletedTaskItem();
                WriteString(text);
                WriteEndTaskItem();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteImage(string text, string url, string title = null)
        {
            try
            {
                if (text == null)
                    throw new ArgumentNullException(nameof(text));

                Error.ThrowOnInvalidUrl(url);

                Push(MarkdownKind.Image);
                WriteRaw("!");
                WriteLinkCore(text, url, title);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteLink(string text, string url, string title = null)
        {
            try
            {
                if (text == null)
                    throw new ArgumentNullException(nameof(text));

                Error.ThrowOnInvalidUrl(url);

                Push(MarkdownKind.Link);
                WriteLinkCore(text, url, title);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteLinkOrText(string text, string url = null, string title = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                WriteLink(text, url, title);
            }
            else
            {
                WriteString(text);
            }
        }

        private void WriteLinkCore(string text, string url, string title)
        {
            WriteSquareBrackets(text);
            WriteRaw("(");
            WriteString(url, MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            WriteLinkTitle(title);
            WriteRaw(")");
        }

        public void WriteAutolink(string url)
        {
            try
            {
                Error.ThrowOnInvalidUrl(url);
                Push(MarkdownKind.Autolink);
                WriteAngleBrackets(url);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteImageReference(string text, string label)
        {
            try
            {
                Push(MarkdownKind.ImageReference);
                WriteRaw("!");
                WriteLinkReferenceCore(text, label);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteLinkReference(string text, string label = null)
        {
            try
            {
                Push(MarkdownKind.LinkReference);
                WriteLinkReferenceCore(text, label);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        private void WriteLinkReferenceCore(string text, string label = null)
        {
            WriteSquareBrackets(text);
            WriteSquareBrackets(label);
        }

        public void WriteLabel(string label, string url, string title = null)
        {
            try
            {
                Error.ThrowOnInvalidUrl(url);

                Push(MarkdownKind.Label);
                WriteLineIfNecessary();
                WriteSquareBrackets(label);
                WriteRaw(": ");
                WriteAngleBrackets(url);
                WriteLinkTitle(title);
                WriteLineIfNecessary();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        private void WriteLinkTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return;

            WriteRaw(" ");
            WriteRaw("\"");
            WriteString(title, MarkdownEscaper.ShouldBeEscapedInLinkTitle);
            WriteRaw("\"");
        }

        private void WriteSquareBrackets(string text)
        {
            WriteRaw("[");
            WriteString(text, MarkdownEscaper.ShouldBeEscapedInLinkText);
            WriteRaw("]");
        }

        private void WriteAngleBrackets(string text)
        {
            WriteRaw("<");
            WriteString(text, MarkdownEscaper.ShouldBeEscapedInAngleBrackets);
            WriteRaw(">");
        }

        public void WriteIndentedCodeBlock(string text)
        {
            try
            {
                Push(MarkdownKind.IndentedCodeBlock);

                WriteLine(Format.EmptyLineBeforeCodeBlock);

                _indentedCodeBlock = true;
                WriteString(text, _ => false);
                _indentedCodeBlock = false;
                WriteLine();
                WriteEmptyLineIf(Format.EmptyLineAfterCodeBlock);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteFencedCodeBlock(string text, string info = null)
        {
            try
            {
                Error.ThrowOnInvalidFencedCodeBlockInfo(info);
                Push(MarkdownKind.FencedCodeBlock);

                WriteLine(Format.EmptyLineBeforeCodeBlock);

                WriteRaw(Format.CodeFence);
                WriteRaw(info);
                WriteLine();
                WriteString(text, _ => false);

                WriteLineIfNecessary();

                WriteRaw(Format.CodeFence);

                WriteLine();
                WriteEmptyLineIf(Format.EmptyLineAfterCodeBlock);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartBlockQuote()
        {
            try
            {
                Push(MarkdownKind.BlockQuote);
                WriteLineIfNecessary();
                QuoteLevel++;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndBlockQuote()
        {
            try
            {
                WriteLineIfNecessary();
                QuoteLevel--;
                Pop(MarkdownKind.BlockQuote);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteBlockQuote(string text)
        {
            try
            {
                QuoteLevel++;
                WriteString(text);
                WriteLineIfNecessary();
                QuoteLevel--;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteHorizontalRule()
        {
            WriteHorizontalRule(Format.HorizontalRuleFormat);
        }

        public void WriteHorizontalRule(HorizontalRuleFormat format)
        {
            WriteHorizontalRule(format.Text, format.Count, format.Separator);
        }

        public void WriteHorizontalRule(string text, int count = HorizontalRuleFormat.DefaultCount, string separator = HorizontalRuleFormat.DefaultSeparator)
        {
            try
            {
                Error.ThrowOnInvalidHorizontalRuleText(text);
                Error.ThrowOnInvalidHorizontalRuleCount(count);
                Error.ThrowOnInvalidHorizontalRuleSeparator(separator);

                Push(MarkdownKind.HorizontalRule);

                WriteLineIfNecessary();

                bool isFirst = true;

                for (int i = 0; i < count; i++)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        WriteRaw(separator);
                    }

                    WriteRaw(text);
                }

                WriteLine();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartTable(int columnCount)
        {
            WriteStartTable(null, columnCount);
        }

        public void WriteStartTable(IReadOnlyList<TableColumnInfo> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            WriteStartTable(columns, columns.Count);
        }

        private void WriteStartTable(IReadOnlyList<TableColumnInfo> columns, int columnCount)
        {
            try
            {
                Push(MarkdownKind.Table);

                WriteLine(Format.EmptyLineBeforeTable);

                _tableColumns = columns;
                _tableColumnCount = columnCount;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndTable()
        {
            try
            {
                _tableRowIndex = -1;
                _tableColumns = null;
                _tableColumnCount = -1;
                WriteEmptyLineIf(Format.EmptyLineAfterTable);
                Pop(MarkdownKind.Table);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        internal void WriteTableRow(MElement content)
        {
            try
            {
                WriteStartTableRow();

                if (content is MContainer container)
                {
                    foreach (MElement element in container.Elements())
                        WriteTableCell(element);
                }
                else
                {
                    WriteTableCell(content);
                }

                WriteEndTableRow();
            }
            catch
            {
                _state = State.Error;
                throw;
            }

            void WriteTableCell(MElement cell)
            {
                try
                {
                    WriteStartTableCell();
                    Write(cell);
                    WriteEndTableCell();
                }
                catch
                {
                    _state = State.Error;
                    throw;
                }
            }
        }

        public void WriteStartTableRow()
        {
            try
            {
                Push(MarkdownKind.TableRow);
                _tableRowIndex++;
                _tableColumnIndex = -1;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndTableRow()
        {
            try
            {
                if (Format.TableOuterDelimiter
                    || (_tableRowIndex == 0 && CurrentColumn.IsWhiteSpace))
                {
                    WriteTableColumnSeparator();
                }

                WriteLine();
                _tableColumnIndex = -1;

                Pop(MarkdownKind.TableRow);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteStartTableCell()
        {
            try
            {
                //TODO: stack top is table row

                Push(MarkdownKind.Inline);

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

                _tableCellPos = Length;
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEndTableCell()
        {
            try
            {
                if (Format.TableOuterDelimiter
                    || !IsLastColumn)
                {
                    if (_tableRowIndex == 0)
                    {
                        if (Format.FormatTableHeader)
                            WritePadRight(Length - _tableCellPos);
                    }
                    else if (Format.FormatTableContent)
                    {
                        WritePadRight(Length - _tableCellPos);
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
                    if (Length - _tableCellPos > 0)
                        WriteRaw(" ");
                }

                _tableCellPos = -1;
                Pop(MarkdownKind.Inline);
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteTableHeaderSeparator()
        {
            try
            {
                WriteLineIfNecessary();

                WriteStartTableRow();

                int count = ColumnCount;

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
                        WritePadRight(3, "-");

                    if (CurrentColumn.Alignment != Alignment.Left)
                    {
                        WriteRaw(":");
                    }
                    else if (Format.TablePadding)
                    {
                        WriteRaw(" ");
                    }
                }

                WriteEndTableRow();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        private void WriteTableColumnSeparator()
        {
            WriteRaw("|");
        }

        private void WritePadRight(int width, string padding = " ")
        {
            int totalWidth = Math.Max(CurrentColumn.Width, Math.Max(width, 3));

            WriteRaw(padding, totalWidth - width);
        }

        public void WriteCharEntity(char value)
        {
            try
            {
                Error.ThrowOnInvalidCharEntity(value);

                Push(MarkdownKind.CharEntity);
                WriteRaw("&#");

                if (Format.CharEntityFormat == CharEntityFormat.Hexadecimal)
                {
                    WriteRaw("x");
                    WriteRaw(((int)value).ToString("x", CultureInfo.InvariantCulture));
                }
                else if (Format.CharEntityFormat == CharEntityFormat.Decimal)
                {
                    WriteRaw(((int)value).ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CharEntityFormat));
                }

                WriteRaw(";");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteEntityRef(string name)
        {
            try
            {
                Push(MarkdownKind.EntityRef);
                WriteRaw("&");
                WriteRaw(name);
                WriteRaw(";");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        public void WriteComment(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.IndexOf("--", StringComparison.Ordinal) >= 0)
                        throw new ArgumentException("XML comment text cannot contain '--'.");

                    if (text[text.Length - 1] == '-')
                        throw new ArgumentException("Last character of XML comment text cannot be '-'.");
                }

                Push(MarkdownKind.Comment);
                WriteRaw("<!-- ");
                WriteRaw(text);
                WriteRaw(" -->");
            }
            catch
            {
                _state = State.Error;
                throw;
            }
        }

        internal void Write(object value)
        {
            if (value == null)
                return;

            if (value is MElement element)
            {
                element.WriteTo(this);
                return;
            }

            if (value is string s)
            {
                WriteString(s);
                return;
            }

            if (value is object[] arr)
            {
                foreach (object item in arr)
                    Write(item);

                return;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    Write(item);

                return;
            }

            WriteString(value.ToString());
        }

        public abstract void Flush();

        public abstract void WriteString(string text);

        private void WriteString(string text, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            ShouldBeEscaped = shouldBeEscaped;
            EscapingChar = escapingChar;
            WriteString(text);
            EscapingChar = '\\';
            ShouldBeEscaped = MarkdownEscaper.ShouldBeEscaped;
        }

        public abstract void WriteRaw(string data);

        private void WriteRaw(string data, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                WriteRaw(data);
        }

        public virtual void WriteLine()
        {
            try
            {
                WriteRaw(NewLineChars);
                OnAfterWriteLine();
            }
            catch
            {
                _state = State.Error;
                throw;
            }
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

        protected void WriteIndentation()
        {
            for (int i = 0; i < QuoteLevel; i++)
                WriteRaw("> ");

            for (int i = 0; i < ListLevel; i++)
                WriteRaw("  ");

            if (_indentedCodeBlock)
                WriteRaw("    ");
        }

        protected void OnBeforeWriteLine()
        {
            if (_lineStartPos == Length)
            {
                _emptyLineStartPos = _lineStartPos;
            }
            else
            {
                _emptyLineStartPos = -1;
            }
        }

        protected void OnAfterWriteLine()
        {
            WriteIndentation();

            if (_emptyLineStartPos == _lineStartPos)
                _emptyLineStartPos = Length;

            _lineStartPos = Length;
        }

        internal void WriteLineIfNecessary()
        {
            if (_lineStartPos != Length)
                WriteLine();
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition
                && Length > 0)
            {
                WriteLine();
            }
        }

        private void WriteLine(bool addEmptyLine)
        {
            if (_emptyLineStartPos != Length)
            {
                if (_lineStartPos != Length)
                    WriteLine();

                WriteEmptyLineIf(addEmptyLine);
            }
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
                if (disposing
                    && WriteState != WriteState.Closed)
                {
                    Close();
                }

                _disposed = true;
            }
        }

        public virtual void Close()
        {
            Dispose();
        }

        protected enum State
        {
            Start = 0,
            Content = 1,
            Closed = 2,
            Error = 3
        }
    }
}