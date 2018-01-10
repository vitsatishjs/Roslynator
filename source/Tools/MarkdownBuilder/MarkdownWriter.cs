// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Pihrtsoft.Markdown.Linq;
using static Pihrtsoft.Markdown.Linq.MarkdownFactory;
using static Pihrtsoft.Markdown.TextUtility;

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private const State InitialState = State.StartOfDocument | State.StartOfLine;

        private bool _disposed;

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

        protected abstract int Length { get; set; }

        private string NewLineChars => Settings.NewLineChars;

        private NewLineHandling NewLineHandling => Settings.NewLineHandling;

        private string BoldDelimiter => BoldDelimiter(Format.BoldStyle);

        private string ItalicDelimiter => ItalicDelimiter(Format.ItalicStyle);

        private ListStyle ListStyle => Format.ListStyle;

        private string ListItemStart => ListItemStart(ListStyle);

        private OrderedListStyle OrderedListStyle => Format.OrderedListStyle;

        private string OrderedListItemStart => OrderedListItemStart(OrderedListStyle);

        private bool AddEmptyLineBeforeHeading => Format.EmptyLineBeforeHeading;

        private bool AddEmptyLineAfterHeading => Format.EmptyLineAfterHeading;

        private bool AddEmptyLineBeforeCodeBlock => Format.EmptyLineBeforeCodeBlock;

        private bool AddEmptyLineAfterCodeBlock => Format.EmptyLineAfterCodeBlock;

        private TableOptions TableOptions => Format.TableOptions;

        private bool FormatTableHeader => (TableOptions & TableOptions.FormatHeader) != 0;

        private bool FormatTableContent => (TableOptions & TableOptions.FormatContent) != 0;

        private bool TableOuterDelimiter => Format.TableOuterDelimiter;

        private bool TablePadding => Format.TablePadding;

        private bool UnderlineHeading1 => Format.UnderlineHeading1;

        private bool UnderlineHeading2 => Format.UnderlineHeading2;

        private bool CloseHeading => Format.CloseHeading;

        private HeadingStyle HeadingStyle => Format.HeadingStyle;

        private CodeFenceStyle CodeFenceStyle => Format.CodeFenceStyle;

        private CharReferenceFormat CharReferenceFormat => Format.CharReferenceFormat;

        private State State { get; set; } = InitialState;

        public static MarkdownWriter Create(TextWriter writer, MarkdownWriterSettings settings = null)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return new MarkdownTextWriter(writer, settings);
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

        private void AddState(State state)
        {
            Debug.Assert((State & state) == 0, $"State {State & state} is already set.");

            State |= state;
        }

        private bool TryAddState(State state)
        {
            if ((State & state) != 0)
            {
                AddState(state);
                return true;
            }

            return false;
        }

        private void AddStateIf(bool condition, State state)
        {
            if (condition)
                AddState(state);
        }

        private void RemoveState(State state)
        {
            State &= ~state;
        }

        private bool HasState(State state)
        {
            return (State & state) != 0;
        }

        protected void ResetState()
        {
            State = InitialState;
        }

        public MarkdownWriter WriteBold(object content)
        {
            return WriteDelimiter(State.Bold, BoldDelimiter, content);
        }

        public MarkdownWriter WriteBold(params object[] content)
        {
            return WriteDelimiter(State.Bold, BoldDelimiter, content);
        }

        public MarkdownWriter WriteItalic(object content)
        {
            return WriteDelimiter(State.Italic, ItalicDelimiter, content);
        }

        public MarkdownWriter WriteItalic(params object[] content)
        {
            return WriteDelimiter(State.Italic, ItalicDelimiter, content);
        }

        public MarkdownWriter WriteStrikethrough(object content)
        {
            return WriteDelimiter(State.Strikethrough, StrikethroughDelimiter, content);
        }

        public MarkdownWriter WriteStrikethrough(params object[] content)
        {
            return WriteDelimiter(State.Strikethrough, StrikethroughDelimiter, content);
        }

        private MarkdownWriter WriteDelimiter(State state, string delimiter, object value)
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
            AddState(State.Code);
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
            RemoveState(State.Code);
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

            AddState(State.Heading);

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

            RemoveState(State.Heading);
            AddStateIf(AddEmptyLineAfterHeading, State.PendingEmptyLine);
            return this;
        }

        public MarkdownWriter WriteListItem(object content)
        {
            return WriteItemCore(state: State.ListItem, prefix1: null, prefix2: ListItemStart, content: content);
        }

        public MarkdownWriter WriteListItem(params object[] content)
        {
            return WriteListItem((object)content);
        }

        public MarkdownWriter WriteOrderedListItem(int number, object content)
        {
            Error.ThrowOnInvalidItemNumber(number);

            return WriteItemCore(state: State.OrderedListItem, prefix1: number.ToString(), prefix2: OrderedListItemStart, content: content);
        }

        public MarkdownWriter WriteOrderedListItem(int number, params object[] content)
        {
            return WriteOrderedListItem(number, (object)content);
        }

        public MarkdownWriter WriteTaskListItem(object content)
        {
            return WriteItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(), content: content);
        }

        public MarkdownWriter WriteTaskListItem(params object[] content)
        {
            return WriteTaskListItem((object)content);
        }

        public MarkdownWriter WriteCompletedTaskListItem(object content)
        {
            return WriteItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(isCompleted: true), content: content);
        }

        public MarkdownWriter WriteCompletedTaskListItem(params object[] content)
        {
            return WriteCompletedTaskListItem((object)content);
        }

        private MarkdownWriter WriteItemCore(State state, string prefix1, string prefix2, object content)
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

        public MarkdownWriter WriteListItems(params MElement[] content)
        {
            return WriteListItems((IEnumerable<MElement>)content);
        }

        public MarkdownWriter WriteListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    WriteListItem(item.TextOrElements());
                }
                else
                {
                    WriteListItem(element);
                }
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteOrderedListItems(params MElement[] content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            for (int i = 0; i < content.Length; i++)
            {
                MElement element = content[i];

                if (element is ListItem item)
                {
                    WriteOrderedListItem(i + 1, item.TextOrElements());
                }
                else
                {
                    WriteOrderedListItem(i + 1, element);
                }
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteOrderedListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            int number = 1;
            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    WriteOrderedListItem(number, item.TextOrElements());
                }
                else
                {
                    WriteOrderedListItem(number, element);
                }

                number++;
            }

            WriteLine();
            return this;
        }

        public MarkdownWriter WriteTaskListItems(params MElement[] content)
        {
            return WriteTaskListItems((IEnumerable<MElement>)content);
        }

        public MarkdownWriter WriteTaskListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    WriteTaskListItem(item.TextOrElements());
                }
                else
                {
                    WriteTaskListItem(element);
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

            AddState(State.Image);
            WriteSyntax("!");
            WriteLinkCore(text, url, title);
            RemoveState(State.Image);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            AddState(State.Link);
            WriteLinkCore(text, url, title);
            RemoveState(State.Link);
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

            AddState(State.Autolink);
            WriteAngleBrackets(url);
            RemoveState(State.Autolink);
            return this;
        }

        public MarkdownWriter WriteImageReference(string text, string label)
        {
            WriteSyntax("!");
            AddState(State.ImageReference);
            WriteLinkReferenceCore(text, label);
            RemoveState(State.ImageReference);
            return this;
        }

        public MarkdownWriter WriteLinkReference(string text, string label = null)
        {
            AddState(State.LinkReference);
            WriteLinkReferenceCore(text, label);
            RemoveState(State.LinkReference);
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
            AddState(State.Label);
            WriteSquareBrackets(label);
            WriteSyntax(": ");
            WriteAngleBrackets(url);
            WriteLinkTitle(title);
            WriteLineIfNecessary();
            RemoveState(State.Label);
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
            AddState(State.IndentedCodeBlock);
            WriteRaw(text);
            WriteLineIfNecessary();
            RemoveState(State.IndentedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, State.PendingEmptyLine);
            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            WriteLineIfNecessary();
            WriteEmptyLineIf(AddEmptyLineBeforeCodeBlock);

            AddState(State.FencedCodeBlock);

            WriteCodeFence();
            WriteSyntax(info);
            WriteLine();

            WriteRaw(text);
            WriteLineIfNecessary();

            WriteCodeFence();
            WriteLine();

            RemoveState(State.FencedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, State.PendingEmptyLine);
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
            AddState(State.BlockQuote);
            Write(content);
            WriteLineIfNecessary();
            RemoveState(State.BlockQuote);
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

            AddState(State.HorizontalRule);

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
            RemoveState(State.HorizontalRule);
            return this;
        }

        public MarkdownWriter WriteTable(params MElement[] rows)
        {
            return WriteTable((IEnumerable<MElement>)rows);
        }

        public MarkdownWriter WriteTable(IEnumerable<MElement> rows)
        {
            AddState(State.Table);

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    List<TableColumnInfo> columns = CalculateWidths(rows);

                    MElement header = en.Current;

                    WriteTableHeader(header, columns);

                    while (en.MoveNext())
                        WriteTableRow(en.Current, columns);
                }
            }

            RemoveState(State.Table);
            return this;
        }

        private List<TableColumnInfo> CalculateWidths(IEnumerable<MElement> rows)
        {
            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                MarkdownStringWriter sw = MarkdownStringBuilderCache.GetInstance(Settings);

                en.MoveNext();

                MElement header = en.Current;

                var infos = new List<TableColumnInfo>();

                if (header is MContainer container)
                {
                    WriteHeaderCells(container, sw, infos);
                }
                else
                {
                    sw.Write(header);
                    infos.Add(TableColumnInfo.Create(header, sw));
                }

                if (FormatTableContent)
                {
                    sw.Clear();
                    int index = 0;

                    while (en.MoveNext())
                    {
                        int columnCount = infos.Count;

                        MElement row = en.Current;

                        if (row is MContainer rowContainer)
                        {
                            int i = 0;
                            foreach (MElement cell in rowContainer.Elements())
                            {
                                sw.Write(cell);
                                infos[i] = infos[i].UpdateWidthIfGreater(sw.Length - index);
                                index = sw.Length;
                                i++;

                                if (i == columnCount)
                                    break;
                            }
                        }
                        else
                        {
                            sw.Write(row);
                            infos[0] = infos[0].UpdateWidthIfGreater(sw.Length - index);
                            index = sw.Length;
                        }
                    }
                }

                MarkdownStringBuilderCache.Free(sw);

                return infos;
            }
        }

        private void WriteHeaderCells(
            MElement header,
            MarkdownStringWriter sw,
            List<TableColumnInfo> infos)
        {
            if (!(header is MContainer container))
            {
                sw.Write(header);
                infos.Add(TableColumnInfo.Create(header, sw));
                return;
            }

            int index = 0;

            bool isFirst = true;
            bool isLast = false;

            int i = 0;

            using (IEnumerator<MElement> en = container.Elements().GetEnumerator())
            {
                if (en.MoveNext())
                {
                    MElement curr = en.Current;

                    isLast = !en.MoveNext();

                    WriteHeaderCell(curr);

                    if (!isLast)
                    {
                        isFirst = false;

                        do
                        {
                            curr = en.Current;
                            isLast = !en.MoveNext();
                            i++;

                            WriteHeaderCell(curr);
                        }
                        while (!isLast);
                    }
                }
            }

            void WriteHeaderCell(MElement cellContent)
            {
                if (isFirst
                    || isLast
                    || FormatTableHeader)
                {
                    sw.Write(cellContent);
                }

                infos.Add(TableColumnInfo.Create(cellContent, sw, index));
                index = sw.Length;
            }
        }

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
            AddState(State.Comment);
            WriteSyntax("<!-- ");
            WriteRaw(value);
            WriteSyntax(" -->");
            RemoveState(State.Comment);
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
            if (!HasState(State.StartOfLine))
                WriteLine();
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            if (!HasState(State.StartOfDocument | State.EmptyLine))
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
            WriteCore(value, repeatCount);
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
            RemoveState(State.PendingEmptyLine);

            if (HasState(State.StartOfLine))
            {
                AddState(State.EmptyLine);
            }
            else
            {
                AddState(State.StartOfLine);
            }
        }

        private void BeforeWrite()
        {
            if (HasState(State.PendingEmptyLine))
            {
                WriteIndentation();
                WriteLine();
            }
            else if (HasState(State.StartOfLine))
            {
                WriteIndentation();
                RemoveState(State.StartOfDocument | State.StartOfLine | State.EmptyLine);
            }
        }

        private void WriteIndentation()
        {
            for (int i = 1; i <= QuoteLevel; i++)
                WriteCore(BlockQuoteStart);

            if (HasState(State.ListItem | State.OrderedListItem | State.TaskListItem))
                WriteCore("  ");

            if (HasState(State.IndentedCodeBlock))
                WriteCore("    ");
        }

        internal MarkdownWriter WriteSpace()
        {
            return WriteSyntax(" ");
        }

        protected abstract void WriteCore(string value);

        protected abstract void WriteCore(string value, int startIndex, int count);

        protected abstract void WriteCore(char value);

        protected abstract void WriteCore(char value, int repeatCount);

        protected void WriteCore(int value)
        {
            WriteCore(value.ToString());
        }

        protected void WriteLineCore()
        {
            WriteCore(NewLineChars);
        }
    }
}