// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Pihrtsoft.Markdown.Linq;
using static Pihrtsoft.Markdown.Linq.MarkdownFactory;
using static Pihrtsoft.Markdown.TextUtility;

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private const WriteState InitialState = WriteState.StartOfDocument | WriteState.StartOfLine;

        private bool _disposed;

        private WriteState _state = InitialState;

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

        private string NewLineChars => Settings.NewLineChars;

        private NewLineHandling NewLineHandling => Settings.NewLineHandling;

        private string BoldDelimiter => BoldDelimiter(Format.BoldStyle);

        private string ItalicDelimiter => ItalicDelimiter(Format.ItalicStyle);

        private BulletListStyle BulletListStyle => Format.BulletListStyle;

        private string BulletItemStart => BulletItemStart(BulletListStyle);

        private OrderedListStyle OrderedListStyle => Format.OrderedListStyle;

        private string OrderedItemStart => OrderedItemStart(OrderedListStyle);

        private bool AddEmptyLineBeforeHeading => Format.EmptyLineBeforeHeading;

        private bool AddEmptyLineAfterHeading => Format.EmptyLineAfterHeading;

        private bool AddEmptyLineBeforeCodeBlock => Format.EmptyLineBeforeCodeBlock;

        private bool AddEmptyLineAfterCodeBlock => Format.EmptyLineAfterCodeBlock;

        private bool FormatTableHeader => Settings.FormatTableHeader;

        private bool FormatTableContent => Settings.FormatTableContent;

        private bool TableOuterDelimiter => Format.TableOuterDelimiter;

        private bool TablePadding => Format.TablePadding;

        private bool UnderlineHeading1 => Format.UnderlineHeading1;

        private bool UnderlineHeading2 => Format.UnderlineHeading2;

        private bool CloseHeading => Format.CloseHeading;

        private HeadingStyle HeadingStyle => Format.HeadingStyle;

        private CodeFenceStyle CodeFenceStyle => Format.CodeFenceStyle;

        private CharReferenceFormat CharReferenceFormat => Format.CharReferenceFormat;

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

        private void AddState(WriteState state)
        {
            Debug.Assert((_state & state) == 0, $"State {_state & state} is already set.");

            _state |= state;
        }

        private bool TryAddState(WriteState state)
        {
            if ((_state & state) != 0)
            {
                AddState(state);
                return true;
            }

            return false;
        }

        private void AddStateIf(bool condition, WriteState state)
        {
            if (condition)
                AddState(state);
        }

        private void RemoveState(WriteState state)
        {
            _state &= ~state;
        }

        private bool HasState(WriteState state)
        {
            return (_state & state) != 0;
        }

        internal virtual void Reset()
        {
            _state = InitialState;
        }

        public MarkdownWriter WriteBold(object content)
        {
            return WriteDelimiter(WriteState.Bold, BoldDelimiter, content);
        }

        public MarkdownWriter WriteBold(params object[] content)
        {
            return WriteDelimiter(WriteState.Bold, BoldDelimiter, content);
        }

        public MarkdownWriter WriteItalic(object content)
        {
            return WriteDelimiter(WriteState.Italic, ItalicDelimiter, content);
        }

        public MarkdownWriter WriteItalic(params object[] content)
        {
            return WriteDelimiter(WriteState.Italic, ItalicDelimiter, content);
        }

        public MarkdownWriter WriteStrikethrough(object content)
        {
            return WriteDelimiter(WriteState.Strikethrough, StrikethroughDelimiter, content);
        }

        public MarkdownWriter WriteStrikethrough(params object[] content)
        {
            return WriteDelimiter(WriteState.Strikethrough, StrikethroughDelimiter, content);
        }

        private MarkdownWriter WriteDelimiter(WriteState state, string delimiter, object value)
        {
            bool isSet = TryAddState(state);

            if (isSet)
            {
                Write(value);
            }
            else
            {
                WriteSyntax(delimiter);
                Write(value);
                WriteSyntax(delimiter);
                RemoveState(state);
            }

            return this;
        }

        public MarkdownWriter WriteInlineCode(string value)
        {
            AddState(WriteState.Code);
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
            RemoveState(WriteState.Code);
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

            bool underline = (level == 1 && UnderlineHeading1)
                || (level == 2 && UnderlineHeading2);

            WriteLineIfNecessary();
            WriteEmptyLineIf(AddEmptyLineBeforeHeading);

            AddState(WriteState.Heading);

            if (!underline)
            {
                WriteRaw(HeadingStartChar(HeadingStyle), level);
                WriteSpace();
            }

            int length = WriteGetLength(content);

            if (length > 0
                && !underline
                && CloseHeading)
            {
                WriteSpace();
                WriteRaw(HeadingStartChar(HeadingStyle), level);
            }

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? '=' : '-', length);
                WriteLine();
            }

            RemoveState(WriteState.Heading);
            AddStateIf(AddEmptyLineAfterHeading, WriteState.PendingEmptyLine);
            return this;
        }

        public MarkdownWriter WriteBulletItem(object content)
        {
            return WriteItemCore(state: WriteState.BulletItem, prefix1: null, prefix2: BulletItemStart, content: content);
        }

        public MarkdownWriter WriteBulletItem(params object[] content)
        {
            return WriteBulletItem((object)content);
        }

        public MarkdownWriter WriteOrderedItem(int number, object content)
        {
            Error.ThrowOnInvalidItemNumber(number);

            return WriteItemCore(state: WriteState.OrderedItem, prefix1: number.ToString(), prefix2: OrderedItemStart, content: content);
        }

        public MarkdownWriter WriteOrderedItem(int number, params object[] content)
        {
            return WriteOrderedItem(number, (object)content);
        }

        public MarkdownWriter WriteTaskItem(object content)
        {
            return WriteItemCore(state: WriteState.TaskItem, prefix1: null, prefix2: TaskItemStart(), content: content);
        }

        public MarkdownWriter WriteTaskItem(params object[] content)
        {
            return WriteTaskItem((object)content);
        }

        public MarkdownWriter WriteCompletedTaskItem(object content)
        {
            return WriteItemCore(state: WriteState.TaskItem, prefix1: null, prefix2: TaskItemStart(isCompleted: true), content: content);
        }

        public MarkdownWriter WriteCompletedTaskItem(params object[] content)
        {
            return WriteCompletedTaskItem((object)content);
        }

        private MarkdownWriter WriteItemCore(WriteState state, string prefix1, string prefix2, object content)
        {
            WriteLineIfNecessary();
            WriteSyntax(prefix1);
            WriteSyntax(prefix2);
            AddState(state);
            Write(content);
            WriteLineIfNecessary();
            RemoveState(state);
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
                if (element is BulletItem item)
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

                if (element is BulletItem item)
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
                if (element is BulletItem item)
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
                if (element is BulletItem item)
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

            AddState(WriteState.Image);
            WriteSyntax("!");
            WriteLinkCore(text, url, title);
            RemoveState(WriteState.Image);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            AddState(WriteState.Link);
            WriteLinkCore(text, url, title);
            RemoveState(WriteState.Link);
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

            AddState(WriteState.Autolink);
            WriteAngleBrackets(url);
            RemoveState(WriteState.Autolink);
            return this;
        }

        public MarkdownWriter WriteImageReference(string text, string label)
        {
            WriteSyntax("!");
            AddState(WriteState.ImageReference);
            WriteLinkReferenceCore(text, label);
            RemoveState(WriteState.ImageReference);
            return this;
        }

        public MarkdownWriter WriteLinkReference(string text, string label = null)
        {
            AddState(WriteState.LinkReference);
            WriteLinkReferenceCore(text, label);
            RemoveState(WriteState.LinkReference);
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
            AddState(WriteState.Label);
            WriteSquareBrackets(label);
            WriteSyntax(": ");
            WriteAngleBrackets(url);
            WriteLinkTitle(title);
            WriteLineIfNecessary();
            RemoveState(WriteState.Label);
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
            WriteEmptyLineIf(AddEmptyLineBeforeCodeBlock);
            AddState(WriteState.IndentedCodeBlock);
            WriteRaw(text);
            WriteLineIfNecessary();
            RemoveState(WriteState.IndentedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, WriteState.PendingEmptyLine);
            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            WriteLineIfNecessary();
            WriteEmptyLineIf(AddEmptyLineBeforeCodeBlock);

            AddState(WriteState.FencedCodeBlock);

            WriteCodeFence();
            WriteSyntax(info);
            WriteLine();

            WriteRaw(text);
            WriteLineIfNecessary();

            WriteCodeFence();
            WriteLine();

            RemoveState(WriteState.FencedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, WriteState.PendingEmptyLine);
            return this;
        }

        private MarkdownWriter WriteCodeFence()
        {
            switch (CodeFenceStyle)
            {
                case CodeFenceStyle.Backtick:
                    return WriteSyntax("```");
                case CodeFenceStyle.Tilde:
                    return WriteSyntax("~~~");
                default:
                    throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(CodeFenceStyle));
            }
        }

        public MarkdownWriter WriteBlockQuote(object content)
        {
            QuoteLevel++;
            AddState(WriteState.BlockQuote);
            Write(content);
            WriteLineIfNecessary();
            RemoveState(WriteState.BlockQuote);
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

            AddState(WriteState.HorizontalRule);

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
            RemoveState(WriteState.HorizontalRule);
            return this;
        }

        public MarkdownWriter WriteTable(params MElement[] rows)
        {
            return WriteTable((IEnumerable<MElement>)rows);
        }

        public MarkdownWriter WriteTable(IEnumerable<MElement> rows)
        {
            List<TableColumnInfo> columns = MeasureTable(rows);

            if (columns == null)
                return this;

            AddState(WriteState.Table);

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    WriteTableHeader(en.Current, columns);

                    while (en.MoveNext())
                        WriteTableRow(en.Current, columns);
                }
            }

            RemoveState(WriteState.Table);
            return this;
        }

        protected abstract List<TableColumnInfo> MeasureTable(IEnumerable<MElement> rows);

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

                if (TablePadding)
                {
                    WriteSpace();
                }
                else if (FormatTableHeader
                    && alignment == Alignment.Center)
                {
                    WriteSpace();
                }

                int width = WriteGetLength(cellContent);

                if (FormatTableHeader)
                {
                    int minimalWidth = Math.Max(width, 3);

                    WritePadRight(width, columns[i].Width, minimalWidth);

                    if (!TablePadding
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

                if (FormatTableHeader)
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
                    if (TableOuterDelimiter
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

                if (FormatTableContent)
                    WritePadRight(length, columns[i].Width);

                WriteTableCellEnd(isLast);
            }
        }

        private void WriteTableCellStart(bool isFirst, bool isLast, bool isWhiteSpace = false)
        {
            if (isFirst)
            {
                WriteLineIfNecessary();

                if (TableOuterDelimiter
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
                if (TableOuterDelimiter)
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
            if (TablePadding)
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

            if (CharReferenceFormat == CharReferenceFormat.Hexadecimal)
            {
                WriteSyntax("x");
                WriteRaw(number.ToString("x", CultureInfo.InvariantCulture));
            }
            else if (CharReferenceFormat == CharReferenceFormat.Decimal)
            {
                WriteRaw(number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(CharReferenceFormat));
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
            AddState(WriteState.Comment);
            WriteSyntax("<!-- ");
            WriteRaw(value);
            WriteSyntax(" -->");
            RemoveState(WriteState.Comment);
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
                    WriteCore(value, lastIndex, value.Length - lastIndex);
                    return this;
                }
            }

            BeforeWrite();
            WriteCore(value);
            return this;

            void WriteLine(int startIndex, int index)
            {
                if (NewLineHandling == NewLineHandling.Replace)
                {
                    index--;
                    if (index > 0
                        && value[index - 1] == '\r')
                    {
                        index--;
                    }
                }

                BeforeWrite();
                WriteCore(value, startIndex, index - startIndex);

                if (NewLineHandling == NewLineHandling.Replace)
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
                WriteCore(value, startIndex, index - startIndex);
                WriteCore(escapingChar);
                WriteCore(ch);
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
            if (!HasState(WriteState.StartOfLine))
                WriteLine();
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            if (!HasState(WriteState.StartOfDocument | WriteState.EmptyLine))
                WriteLine();
        }

        private MarkdownWriter WriteRaw(char value)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();
            WriteCore(value);
            return this;
        }

        private MarkdownWriter WriteRaw(char value, int repeatCount)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();

            for (int i = 0; i < repeatCount; i++)
                WriteCore(value);

            return this;
        }

        private MarkdownWriter WriteSyntax(string value)
        {
            BeforeWrite();
            WriteCore(value);
            return this;
        }

        public MarkdownWriter WriteRaw(string value)
        {
            Write(value, f => false);
            return this;
        }

        public MarkdownWriter WriteRaw(int value)
        {
            BeforeWrite();
            WriteCore(value);
            return this;
        }

        public MarkdownWriter WriteLine()
        {
            WriteLineCore();
            AfterWriteLine();
            return this;
        }

        private void AfterWriteLine()
        {
            RemoveState(WriteState.PendingEmptyLine);

            if (HasState(WriteState.StartOfLine))
            {
                AddState(WriteState.EmptyLine);
            }
            else
            {
                AddState(WriteState.StartOfLine);
            }
        }

        private void BeforeWrite()
        {
            if (HasState(WriteState.PendingEmptyLine))
            {
                WriteIndentation();
                WriteLine();
            }
            else if (HasState(WriteState.StartOfLine))
            {
                WriteIndentation();
                RemoveState(WriteState.StartOfDocument | WriteState.StartOfLine | WriteState.EmptyLine);
            }
        }

        private void WriteIndentation()
        {
            for (int i = 1; i <= QuoteLevel; i++)
                WriteCore(BlockQuoteStart);

            if (HasState(WriteState.BulletItem | WriteState.OrderedItem | WriteState.TaskItem))
                WriteCore("  ");

            if (HasState(WriteState.IndentedCodeBlock))
                WriteCore("    ");
        }

        internal MarkdownWriter WriteSpace()
        {
            return WriteSyntax(" ");
        }

        protected abstract void WriteCore(string value);

        protected abstract void WriteCore(string value, int startIndex, int count);

        protected abstract void WriteCore(char value);

        protected abstract void WriteCore(int value);

        protected abstract void WriteCore(uint value);

        protected abstract void WriteCore(long value);

        protected abstract void WriteCore(ulong value);

        protected abstract void WriteCore(float value);

        protected abstract void WriteCore(double value);

        protected abstract void WriteCore(decimal value);

        protected virtual void WriteLineCore()
        {
            WriteCore(NewLineChars);
        }
    }
}