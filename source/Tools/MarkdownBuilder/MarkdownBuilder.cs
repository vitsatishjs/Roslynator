// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using static Pihrtsoft.Markdown.Linq.MarkdownFactory;
using static Pihrtsoft.Markdown.TextUtility;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    public class MarkdownBuilder
    {
        private const State InitialState = State.StartOfDocument | State.StartOfLine;

        private MarkdownFormat _format;

        public MarkdownBuilder(MarkdownFormat format = null)
            : this(new StringBuilder(), format)
        {
        }

        public MarkdownBuilder(StringBuilder sb, MarkdownFormat format = null)
        {
            StringBuilder = sb ?? throw new ArgumentNullException(nameof(sb));
            Format = format ?? MarkdownFormat.Default;
        }

        public MarkdownFormat Format
        {
            get { return _format; }
            set { _format = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public int QuoteLevel { get; private set; }

        public StringBuilder StringBuilder { get; }

        public int Length => StringBuilder.Length;

        private string BoldDelimiter => BoldDelimiter(Format.BoldStyle);

        private string ItalicDelimiter => ItalicDelimiter(Format.ItalicStyle);

        private ListItemStyle ListItemStyle => Format.ListItemStyle;

        private string ListItemStart => ListItemStart(ListItemStyle);

        private OrderedListItemStyle OrderedListItemStyle => Format.OrderedListItemStyle;

        private string OrderedListItemStart => OrderedListItemStart(OrderedListItemStyle);

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

        public char this[int index]
        {
            get { return StringBuilder[index]; }
        }

        private State State { get; set; } = InitialState;

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

        public MarkdownBuilder AppendBold(object content)
        {
            return AppendDelimiter(State.Bold, BoldDelimiter, content);
        }

        public MarkdownBuilder AppendBold(params object[] content)
        {
            return AppendDelimiter(State.Bold, BoldDelimiter, content);
        }

        public MarkdownBuilder AppendItalic(object content)
        {
            return AppendDelimiter(State.Italic, ItalicDelimiter, content);
        }

        public MarkdownBuilder AppendItalic(params object[] content)
        {
            return AppendDelimiter(State.Italic, ItalicDelimiter, content);
        }

        public MarkdownBuilder AppendStrikethrough(object content)
        {
            return AppendDelimiter(State.Strikethrough, StrikethroughDelimiter, content);
        }

        public MarkdownBuilder AppendStrikethrough(params object[] content)
        {
            return AppendDelimiter(State.Strikethrough, StrikethroughDelimiter, content);
        }

        //TODO: Trim value
        private MarkdownBuilder AppendDelimiter(State state, string delimiter, object value)
        {
            bool isSet = TryAddState(state);

            if (isSet)
            {
                Append(value);
            }
            else
            {
                AppendSyntax(delimiter);
                Append(value);
                AppendSyntax(delimiter);
                RemoveState(state);
            }

            return this;
        }

        public MarkdownBuilder AppendInlineCode(string value)
        {
            AddState(State.Code);
            AppendSyntax(CodeDelimiter);

            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == CodeDelimiterChar)
                    AppendSpace();

                Append(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);

                if (value[value.Length - 1] == CodeDelimiterChar)
                    AppendSpace();
            }

            AppendSyntax(CodeDelimiter);
            RemoveState(State.Code);
            return this;
        }

        public MarkdownBuilder AppendHeading1(object content)
        {
            return AppendHeading(1, content);
        }

        public MarkdownBuilder AppendHeading1(params object[] content)
        {
            return AppendHeading(1, content);
        }

        public MarkdownBuilder AppendHeading2(object content)
        {
            return AppendHeading(2, content);
        }

        public MarkdownBuilder AppendHeading2(params object[] content)
        {
            return AppendHeading(2, content);
        }

        public MarkdownBuilder AppendHeading3(object content)
        {
            return AppendHeading(3, content);
        }

        public MarkdownBuilder AppendHeading3(params object[] content)
        {
            return AppendHeading(3, content);
        }

        public MarkdownBuilder AppendHeading4(object content)
        {
            return AppendHeading(4, content);
        }

        public MarkdownBuilder AppendHeading4(params object[] content)
        {
            return AppendHeading(4, content);
        }

        public MarkdownBuilder AppendHeading5(object content)
        {
            return AppendHeading(5, content);
        }

        public MarkdownBuilder AppendHeading5(params object[] content)
        {
            return AppendHeading(5, content);
        }

        public MarkdownBuilder AppendHeading6(object content)
        {
            return AppendHeading(6, content);
        }

        public MarkdownBuilder AppendHeading6(params object[] content)
        {
            return AppendHeading(6, content);
        }

        public MarkdownBuilder AppendHeading(int level, object content)
        {
            return AppendHeadingCore(level, content);
        }

        public MarkdownBuilder AppendHeading(int level, params object[] content)
        {
            return AppendHeadingCore(level, content);
        }

        private MarkdownBuilder AppendHeadingCore(int level, object content)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            bool underline = (level == 1 && UnderlineHeading1)
                || (level == 2 && UnderlineHeading2);

            AppendLineIfNecessary();
            AppendEmptyLineIf(AddEmptyLineBeforeHeading);

            AddState(State.Heading);

            if (!underline)
            {
                AppendRaw(HeadingStartChar(HeadingStyle), level);
                AppendSpace();
            }

            int length = AppendGetLength(content);

            if (length > 0
                && !underline
                && CloseHeading)
            {
                AppendSpace();
                AppendRaw(HeadingStartChar(HeadingStyle), level);
            }

            AppendLineIfNecessary();

            if (underline)
            {
                AppendRaw((level == 1) ? '=' : '-', length);
                AppendLine();
            }

            RemoveState(State.Heading);
            AddStateIf(AddEmptyLineAfterHeading, State.PendingEmptyLine);
            return this;
        }

        public MarkdownBuilder AppendListItem(object content)
        {
            return AppendItemCore(state: State.ListItem, prefix1: null, prefix2: ListItemStart, content: content);
        }

        public MarkdownBuilder AppendListItem(params object[] content)
        {
            return AppendListItem((object)content);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, object content)
        {
            Error.ThrowOnInvalidItemNumber(number);

            return AppendItemCore(state: State.OrderedListItem, prefix1: number.ToString(), prefix2: OrderedListItemStart, content: content);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, params object[] content)
        {
            return AppendOrderedListItem(number, (object)content);
        }

        public MarkdownBuilder AppendTaskListItem(object content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(), content: content);
        }

        public MarkdownBuilder AppendTaskListItem(params object[] content)
        {
            return AppendTaskListItem((object)content);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(object content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(isCompleted: true), content: content);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(params object[] content)
        {
            return AppendCompletedTaskListItem((object)content);
        }

        private MarkdownBuilder AppendItemCore(State state, string prefix1, string prefix2, object content)
        {
            AppendLineIfNecessary();
            AppendSyntax(prefix1);
            AppendSyntax(prefix2);
            AddState(state);
            Append(content);
            AppendLineIfNecessary();
            RemoveState(state);
            return this;
        }

        public MarkdownBuilder AppendListItems(params MElement[] content)
        {
            return AppendListItems((IEnumerable<MElement>)content);
        }

        public MarkdownBuilder AppendListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    AppendListItem(item.TextOrElements());
                }
                else
                {
                    AppendListItem(element);
                }
            }

            AppendLine();
            return this;
        }

        public MarkdownBuilder AppendOrderedListItems(params MElement[] content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            for (int i = 0; i < content.Length; i++)
            {
                MElement element = content[i];

                if (element is ListItem item)
                {
                    AppendOrderedListItem(i + 1, item.TextOrElements());
                }
                else
                {
                    AppendOrderedListItem(i + 1, element);
                }
            }

            AppendLine();
            return this;
        }

        public MarkdownBuilder AppendOrderedListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            int number = 1;
            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    AppendOrderedListItem(number, item.TextOrElements());
                }
                else
                {
                    AppendOrderedListItem(number, element);
                }

                number++;
            }

            AppendLine();
            return this;
        }

        public MarkdownBuilder AppendTaskListItems(params MElement[] content)
        {
            return AppendTaskListItems((IEnumerable<MElement>)content);
        }

        public MarkdownBuilder AppendTaskListItems(IEnumerable<MElement> content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            foreach (MElement element in content)
            {
                if (element is ListItem item)
                {
                    AppendTaskListItem(item.TextOrElements());
                }
                else
                {
                    AppendTaskListItem(element);
                }
            }

            AppendLine();
            return this;
        }

        public MarkdownBuilder AppendImage(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            AddState(State.Image);
            AppendSyntax("!");
            AppendLinkCore(text, url, title);
            RemoveState(State.Image);
            return this;
        }

        public MarkdownBuilder AppendLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            AddState(State.Link);
            AppendLinkCore(text, url, title);
            RemoveState(State.Link);
            return this;
        }

        public MarkdownBuilder AppendLinkOrText(string text, string url = null, string title = null)
        {
            return Append(LinkOrText(text, url, title));
        }

        private MarkdownBuilder AppendLinkCore(string text, string url, string title)
        {
            AppendSquareBrackets(text);
            AppendSyntax("(");
            Append(url, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            AppendLinkTitle(title);
            AppendSyntax(")");
            return this;
        }

        public MarkdownBuilder AppendAutolink(string url)
        {
            Error.ThrowOnInvalidUrl(url);

            AddState(State.Autolink);
            AppendAngleBrackets(url);
            RemoveState(State.Autolink);
            return this;
        }

        public MarkdownBuilder AppendImageReference(string text, string label)
        {
            AppendSyntax("!");
            AddState(State.ImageReference);
            AppendLinkReferenceCore(text, label);
            RemoveState(State.ImageReference);
            return this;
        }

        public MarkdownBuilder AppendLinkReference(string text, string label = null)
        {
            AddState(State.LinkReference);
            AppendLinkReferenceCore(text, label);
            RemoveState(State.LinkReference);
            return this;
        }

        private MarkdownBuilder AppendLinkReferenceCore(string text, string label = null)
        {
            AppendSquareBrackets(text);
            AppendSquareBrackets(label);
            return this;
        }

        public MarkdownBuilder AppendLabel(string label, string url, string title = null)
        {
            Error.ThrowOnInvalidUrl(url);

            AddState(State.Label);
            AppendSquareBrackets(label);
            AppendSyntax(": ");
            AppendAngleBrackets(url);
            AppendLinkTitle(title);
            RemoveState(State.Label);
            return this;
        }

        private void AppendLinkTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                AppendSpace();

                AppendSyntax("\"");
                Append(title, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                AppendSyntax("\"");
            }
        }

        private void AppendSquareBrackets(string value)
        {
            AppendSyntax("[");
            Append(value, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkText);
            AppendSyntax("]");
        }

        private void AppendAngleBrackets(string value)
        {
            AppendSyntax("<");
            Append(value, shouldBeEscaped: ch => ch == '<' || ch == '>');
            AppendSyntax(">");
        }

        public MarkdownBuilder AppendIndentedCodeBlock(string text)
        {
            AppendLineIfNecessary();
            AppendEmptyLineIf(AddEmptyLineBeforeCodeBlock);
            AddState(State.IndentedCodeBlock);
            AppendRaw(text);
            AppendLineIfNecessary();
            RemoveState(State.IndentedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, State.PendingEmptyLine);
            return this;
        }

        public MarkdownBuilder AppendFencedCodeBlock(string text, string info = null)
        {
            AppendLineIfNecessary();
            AppendEmptyLineIf(AddEmptyLineBeforeCodeBlock);

            AddState(State.FencedCodeBlock);

            AppendCodeFence();
            AppendSyntax(info);
            AppendLine();

            AppendRaw(text);
            AppendLineIfNecessary();

            AppendCodeFence();
            AppendLine();

            RemoveState(State.FencedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, State.PendingEmptyLine);
            return this;
        }

        private MarkdownBuilder AppendCodeFence()
        {
            switch (CodeFenceStyle)
            {
                case CodeFenceStyle.Backtick:
                    return AppendSyntax("```");
                case CodeFenceStyle.Tilde:
                    return AppendSyntax("~~~");
                default:
                    throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(CodeFenceStyle));
            }
        }

        public MarkdownBuilder AppendBlockQuote(object content)
        {
            QuoteLevel++;
            AddState(State.BlockQuote);
            Append(content);
            AppendLineIfNecessary();
            RemoveState(State.BlockQuote);
            QuoteLevel--;
            return this;
        }

        public MarkdownBuilder AppendBlockQuote(params object[] content)
        {
            return AppendBlockQuote((object)content);
        }

        public MarkdownBuilder AppendHorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            Error.ThrowOnInvalidHorizontalRuleCount(count);

            AppendLineIfNecessary();

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
                    AppendRaw(space);
                }

                AppendRaw(ch);
            }

            AppendLine();
            RemoveState(State.HorizontalRule);
            return this;
        }

        public MarkdownBuilder AppendTable(params MElement[] rows)
        {
            return AppendTable((IEnumerable<MElement>)rows);
        }

        public MarkdownBuilder AppendTable(IEnumerable<MElement> rows)
        {
            AddState(State.Table);

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    List<TableColumnInfo> columns = CalculateWidths(rows);

                    MElement header = en.Current;

                    AppendTableHeader(header, columns);

                    while (en.MoveNext())
                        AppendTableRow(en.Current, columns);
                }
            }

            RemoveState(State.Table);
            return this;
        }

        private List<TableColumnInfo> CalculateWidths(IEnumerable<MElement> rows)
        {
            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                MarkdownBuilder mb = MarkdownBuilderCache.GetInstance(Format);

                en.MoveNext();

                MElement header = en.Current;

                var infos = new List<TableColumnInfo>();

                if (header is MContainer container)
                {
                    AppendHeaderCells(container, mb, infos);
                }
                else
                {
                    mb.Append(header);
                    infos.Add(TableColumnInfo.Create(header, mb));
                }

                if (FormatTableContent)
                {
                    mb.Clear();
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
                                mb.Append(cell);
                                infos[i] = infos[i].UpdateWidthIfGreater(mb.Length - index);
                                index = mb.Length;
                                i++;

                                if (i == columnCount)
                                    break;
                            }
                        }
                        else
                        {
                            mb.Append(row);
                            infos[0] = infos[0].UpdateWidthIfGreater(mb.Length - index);
                            index = mb.Length;
                        }
                    }
                }

                MarkdownBuilderCache.Free(mb);

                return infos;
            }
        }

        private void AppendHeaderCells(
            MElement header,
            MarkdownBuilder mb,
            List<TableColumnInfo> infos)
        {
            if (!(header is MContainer container))
            {
                mb.Append(header);
                infos.Add(TableColumnInfo.Create(header, mb));
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

                    AppendHeaderCell(curr);

                    if (!isLast)
                    {
                        isFirst = false;

                        do
                        {
                            curr = en.Current;
                            isLast = !en.MoveNext();
                            i++;

                            AppendHeaderCell(curr);
                        }
                        while (!isLast);
                    }
                }
            }

            void AppendHeaderCell(MElement cellContent)
            {
                if (isFirst
                    || isLast
                    || FormatTableHeader)
                {
                    mb.Append(cellContent);
                }

                infos.Add(TableColumnInfo.Create(cellContent, mb, index));
                index = mb.Length;
            }
        }

        internal void AppendTableHeader(MElement content, IList<TableColumnInfo> columns)
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

                        AppendCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                AppendCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                AppendCell(content);
            }

            AppendLine();
            AppendTableHeaderSeparator(columns);

            void AppendCell(MElement cellContent)
            {
                Alignment alignment = columns[i].Alignment;

                AppendTableCellStart(isFirst, isLast, columns[i].IsWhiteSpace);

                if (TablePadding)
                {
                    AppendSpace();
                }
                else if (FormatTableHeader
                    && alignment == Alignment.Center)
                {
                    AppendSpace();
                }

                int width = AppendGetLength(cellContent);

                if (FormatTableHeader)
                {
                    int minimalWidth = Math.Max(width, 3);

                    AppendPadRight(width, columns[i].Width, minimalWidth);

                    if (!TablePadding
                        && alignment != Alignment.Left)
                    {
                        AppendSpace();
                    }
                }

                AppendTableCellEnd(isLast, columns[i].IsWhiteSpace);
            }
        }

        private void AppendTableHeaderSeparator(IList<TableColumnInfo> columns)
        {
            int count = columns.Count;

            for (int i = 0; i < count; i++)
            {
                TableColumnInfo column = columns[i];

                bool isLast = i == count - 1;

                AppendTableCellStart(i == 0, isLast, column.IsWhiteSpace);

                if (column.Alignment == Alignment.Center)
                {
                    AppendSyntax(":");
                }
                else
                {
                    AppendTablePadding();
                }

                AppendSyntax("---");

                if (FormatTableHeader)
                    AppendPadRight(3, columns[i].Width, 3, '-');

                if (column.Alignment != Alignment.Left)
                {
                    AppendSyntax(":");
                }
                else
                {
                    AppendTablePadding();
                }

                if (isLast)
                {
                    if (TableOuterDelimiter
                        || columns[i].IsWhiteSpace)
                    {
                        AppendSyntax(TableDelimiter);
                    }
                }
            }

            AppendLine();
        }

        internal MarkdownBuilder AppendTableRow(MElement content, List<TableColumnInfo> columns = null)
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

                        AppendCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                AppendCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                AppendCell(content);
            }

            AppendLine();
            return this;

            void AppendCell(MElement cell)
            {
                AppendTableCellStart(isFirst, isLast);
                AppendTablePadding();

                int length = AppendGetLength(cell);

                if (FormatTableContent)
                    AppendPadRight(length, columns[i].Width);

                AppendTableCellEnd(isLast);
            }
        }

        private void AppendTableCellStart(bool isFirst, bool isLast, bool isWhiteSpace = false)
        {
            if (isFirst)
            {
                AppendLineIfNecessary();

                if (TableOuterDelimiter
                    || isLast
                    || isWhiteSpace)
                {
                    AppendSyntax(TableDelimiter);
                }
            }
            else
            {
                AppendSyntax(TableDelimiter);
            }
        }

        private void AppendTableCellEnd(bool isLast, bool isWhiteSpace = false)
        {
            if (isLast)
            {
                if (TableOuterDelimiter)
                {
                    AppendTablePadding();
                    AppendSyntax(TableDelimiter);
                }
                else if (isWhiteSpace)
                {
                    AppendSyntax(TableDelimiter);
                }
            }
            else
            {
                AppendTablePadding();
            }
        }

        private void AppendTablePadding()
        {
            if (TablePadding)
                AppendSpace();
        }

        private void AppendPadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
                AppendRaw(paddingChar);
        }

        public MarkdownBuilder AppendCharReference(int number)
        {
            AppendSyntax("&#");

            if (CharReferenceFormat == CharReferenceFormat.Hexadecimal)
            {
                AppendSyntax("x");
                AppendRaw(number.ToString("x", CultureInfo.InvariantCulture));
            }
            else if (CharReferenceFormat == CharReferenceFormat.Decimal)
            {
                AppendRaw(number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(CharReferenceFormat));
            }

            AppendSyntax(";");
            return this;
        }

        public MarkdownBuilder AppendEntityReference(string name)
        {
            AppendSyntax("&");
            AppendSyntax(name);
            AppendSyntax(";");
            return this;
        }

        public MarkdownBuilder AppendComment(string value)
        {
            AddState(State.Comment);
            AppendSyntax("<!-- ");
            AppendRaw(value);
            AppendSyntax(" -->");
            RemoveState(State.Comment);
            return this;
        }

        private int AppendGetLength(object content)
        {
            int length = Length;
            Append(content);
            return Length - length;
        }

        public MarkdownBuilder Append(char value)
        {
            return Append(value, escape: true);
        }

        public MarkdownBuilder Append(char value, bool escape)
        {
            if (escape
                && MarkdownEscaper.ShouldBeEscaped(value))
            {
                return AppendRaw('\\');
            }

            return AppendRaw(value);
        }

        public MarkdownBuilder Append(string value)
        {
            return Append(value, escape: true);
        }

        public MarkdownBuilder Append(string value, bool escape)
        {
            if (escape)
            {
                return Append(value, MarkdownEscaper.ShouldBeEscaped);
            }
            else
            {
                return AppendRaw(value);
            }
        }

        internal MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            if (value == null)
                return this;

            BeforeAppend();

            int length = value.Length;

            bool f = false;

            for (int i = 0; i < length; i++)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    AppendLine(0, i);
                    f = true;
                }
                else if (ch == 13
                    && (i == length - 1 || value[i + 1] != 10))
                {
                    AppendLine(0, i);
                    f = true;
                }
                else if (shouldBeEscaped(ch))
                {
                    AppendEscapedChar(0, i, ch);
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
                            AppendLine(lastIndex, i);
                            f = true;
                        }
                        else if (ch == 13
                            && (i == length - 1 || value[i + 1] != 10))
                        {
                            AppendLine(0, i);
                            f = true;
                        }
                        else if (shouldBeEscaped(ch))
                        {
                            AppendEscapedChar(lastIndex, i, ch);
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

                    BeforeAppend();
                    AppendCore(value, lastIndex, value.Length - lastIndex);
                    return this;
                }
            }

            BeforeAppend();
            AppendCore(value);
            return this;

            void AppendLine(int startIndex, int index)
            {
                BeforeAppend();
                AppendCore(value, startIndex, index - startIndex);
                AfterAppendLine();
            }

            void AppendEscapedChar(int startIndex, int index, char ch)
            {
                BeforeAppend();
                AppendCore(value, startIndex, index - startIndex);
                AppendCore(escapingChar);
                AppendCore(ch);
            }
        }

        public MarkdownBuilder Append(object value)
        {
            if (value == null)
                return this;

            if (value is MElement element)
                return element.AppendTo(this);

            if (value is string s)
                return Append(s, escape: true);

            if (value is object[] arr)
            {
                foreach (object item in arr)
                    Append(item);

                return this;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    Append(item);

                return this;
            }

            return Append(value.ToString(), escape: true);
        }

        public MarkdownBuilder AppendLine(string value, bool escape = true)
        {
            Append(value, escape: escape);
            AppendLine();
            return this;
        }

        public MarkdownBuilder AppendLineRaw(string value)
        {
            return AppendLine(value, escape: false);
        }

        private void AppendLineIfNecessary()
        {
            if (!HasState(State.StartOfLine))
                AppendLine();
        }

        private void AppendEmptyLineIf(bool condition)
        {
            if (condition)
                AppendEmptyLine();
        }

        private void AppendEmptyLine()
        {
            if (!HasState(State.StartOfDocument | State.EmptyLine))
                AppendLine();
        }

        private MarkdownBuilder AppendRaw(char value)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeAppend();
            AppendCore(value);
            return this;
        }

        private MarkdownBuilder AppendRaw(char value, int repeatCount)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeAppend();
            AppendCore(value, repeatCount);
            return this;
        }

        private MarkdownBuilder AppendSyntax(string value)
        {
            BeforeAppend();
            AppendCore(value);
            return this;
        }

        public MarkdownBuilder AppendRaw(string value)
        {
            Append(value, f => false);
            return this;
        }

        public MarkdownBuilder AppendRaw(int value)
        {
            BeforeAppend();
            AppendCore(value);
            return this;
        }

        public MarkdownBuilder AppendLine()
        {
            AppendLineCore();
            AfterAppendLine();
            return this;
        }

        private void AfterAppendLine()
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

        private void BeforeAppend()
        {
            if (HasState(State.PendingEmptyLine))
            {
                AppendIndentation();
                AppendLine();
            }
            else if (HasState(State.StartOfLine))
            {
                AppendIndentation();
                RemoveState(State.StartOfDocument | State.StartOfLine | State.EmptyLine);
            }
        }

        private void AppendIndentation()
        {
            for (int i = 1; i <= QuoteLevel; i++)
                AppendCore(BlockQuoteStart);

            if (HasState(State.ListItem | State.OrderedListItem | State.TaskListItem))
                AppendCore("  ");

            if (HasState(State.IndentedCodeBlock))
                AppendCore("    ");
        }

        internal MarkdownBuilder AppendSpace()
        {
            return AppendSyntax(" ");
        }

        public void Clear()
        {
            StringBuilder.Clear();
            State = InitialState;
        }

        public override string ToString()
        {
            return StringBuilder.ToString();
        }

        public void AppendCore(string value)
        {
            StringBuilder.Append(value);
        }

        public void AppendCore(string value, int startIndex, int count)
        {
            StringBuilder.Append(value, startIndex, count);
        }

        public void AppendCore(char value)
        {
            StringBuilder.Append(value);
        }

        public void AppendCore(char value, int repeatCount)
        {
            StringBuilder.Append(value, repeatCount);
        }

        public void AppendCore(int value)
        {
            StringBuilder.Append(value);
        }

        public void AppendLineCore()
        {
            StringBuilder.AppendLine();
        }
    }
}