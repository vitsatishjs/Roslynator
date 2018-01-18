// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Pihrtsoft.Markdown.Linq;

#pragma warning disable CA1814

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;

        protected bool _startOfLine;
        private bool _emptyLine;
        private bool _pendingEmptyLine;

        private int _headingPosition = -1;
        private int _headingLevel = -1;

        private IReadOnlyList<TableColumnInfo> _tableColumns;
        private int _tableRowIndex = -1;
        private int _tableColumnIndex = -1;
        private int _tableCellPosition = -1;

        private State _state;
        private readonly Stack<MarkdownKind> _containers = new Stack<MarkdownKind>();

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
            _startOfLine = true;
        }

        public virtual MarkdownWriterSettings Settings { get; }

        public MarkdownFormat Format => Settings.Format;

        internal NewLineHandling NewLineHandling => Settings.NewLineHandling;

        private MarkdownKind CurrentKind
        {
            get { return (_containers.Count > 0) ? _containers.Peek() : MarkdownKind.None; }
        }

        public int QuoteLevel { get; private set; }

        internal int ListLevel { get; private set; }

        protected internal abstract int Length { get; set; }

        protected Func<char, bool> ShouldBeEscaped { get; set; } = MarkdownEscaper.ShouldBeEscaped;

        protected char EscapingChar => (_state == State.InlineCode) ? '`' : '\\';

        private TableColumnInfo CurrentColumn => _tableColumns[_tableColumnIndex];

        private bool IsLastColumn => _tableColumnIndex == _tableColumns.Count - 1;

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

        private void CheckPush(MarkdownKind kind)
        {
            Check(kind);
            _containers.Push(kind);
        }

        private void Pop()
        {
            _containers.Pop();
        }

        internal void Check(MarkdownKind kind)
        {
        }

        private void ChangeState(State state)
        {
            _state = state;

            switch (state)
            {
                case State.None:
                    {
                        ShouldBeEscaped = MarkdownEscaper.ShouldBeEscaped;
                        break;
                    }
                case State.Raw:
                    {
                        ShouldBeEscaped = _ => false;
                        break;
                    }
                case State.InlineCode:
                    {
                        ShouldBeEscaped = f => f == '`';
                        break;
                    }
                case State.LinkText:
                    {
                        ShouldBeEscaped = MarkdownEscaper.ShouldBeEscapedInLinkText;
                        break;
                    }
                case State.LinkUrl:
                    {
                        ShouldBeEscaped = MarkdownEscaper.ShouldBeEscapedInLinkUrl;
                        break;
                    }
                case State.LinkTitle:
                    {
                        ShouldBeEscaped = MarkdownEscaper.ShouldBeEscapedInLinkTitle;
                        break;
                    }
                case State.AngleBrackets:
                    {
                        ShouldBeEscaped = ch => ch == '<' || ch == '>';
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(_state));
                    }
            }
        }

        public MarkdownWriter WriteStartBold()
        {
            CheckPush(MarkdownKind.Bold);
            WriteRaw(Format.BoldDelimiter);
            return this;
        }

        public MarkdownWriter WriteEndBold()
        {
            WriteRaw(Format.BoldDelimiter);
            Pop();
            return this;
        }

        public MarkdownWriter WriteBold(string text)
        {
            WriteStartBold();
            WriteString(text);
            WriteEndBold();
            return this;
        }

        public MarkdownWriter WriteStartItalic()
        {
            CheckPush(MarkdownKind.Italic);
            WriteRaw(Format.ItalicDelimiter);
            return this;
        }

        public MarkdownWriter WriteEndItalic()
        {
            WriteRaw(Format.ItalicDelimiter);
            Pop();
            return this;
        }

        public MarkdownWriter WriteItalic(string text)
        {
            WriteStartItalic();
            WriteString(text);
            WriteEndItalic();
            return this;
        }

        public MarkdownWriter WriteStartStrikethrough()
        {
            CheckPush(MarkdownKind.Strikethrough);
            WriteRaw("~~");
            return this;
        }

        public MarkdownWriter WriteEndStrikethrough()
        {
            WriteRaw("~~");
            Pop();
            return this;
        }

        public MarkdownWriter WriteStrikethrough(string text)
        {
            WriteStartStrikethrough();
            WriteString(text);
            WriteEndStrikethrough();
            return this;
        }

        public MarkdownWriter WriteInlineCode(string text)
        {
            Check(MarkdownKind.InlineCode);
            WriteRaw("`");
            ChangeState(State.InlineCode);

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '`')
                    WriteRaw(" ");

                WriteString(text);

                if (text[text.Length - 1] == '`')
                    WriteRaw(" ");
            }

            ChangeState(State.None);
            WriteRaw("`");
            return this;
        }

        public MarkdownWriter WriteStartHeading(int level)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            CheckPush(MarkdownKind.Heading);

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeHeading);

            if (!underline)
            {
                WriteRaw(Format.HeadingStart, level);
                WriteRaw(" ");
            }

            return this;
        }

        public MarkdownWriter WriteEndHeading()
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

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? "=" : "-", Length - _headingPosition);
                _headingPosition = -1;
                WriteLine();
            }

            PendingLineIf(Format.EmptyLineAfterHeading);
            Pop();
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

        public MarkdownWriter WriteHeading(int level, string text)
        {
            WriteStartHeading(level);
            WriteString(text);
            WriteEndHeading();
            return this;
        }

        public MarkdownWriter WriteStartBulletItem()
        {
            CheckPush(MarkdownKind.BulletItem);
            WriteLineIfNecessary();
            WriteRaw(Format.BulletItemStart);
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteEndBulletItem()
        {
            Pop();
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteBulletItem(string text)
        {
            WriteStartBulletItem();
            WriteString(text);
            WriteEndBulletItem();
            return this;
        }

        public MarkdownWriter WriteStartOrderedItem(int number)
        {
            Error.ThrowOnInvalidItemNumber(number);
            CheckPush(MarkdownKind.OrderedItem);
            WriteLineIfNecessary();
            WriteValue(number);
            WriteRaw(Format.OrderedItemStart);
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteEndOrderedItem()
        {
            Pop();
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteOrderedItem(int number, string text)
        {
            Error.ThrowOnInvalidItemNumber(number);

            WriteStartOrderedItem(number);
            WriteString(text);
            WriteEndOrderedItem();
            return this;
        }

        public MarkdownWriter WriteStartTaskItem(bool isCompleted = false)
        {
            CheckPush(MarkdownKind.TaskItem);
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
            return this;
        }

        public MarkdownWriter WriteStartCompletedTaskItem()
        {
            return WriteStartTaskItem(isCompleted: true);
        }

        public MarkdownWriter WriteEndTaskItem()
        {
            Pop();
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteTaskItem(string text)
        {
            WriteStartTaskItem();
            WriteString(text);
            WriteEndTaskItem();
            return this;
        }

        public MarkdownWriter WriteCompletedTaskItem(string text)
        {
            WriteStartCompletedTaskItem();
            WriteString(text);
            WriteEndTaskItem();
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
                WriteString(text);
            }

            return this;
        }

        private MarkdownWriter WriteLinkCore(string text, string url, string title)
        {
            WriteSquareBrackets(text);
            WriteRaw("(");
            ChangeState(State.LinkUrl);
            WriteString(url);
            ChangeState(State.None);
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
                ChangeState(State.LinkTitle);
                WriteString(title);
                ChangeState(State.None);
                WriteRaw("\"");
            }
        }

        private void WriteSquareBrackets(string text)
        {
            WriteRaw("[");
            ChangeState(State.LinkText);
            WriteString(text);
            ChangeState(State.None);
            WriteRaw("]");
        }

        private void WriteAngleBrackets(string text)
        {
            WriteRaw("<");
            ChangeState(State.AngleBrackets);
            WriteString(text);
            ChangeState(State.None);
            WriteRaw(">");
        }

        public MarkdownWriter WriteIndentedCodeBlock(string text)
        {
            CheckPush(MarkdownKind.IndentedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);
            ChangeState(State.Raw);
            WriteRaw(text);
            ChangeState(State.None);
            WriteLineIfNecessary();
            Pop();
            PendingLineIf(Format.EmptyLineAfterCodeBlock);

            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            Error.ThrowOnInvalidFencedCodeBlockInfo(info);

            Check(MarkdownKind.FencedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);

            WriteRaw(Format.CodeFence);
            WriteRaw(info);
            WriteLine();

            ChangeState(State.Raw);
            WriteRaw(text);
            ChangeState(State.None);

            WriteLineIfNecessary();

            WriteRaw(Format.CodeFence);
            WriteLine();

            PendingLineIf(Format.EmptyLineAfterCodeBlock);
            return this;
        }

        public void WriteStartBlockQuote()
        {
            CheckPush(MarkdownKind.BlockQuote);
            WriteLineIfNecessary();
            QuoteLevel++;
        }

        public void WriteEndBlockQuote()
        {
            WriteLineIfNecessary();
            QuoteLevel--;
            Pop();
        }

        public MarkdownWriter WriteBlockQuote(string text)
        {
            QuoteLevel++;
            WriteString(text);
            WriteLineIfNecessary();
            QuoteLevel--;
            return this;
        }

        public MarkdownWriter WriteHorizontalRule()
        {
            return WriteHorizontalRule(Format.HorizontalRuleFormat.Value, Format.HorizontalRuleFormat.Count, Format.HorizontalRuleFormat.Separator);
        }

        public MarkdownWriter WriteHorizontalRule(string value, int count = 3, string separator = " ")
        {
            Check(MarkdownKind.HorizontalRule);

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

                WriteRaw(value);
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteStartTable(IReadOnlyList<TableColumnInfo> columns)
        {
            CheckPush(MarkdownKind.Table);
            WriteLineIfNecessary();
            PendingLineIf(Format.EmptyLineBeforeTable);
            _tableColumns = columns;
            return this;
        }

        public MarkdownWriter WriteEndTable()
        {
            _tableRowIndex = -1;
            _tableColumns = null;
            PendingLineIf(Format.EmptyLineAfterTable);
            Pop();
            return this;
        }

        protected internal abstract IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows);

        internal MarkdownWriter WriteTableRow(MElement content)
        {
            WriteStartTableRow();

            if (content is MContainer container)
            {
                foreach (MElement element in container.Elements())
                    WriteCell(element);
            }
            else
            {
                WriteCell(content);
            }

            WriteEndTableRow();
            return this;
        }

        public void WriteStartTableRow()
        {
            CheckPush(MarkdownKind.TableRow);
            _tableRowIndex++;
            _tableColumnIndex = -1;
        }

        public void WriteEndTableRow()
        {
            if (Format.TableOuterDelimiter
                || (_tableRowIndex == 0 && CurrentColumn.IsWhiteSpace))
            {
                WriteTableColumnSeparator();
            }

            WriteLine();
            _tableColumnIndex = -1;

            Pop();
        }

        internal void WriteCell(MElement cell)
        {
            WriteStartTableCell();
            Write(cell);
            WriteEndTableCell();
        }

        public void WriteStartTableCell()
        {
            CheckPush(MarkdownKind.TableColumn);
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

        public void WriteEndTableCell()
        {
            if (Format.TableOuterDelimiter
                || !IsLastColumn)
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
            Pop();
        }

        public void WriteTableHeaderSeparator()
        {
            WriteLineIfNecessary();

            WriteStartTableRow();

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

        private MarkdownWriter WriteTableColumnSeparator()
        {
            return WriteRaw("|");
        }

        private void WritePadRight(int width, string padding = " ")
        {
            int totalWidth = Math.Max(CurrentColumn.Width, Math.Max(width, 3));

            WriteRaw(padding, totalWidth - width);
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
            Check(MarkdownKind.Comment);
            WriteRaw("<!-- ");
            WriteRaw(text);
            WriteRaw(" -->");
            return this;
        }

        internal MarkdownWriter Write(object value)
        {
            if (value == null)
                return this;

            if (value is MElement element)
                return element.WriteTo(this);

            if (value is string s)
                return WriteString(s);

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

            return WriteString(value.ToString());
        }

        public abstract void Flush();

        public abstract MarkdownWriter WriteString(string value);

        public abstract MarkdownWriter WriteRaw(string value);

        private MarkdownWriter WriteRaw(string value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                WriteRaw(value);

            return this;
        }

        public virtual MarkdownWriter WriteLine()
        {
            WriteRaw(Settings.NewLineChars);
            OnAfterWriteLine();
            return this;
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

            if (CurrentKind == MarkdownKind.IndentedCodeBlock)
                WriteRaw("    ");
        }

        protected void OnAfterWriteLine()
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

        protected void OnBeforeWrite()
        {
            if (_pendingEmptyLine)
            {
                WriteIndentation();
                WriteLine();
            }
            else if (_startOfLine)
            {
                WriteIndentation();
            }

            _startOfLine = false;
            _emptyLine = false;
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

        protected enum State
        {
            None,
            Raw,
            InlineCode,
            LinkText,
            LinkUrl,
            LinkTitle,
            AngleBrackets
        }
    }
}