// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using static Pihrtsoft.Markdown.MarkdownFactory;

namespace Pihrtsoft.Markdown
{
    public class MarkdownBuilder
    {
        private const State InitialState = State.StartOfLine;

        private MarkdownFormat _format;

        public MarkdownBuilder(MarkdownFormat format = null)
            : this(new StringBuilder(), format)
        {
        }

        public MarkdownBuilder(StringBuilder sb, MarkdownFormat format = null)
        {
            Format = format ?? MarkdownFormat.Default;
            StringBuilder = sb;
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

        private HtmlEntityFormat HtmlEntityFormat => Format.HtmlEntityFormat;

        public char this[int index]
        {
            get { return StringBuilder[index]; }
        }

        private State State { get; set; } = InitialState;

        private MarkdownBuilder AddState(State state)
        {
            State |= state;
            return this;
        }

        private MarkdownBuilder AddStateIf(bool condition, State state)
        {
            if (condition)
                State |= state;

            return this;
        }

        private MarkdownBuilder RemoveState(State state)
        {
            State &= ~state;
            return this;
        }

        private bool HasState(State state)
        {
            return (State & state) != 0;
        }

        internal MarkdownBuilder WithFormat(MarkdownFormat format)
        {
            Format = format;
            return this;
        }

        public MarkdownBuilder AppendBold(string value)
        {
            return AppendDelimiter(State.Bold, BoldDelimiter, value);
        }

        public MarkdownBuilder AppendBold(object value)
        {
            return AppendDelimiter(State.Bold, BoldDelimiter, value);
        }

        public MarkdownBuilder AppendBold(params object[] values)
        {
            return AppendDelimiter(State.Bold, BoldDelimiter, null, values);
        }

        public MarkdownBuilder AppendItalic(string value)
        {
            return AppendDelimiter(State.Italic, ItalicDelimiter, value);
        }

        public MarkdownBuilder AppendItalic(object value)
        {
            return AppendDelimiter(State.Italic, ItalicDelimiter, value);
        }

        public MarkdownBuilder AppendItalic(params object[] values)
        {
            return AppendDelimiter(State.Italic, ItalicDelimiter, null, values);
        }

        internal MarkdownBuilder AppendItalicDelimiter()
        {
            return AppendSyntax(ItalicDelimiter);
        }

        public MarkdownBuilder AppendStrikethrough(string value)
        {
            return AppendDelimiter(State.Strikethrough, StrikethroughDelimiter, value);
        }

        public MarkdownBuilder AppendStrikethrough(object value)
        {
            return AppendDelimiter(State.Strikethrough, StrikethroughDelimiter, value);
        }

        public MarkdownBuilder AppendStrikethrough(params object[] values)
        {
            return AppendDelimiter(State.Strikethrough, StrikethroughDelimiter, null, values);
        }

        internal MarkdownBuilder AppendStrikethroughDelimiter()
        {
            return AppendSyntax(StrikethroughDelimiter);
        }

        public MarkdownBuilder AppendCode(string value)
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

        public MarkdownBuilder AppendCode(object value)
        {
            return AppendCodeCore(value);
        }

        public MarkdownBuilder AppendCode(params object[] values)
        {
            return AppendCodeCore(null, values);
        }

        private MarkdownBuilder AppendCodeCore(object value, params object[] additionalValues)
        {
            CodeMarkdownBuilder mb = CodeMarkdownBuilderCache.GetInstance();

            mb.Format = Format;
            mb.AppendRange(value, additionalValues);

            string s = CodeMarkdownBuilderCache.GetResultAndFree(mb);

            AddState(State.Code);
            AppendSyntax(CodeDelimiter);

            if (s.Length > 0)
            {
                if (s[0] == CodeDelimiterChar)
                    AppendSpace();

                AppendRaw(s);

                if (s[s.Length - 1] == CodeDelimiterChar)
                    AppendSpace();
            }

            AppendSyntax(CodeDelimiter);
            RemoveState(State.Code);
            return this;
        }

        internal MarkdownBuilder AppendCodeDelimiter()
        {
            return AppendSyntax(CodeDelimiter);
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
            return AppendHeadingCore(level, (object)content);
        }

        private MarkdownBuilder AppendHeadingCore(int level, object content)
        {
            Heading.ThrowOnInvalidLevel(level);

            bool underline = (level == 1 && UnderlineHeading1)
                || (level == 2 && UnderlineHeading2);

            AppendLineIfNecessary();
            AppendEmptyLineIf(AddEmptyLineBeforeHeading);

            AddState(State.Heading);

            if (!underline)
                AppendHeadingStart(level);

            int length = Length;

            Append(content);

            length = Length - length;

            if (length > 0
                && !underline
                && CloseHeading)
            {
                AppendHeadingEnd(level);
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

        internal MarkdownBuilder AppendHeadingStart(int level)
        {
            AppendRaw(HeadingStartChar(HeadingStyle), level);
            AppendSpace();
            return this;
        }

        internal MarkdownBuilder AppendHeadingEnd(int level)
        {
            AppendSpace();
            AppendRaw(HeadingStartChar(HeadingStyle), level);
            return this;
        }

        public MarkdownBuilder AppendListItem(object content)
        {
            return AppendItemCore(state: State.ListItem, prefix1: null, prefix2: ListItemStart, content: content);
        }

        public MarkdownBuilder AppendListItem(params object[] content)
        {
            return AppendItemCore(state: State.ListItem, prefix1: null, prefix2: ListItemStart, content: content);
        }

        internal MarkdownBuilder AppendListItemStart()
        {
            return AppendSyntax(ListItemStart);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, object content)
        {
            OrderedListItem.ThrowOnInvalidNumber(number);

            return AppendItemCore(state: State.OrderedListItem, prefix1: number.ToString(), prefix2: OrderedListItemStart, content: content);
        }

        public MarkdownBuilder AppendOrderedListItem(int number, params object[] content)
        {
            OrderedListItem.ThrowOnInvalidNumber(number);

            return AppendItemCore(state: State.OrderedListItem, prefix1: number.ToString(), prefix2: OrderedListItemStart, content: content);
        }

        internal MarkdownBuilder AppendOrderedListItemStart(int number)
        {
            OrderedListItem.ThrowOnInvalidNumber(number);

            AppendRaw(number);
            return AppendSyntax(OrderedListItemStart);
        }

        public MarkdownBuilder AppendTaskListItem(object content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(), content: content);
        }

        public MarkdownBuilder AppendTaskListItem(params object[] content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(), content: content);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(object content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(isCompleted: true), content: content);
        }

        public MarkdownBuilder AppendCompletedTaskListItem(params object[] content)
        {
            return AppendItemCore(state: State.TaskListItem, prefix1: null, prefix2: TaskListItemStart(isCompleted: true), content: content);
        }

        internal MarkdownBuilder AppendTaskListItemStart(bool isCompleted = false)
        {
            return AppendSyntax(TaskListItemStart(isCompleted));
        }

        private MarkdownBuilder AppendItemCore(State state, string prefix1, string prefix2, object content)
        {
            AppendLineIfNecessary();
            AppendSyntax(prefix1);
            AppendSyntax(prefix2);
            AddState(state);
            AppendRange(content);
            AppendLineIfNecessary();
            RemoveState(state);
            return this;
        }

        public MarkdownBuilder AppendOrderedItems(IEnumerable<MElement> items)
        {
            int number = 0;
            foreach (MElement item in items)
            {
                AppendOrderedListItem(number, item);
                number++;
            }

            return this;
        }

        public MarkdownBuilder AppendImage(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (url == null)
                throw new ArgumentNullException(nameof(url));

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

            if (url == null)
                throw new ArgumentNullException(nameof(url));

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
            AddState(State.Parentheses);
            Append(url, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            AppendLinkTitle(title);
            RemoveState(State.Parentheses);
            AppendSyntax(")");
            return this;
        }

        public MarkdownBuilder AppendAutoLink(string url)
        {
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
                AddState(State.DoubleQuotes);
                Append(title, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                RemoveState(State.DoubleQuotes);
                AppendSyntax("\"");
            }
        }

        private void AppendSquareBrackets(string value)
        {
            AppendSyntax("[");
            AddState(State.SquareBrackets);
            Append(value, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkText);
            RemoveState(State.SquareBrackets);
            AppendSyntax("]");
        }

        private void AppendAngleBrackets(string value)
        {
            AppendSyntax("<");
            AddState(State.AngleBrackets);
            Append(value, shouldBeEscaped: ch => ch == '<' || ch == '>');
            RemoveState(State.AngleBrackets);
            AppendSyntax(">");
        }

        public MarkdownBuilder AppendIndentedCodeBlock(string code)
        {
            AppendLineStart(State.FencedCodeBlock, addEmptyLine: AddEmptyLineBeforeCodeBlock);
            AddState(State.IndentedCodeBlock);
            AppendRaw(code);
            AppendLineIfNecessary();
            RemoveState(State.IndentedCodeBlock);
            AddStateIf(AddEmptyLineAfterCodeBlock, State.PendingEmptyLine);
            return this;
        }

        public MarkdownBuilder AppendFencedCodeBlock(string code, string info = null)
        {
            AppendLineIfNecessary();
            AppendEmptyLineIf(AddEmptyLineBeforeCodeBlock);

            AddState(State.FencedCodeBlock);

            AppendCodeFence();
            AppendSyntax(info);
            AppendLine();

            AppendRaw(code);
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
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(CodeFenceStyle), nameof(CodeFenceStyle));
            }
        }

