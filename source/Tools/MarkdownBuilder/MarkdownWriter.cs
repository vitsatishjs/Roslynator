// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;
        private bool _inIndentedCodeBlock;

        private int _lineStartPos;
        private int _emptyLineStartPos = -1;

        private int _headingLevel = -1;

        private IReadOnlyList<TableColumnInfo> _tableColumns;
        private int _tableColumnCount = -1;
        private int _tableRowIndex = -1;
        private int _tableColumnIndex = -1;
        private int _tableCellPos = -1;

        private readonly Stack<MarkdownKind> _stack = new Stack<MarkdownKind>();

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
        }

        public WriteState WriteState { get; }

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

        private void CheckPush(MarkdownKind kind)
        {
            Check(kind);
            _stack.Push(kind);
        }

        private void Pop()
        {
            _stack.Pop();
        }

        internal void Check(MarkdownKind _)
        {
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

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '`')
                    WriteRaw(" ");

                WriteString(text, MarkdownEscaper.ShouldBeEscapedInInlineCode, '`');

                if (text[text.Length - 1] == '`')
                    WriteRaw(" ");
            }

            WriteRaw("`");
            return this;
        }

        public MarkdownWriter WriteStartHeading(int level)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            CheckPush(MarkdownKind.Heading);

            _headingLevel = level;

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            WriteLine(Format.EmptyLineBeforeHeading);

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

            int length = Length - _lineStartPos;

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? "=" : "-", length);
                WriteLine();
            }

            WriteEmptyLineIf(Format.EmptyLineAfterHeading);
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
            ListLevel--;
            WriteLine();
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
            ListLevel--;
            WriteLineIfNecessary();
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
            ListLevel--;
            WriteLineIfNecessary();
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
            WriteString(url, MarkdownEscaper.ShouldBeEscapedInLinkUrl);
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
                WriteString(title, MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                WriteRaw("\"");
            }
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

        public MarkdownWriter WriteIndentedCodeBlock(string text)
        {
            Check(MarkdownKind.IndentedCodeBlock);

            WriteLine(Format.EmptyLineBeforeCodeBlock);

            _inIndentedCodeBlock = true;
            WriteString(text, _ => false);
            _inIndentedCodeBlock = false;
            WriteLine();
            WriteEmptyLineIf(Format.EmptyLineAfterCodeBlock);
            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            Error.ThrowOnInvalidFencedCodeBlockInfo(info);

            Check(MarkdownKind.FencedCodeBlock);

            WriteLine(Format.EmptyLineBeforeCodeBlock);

            WriteRaw(Format.CodeFence);
            WriteRaw(info);
            WriteLine();
            WriteString(text, _ => false);

            WriteLineIfNecessary();

            WriteRaw(Format.CodeFence);

            WriteLine();
            WriteEmptyLineIf(Format.EmptyLineAfterCodeBlock);
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
            return WriteHorizontalRule(Format.HorizontalRuleFormat);
        }

        public MarkdownWriter WriteHorizontalRule(HorizontalRuleFormat format)
        {
            return WriteHorizontalRule(format.Text, format.Count, format.Separator);
        }

        public MarkdownWriter WriteHorizontalRule(string text, int count = HorizontalRuleFormat.DefaultCount, string separator = HorizontalRuleFormat.DefaultSeparator)
        {
            Error.ThrowOnInvalidHorizontalRuleText(text);
            Error.ThrowOnInvalidHorizontalRuleCount(count);
            Error.ThrowOnInvalidHorizontalRuleSeparator(separator);

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

                WriteRaw(text);
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteStartTable(int columnCount)
        {
            return WriteStartTable(null, columnCount);
        }

        public MarkdownWriter WriteStartTable(IReadOnlyList<TableColumnInfo> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            return WriteStartTable(columns, columns.Count);
        }

        private MarkdownWriter WriteStartTable(IReadOnlyList<TableColumnInfo> columns, int columnCount)
        {
            CheckPush(MarkdownKind.Table);

            WriteLine(Format.EmptyLineBeforeTable);

            _tableColumns = columns;
            _tableColumnCount = columnCount;
            return this;
        }

        public MarkdownWriter WriteEndTable()
        {
            _tableRowIndex = -1;
            _tableColumns = null;
            _tableColumnCount = -1;
            WriteEmptyLineIf(Format.EmptyLineAfterTable);
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
            CheckPush(MarkdownKind.TableCell);
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

        public void WriteEndTableCell()
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
            Pop();
        }

        public void WriteTableHeaderSeparator()
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

        private MarkdownWriter WriteTableColumnSeparator()
        {
            return WriteRaw("|");
        }

        private void WritePadRight(int width, string padding = " ")
        {
            int totalWidth = Math.Max(CurrentColumn.Width, Math.Max(width, 3));

            WriteRaw(padding, totalWidth - width);
        }

        public MarkdownWriter WriteCharEntity(char value)
        {
            Error.ThrowOnInvalidCharEntity(value);

            Check(MarkdownKind.CharEntity);
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
            return this;
        }

        public MarkdownWriter WriteEntityRef(string name)
        {
            Check(MarkdownKind.EntityRef);
            WriteRaw("&");
            WriteRaw(name);
            WriteRaw(";");
            return this;
        }

        public MarkdownWriter WriteComment(string text)
        {
            Check(MarkdownKind.Comment);

            if (!string.IsNullOrEmpty(text))
            {
                if (text.IndexOf("--", StringComparison.Ordinal) >= 0)
                    throw new ArgumentException("XML comment text cannot contain '--'.");

                if (text[text.Length - 1] == '-')
                    throw new ArgumentException("Last character of XML comment text cannot be '-'.");
            }

            WriteRaw("<!-- ");
            WriteRaw(text);
            WriteRaw(" -->");
            return this;
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

        public abstract MarkdownWriter WriteString(string text);

        private void WriteString(string text, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            ShouldBeEscaped = shouldBeEscaped;
            EscapingChar = escapingChar;
            WriteString(text);
            EscapingChar = '\\';
            ShouldBeEscaped = MarkdownEscaper.ShouldBeEscaped;
        }

        public abstract MarkdownWriter WriteRaw(string data);

        private MarkdownWriter WriteRaw(string data, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                WriteRaw(data);

            return this;
        }

        public virtual MarkdownWriter WriteLine()
        {
            WriteRaw(NewLineChars);
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

            if (_inIndentedCodeBlock)
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
    }
}