        public MarkdownBuilder AppendQuoteBlock(object value)
        {
            return AppendQuoteBlockCore(value);
        }

        public MarkdownBuilder AppendQuoteBlock(params object[] values)
        {
            return AppendQuoteBlockCore(null, values);
        }

        private MarkdownBuilder AppendQuoteBlockCore(object value, params object[] additionalValues)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            QuoteLevel++;
            AddState(State.QuoteBlock);
            AppendRange(value, additionalValues);
            AppendLineIfNecessary();
            RemoveState(State.QuoteBlock);
            QuoteLevel--;
            return this;
        }

        public MarkdownBuilder AppendHorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            HorizontalRule.ThrowOnInvalidCount(count);

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

        public MarkdownBuilder AppendTable(IEnumerable<MElement> rows)
        {
            AddState(State.Table);

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    List<int> widths = (FormatTableHeader || FormatTableContent) ? CalculateWidths(rows) : null;

                    MElement header = en.Current;

                    AppendTableHeader(header, widths);

                    while (en.MoveNext())
                        AppendTableRow(en.Current, widths);
                }
            }

            RemoveState(State.Table);
            return this;
        }

        private List<int> CalculateWidths(IEnumerable<MElement> rows)
        {
            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    MarkdownBuilder mb = MarkdownBuilderCache.GetInstance(Format);
                    int index = 0;

                    MElement row = en.Current;

                    var widths = new List<int>();

                    if (FormatTableHeader)
                    {
                        if (row is MContainer container)
                        {
                            foreach (MElement content in container.Elements())
                            {
                                mb.Append(content);
                                widths.Add(mb.Length - index);
                                index = mb.Length;
                            }
                        }
                        else
                        {
                            mb.Append(row);
                            widths.Add(mb.Length);
                        }
                    }
                    else
                    {
                        widths.Add(0);

                        while (en.MoveNext())
                            widths.Add(0);
                    }

                    if (FormatTableContent)
                    {
                        mb.Clear();
                        index = 0;

                        while (en.MoveNext())
                        {
                            int columnCount = widths.Count;

                            row = en.Current;

                            if (row is MContainer container)
                            {
                                int i = 0;
                                foreach (MElement cell in container.Elements())
                                {
                                    mb.Append(cell);
                                    widths[i] = Math.Max(widths[i], mb.Length - index);
                                    index = mb.Length;
                                    i++;

                                    if (i == columnCount)
                                        break;
                                }
                            }
                            else
                            {
                                mb.Append(row);
                                widths[0] = Math.Max(widths[0], mb.Length - index);
                                index = mb.Length;
                            }
                        }
                    }

                    MarkdownBuilderCache.Free(mb);

                    return widths;
                }
            }

            return null;
        }

        internal void AppendTableHeader(MElement content, IList<int> widths = null)
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
            AppendTableHeaderSeparator(content, widths);

            void AppendCell(MElement cellContent)
            {
                Alignment alignment = (cellContent as TableColumn)?.Alignment ?? Alignment.Left;

                AppendTableCellStart(isFirst, isLast);

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

                    AppendPadRight(width, widths?[i], minimalWidth);

                    if (!TablePadding
                        && alignment != Alignment.Left)
                    {
                        AppendSpace();
                    }
                }

                AppendTableCellEnd(isLast);
            }
        }

        private void AppendTableHeaderSeparator(MElement content, IList<int> widths = null)
        {
            bool isFirst = true;
            bool isLast = true;

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

            void AppendCell(MElement column)
            {
                Alignment alignment = (column as TableColumn)?.Alignment ?? Alignment.Left;

                AppendTableCellStart(isFirst, isLast);

                if (alignment == Alignment.Center)
                {
                    AppendSyntax(":");
                }
                else
                {
                    AppendTablePadding();
                }

                AppendSyntax("---");

                if (FormatTableHeader)
                    AppendPadRight(3, widths?[i], 3, '-');

                if (alignment != Alignment.Left)
                {
                    AppendSyntax(":");
                }
                else
                {
                    AppendTablePadding();
                }

                if (isLast
                    && TableOuterDelimiter)
                {
                    AppendTableDelimiter();
                }
            }
        }

        internal MarkdownBuilder AppendTableRow(MElement content, List<int> widths = null)
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
                    AppendPadRight(length, widths?[i]);

                AppendTableCellEnd(isLast);
            }
        }

        private void AppendTableCellStart(bool isFirst, bool isLast)
        {
            if (isFirst)
            {
                AppendLineStart();

                if (TableOuterDelimiter
                    || isLast)
                {
                    AppendTableDelimiter();
                }
            }
            else
            {
                AppendTableDelimiter();
            }
        }

        private void AppendTableCellEnd(bool isLast, bool isBlankCell = false)
        {
            if (isLast)
            {
                if (TableOuterDelimiter)
                {
                    AppendTablePadding();
                    AppendTableDelimiter();
                }
                else if (isBlankCell)
                {
                    AppendTableDelimiter();
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

        internal void AppendTableDelimiter()
        {
            AppendSyntax(TableDelimiter);
        }

        private void AppendPadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
            {
                AppendRaw(paddingChar);
            }
        }

        public MarkdownBuilder AppendHtmlEntity(int number)
        {
            AppendSyntax("&#");

            if (HtmlEntityFormat == HtmlEntityFormat.Hexadecimal)
            {
                AppendSyntax("x");
                AppendRaw(number.ToString("x", CultureInfo.InvariantCulture));
            }
            else if (HtmlEntityFormat == HtmlEntityFormat.Decimal)
            {
                AppendRaw(number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new ArgumentException(ErrorMessages.UnknownEnumValue(HtmlEntityFormat), nameof(HtmlEntityFormat));
            }

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

        private MarkdownBuilder AppendDelimiter(State state, string delimiter, string value)
        {
            AddState(state);
            AppendSyntax(delimiter);
            Append(value);
            AppendSyntax(delimiter);
            RemoveState(state);
            return this;
        }

        private MarkdownBuilder AppendDelimiter(State state, string delimiter, object value, params object[] additionalValues)
        {
            AddState(state);
            AppendSyntax(delimiter);
            AppendRange(value, additionalValues);
            AppendSyntax(delimiter);
            RemoveState(state);
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

        public MarkdownBuilder Append(EmphasisOption option, params object[] content)
        {
            return Append(option, content);
        }

        public MarkdownBuilder Append(EmphasisOption option, object content)
        {
            string delimiter = GetDelimiter();

            State state = option.ToState();

            AddState(state);
            AppendSyntax(delimiter);
            Append(content);
            AppendSyntax(delimiter);
            RemoveState(state);
            return this;

            string GetDelimiter()
            {
                switch (option)
                {
                    case EmphasisOption.Bold:
                        return BoldDelimiter;
                    case EmphasisOption.Italic:
                        return ItalicDelimiter;
                    case EmphasisOption.Strikethrough:
                        return StrikethroughDelimiter;
                    default:
                        throw new ArgumentException(ErrorMessages.UnknownEnumValue(option), nameof(option));
                }
            }
        }

        internal virtual MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
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
                    AppendLinefeed(0, i);
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
                            AppendLinefeed(lastIndex, i);
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

                    StringBuilder.Append(value, lastIndex, value.Length - lastIndex);
                    return this;
                }
            }

            StringBuilder.Append(value);
            return this;

            void AppendLinefeed(int startIndex, int index)
            {
                index--;
                if (index > 0
                    && value[index - 1] == '\r')
                {
                    index--;
                }

                BeforeAppend();
                StringBuilder.Append(value, startIndex, index - startIndex);
                AppendLine();
            }

            void AppendEscapedChar(int startIndex, int index, char ch)
            {
                BeforeAppend();
                StringBuilder.Append(value, startIndex, index - startIndex);
                StringBuilder.Append(escapingChar);
                StringBuilder.Append(ch);
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

        private MarkdownBuilder AppendRange(object value, params object[] additionalValues)
        {
            Append(value);
            AppendRange(additionalValues);
            return this;
        }

        internal MarkdownBuilder AppendRange(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (object value in values)
                Append(value);

            return this;
        }

        internal MarkdownBuilder AppendRange(IEnumerable<MElement> elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            foreach (MElement element in elements)
                element.AppendTo(this);

            return this;
        }

        private void AppendLineStart(
            State state = State.None,
            bool addEmptyLine = false,
            string prefix1 = null,
            string prefix2 = null)
        {
            AppendLineIfNecessary();
            AppendEmptyLineIf(addEmptyLine);

            AddState(state);

            AppendSyntax(prefix1);
            AppendSyntax(prefix2);
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

        private void AppendLine(bool addEmptyLine)
        {
            AppendLineIfNecessary();

            if (addEmptyLine)
                AppendEmptyLine();
        }

        private void AppendLineIfNecessary()
        {
            int length = Length;

            if (length == 0)
                return;

            if (this[length - 1] != '\n')
                AppendLine();
        }

        private void AppendEmptyLineIf(bool condition)
        {
            if (condition)
                AppendEmptyLine();
        }

        private void AppendEmptyLine()
        {
            int length = Length;

            if (length == 0)
                return;

            int index = length - 1;

            char ch = this[index];

            if (ch == '\n'
                && --index >= 0)
            {
                ch = this[index];

                if (ch == '\r')
                {
                    if (--index >= 0)
                    {
                        ch = this[index];

                        if (ch == '\n')
                            return;
                    }
                }
                else if (ch == '\n')
                {
                    return;
                }
            }

            AppendLine();
        }

        private MarkdownBuilder AppendRaw(char value)
        {
            Debug.Assert(value != '\r' && value != '\n', value.ToString());

            BeforeAppend();
            StringBuilder.Append(value);
            return this;
        }

        private MarkdownBuilder AppendRaw(char value, int repeatCount)
        {
            Debug.Assert(value != '\r' && value != '\n', value.ToString());

            BeforeAppend();
            StringBuilder.Append(value, repeatCount);
            return this;
        }

        private MarkdownBuilder AppendSyntax(string value)
        {
            BeforeAppend();
            StringBuilder.Append(value);
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
            StringBuilder.Append(value);
            return this;
        }

        public MarkdownBuilder AppendLine()
        {
            RemoveState(State.PendingEmptyLine);
            StringBuilder.AppendLine();
            AddState(State.StartOfLine);
            return this;
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
                RemoveState(State.StartOfLine);
            }
        }

        private void AppendIndentation()
        {
            for (int i = 1; i <= QuoteLevel; i++)
                StringBuilder.Append(QuoteBlockStart);

            if (HasState(State.ListItem | State.OrderedListItem | State.TaskListItem))
                StringBuilder.Append("  ");

            if (HasState(State.IndentedCodeBlock))
                StringBuilder.Append("    ");
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
            return ToString(addSignature: false);
        }

        internal string ToString(bool addSignature)
        {
            if (Length > 0
                && addSignature)
            {
                AppendLine(addEmptyLine: true);
                AppendComment("Generated with MarkdownBuilder (http://www.github.com/josefpihrt/markdownbuilder)");
            }

            return StringBuilder.ToString();
        }
    }
}