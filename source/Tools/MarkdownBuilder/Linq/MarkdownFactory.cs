// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown.Linq
{
    //TODO: MFactory
    public static class MarkdownFactory
    {
        public static MDocument Document()
        {
            return new MDocument();
        }

        public static MDocument Document(object content)
        {
            return new MDocument(content);
        }

        public static MDocument Document(params object[] content)
        {
            return new MDocument(content);
        }

        public static MDocument Document(MDocument other)
        {
            return new MDocument(other);
        }

        public static RawText RawText(string text)
        {
            return new RawText(text);
        }

        public static RawText RawText(RawText other)
        {
            return new RawText(other);
        }

        public static Bold Bold()
        {
            return new Bold();
        }

        public static Bold Bold(object content)
        {
            return new Bold(content);
        }

        public static Bold Bold(params object[] content)
        {
            return new Bold(content);
        }

        public static Bold Bold(Bold other)
        {
            return new Bold(other);
        }

        public static Italic Italic()
        {
            return new Italic();
        }

        public static Italic Italic(object content)
        {
            return new Italic(content);
        }

        public static Italic Italic(params object[] content)
        {
            return new Italic(content);
        }

        public static Italic Italic(Italic other)
        {
            return new Italic(other);
        }

        public static Strikethrough Strikethrough()
        {
            return new Strikethrough();
        }

        public static Strikethrough Strikethrough(object content)
        {
            return new Strikethrough(content);
        }

        public static Strikethrough Strikethrough(params object[] content)
        {
            return new Strikethrough(content);
        }

        public static Strikethrough Strikethrough(Strikethrough other)
        {
            return new Strikethrough(other);
        }

        public static MInlineContainer BoldItalic(object content)
        {
            return Bold(Italic(content));
        }

        public static MInlineContainer BoldItalic(params object[] content)
        {
            return Bold(Italic(content));
        }

        public static InlineCode InlineCode(string text)
        {
            return new InlineCode(text);
        }

        public static InlineCode InlineCode(InlineCode other)
        {
            return new InlineCode(other);
        }

        public static MInlineContainer Inline()
        {
            return new MInlineContainer();
        }

        public static MInlineContainer Inline(object content)
        {
            return new MInlineContainer(content);
        }

        public static MInlineContainer Inline(params object[] content)
        {
            return new MInlineContainer(content);
        }

        public static MInlineContainer Inline(MInlineContainer other)
        {
            return new MInlineContainer(other);
        }

        public static MInlineContainer Join(object separator, params object[] values)
        {
            return Join(separator, (IEnumerable<MElement>)values);
        }

        public static MInlineContainer Join(object separator, IEnumerable<object> values)
        {
            return new MInlineContainer(GetContent());

            IEnumerable<object> GetContent()
            {
                bool addSeparator = false;

                foreach (object value in values)
                {
                    if (addSeparator)
                    {
                        yield return separator;
                    }
                    else
                    {
                        addSeparator = true;
                    }

                    yield return value;
                }
            }
        }

        public static Heading Heading(int level)
        {
            return new Heading(level);
        }

        public static Heading Heading(int level, object content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading(int level, params object[] content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading(Heading other)
        {
            return new Heading(other);
        }

        public static Heading Heading1()
        {
            return Heading(1);
        }

        public static Heading Heading1(object content)
        {
            return Heading(1, content);
        }

        public static Heading Heading1(params object[] content)
        {
            return Heading(1, content);
        }

        public static Heading Heading2()
        {
            return Heading(2);
        }

        public static Heading Heading2(object content)
        {
            return Heading(2, content);
        }

        public static Heading Heading2(params object[] content)
        {
            return Heading(2, content);
        }

        public static Heading Heading3()
        {
            return Heading(3);
        }

        public static Heading Heading3(object content)
        {
            return Heading(3, content);
        }

        public static Heading Heading3(params object[] content)
        {
            return Heading(3, content);
        }

        public static Heading Heading4()
        {
            return Heading(4);
        }

        public static Heading Heading4(object content)
        {
            return Heading(4, content);
        }

        public static Heading Heading4(params object[] content)
        {
            return Heading(4, content);
        }

        public static Heading Heading5()
        {
            return Heading(5);
        }

        public static Heading Heading5(object content)
        {
            return Heading(5, content);
        }

        public static Heading Heading5(params object[] content)
        {
            return Heading(5, content);
        }

        public static Heading Heading6()
        {
            return Heading(6);
        }

        public static Heading Heading6(object content)
        {
            return Heading(6, content);
        }

        public static Heading Heading6(params object[] content)
        {
            return Heading(6, content);
        }

        public static BulletItem BulletItem()
        {
            return new BulletItem();
        }

        public static BulletItem BulletItem(object content)
        {
            return new BulletItem(content);
        }

        public static BulletItem BulletItem(params object[] content)
        {
            return new BulletItem(content);
        }

        public static BulletItem BulletItem(BulletItem other)
        {
            return new BulletItem(other);
        }

        public static OrderedItem OrderedItem(int number)
        {
            return new OrderedItem(number);
        }

        public static OrderedItem OrderedItem(int number, object content)
        {
            return new OrderedItem(number, content);
        }

        public static OrderedItem OrderedItem(int number, params object[] content)
        {
            return new OrderedItem(number, content);
        }

        public static OrderedItem OrderedItem(OrderedItem other)
        {
            return new OrderedItem(other);
        }

        public static TaskItem TaskItem(bool isCompleted)
        {
            return new TaskItem(isCompleted);
        }

        public static TaskItem TaskItem(bool isCompleted, object content)
        {
            return new TaskItem(isCompleted, content);
        }

        public static TaskItem TaskItem(bool isCompleted, params object[] content)
        {
            return new TaskItem(isCompleted, content);
        }

        public static TaskItem TaskItem(TaskItem other)
        {
            return new TaskItem(other);
        }

        public static TaskItem CompletedTaskItem()
        {
            return TaskItem(isCompleted: true);
        }

        public static TaskItem CompletedTaskItem(object content)
        {
            return TaskItem(isCompleted: true, content: content);
        }

        public static TaskItem CompletedTaskItem(params object[] content)
        {
            return TaskItem(isCompleted: true, content: content);
        }

        public static BulletList List()
        {
            return new BulletList();
        }

        public static BulletList List(object content)
        {
            return new BulletList(content);
        }

        public static BulletList List(params object[] content)
        {
            return new BulletList(content);
        }

        public static BulletList List(BulletList other)
        {
            return new BulletList(other);
        }

        public static OrderedList OrderedList(int number)
        {
            return new OrderedList(number);
        }

        public static OrderedList OrderedList(int number, object content)
        {
            return new OrderedList(number, content);
        }

        public static OrderedList OrderedList(int number, params object[] content)
        {
            return new OrderedList(number, content);
        }

        public static OrderedList OrderedList(OrderedList other)
        {
            return new OrderedList(other);
        }

        public static TaskList TaskList(bool isCompleted)
        {
            return new TaskList(isCompleted);
        }

        public static TaskList TaskList(bool isCompleted, object content)
        {
            return new TaskList(isCompleted, content);
        }

        public static TaskList TaskList(bool isCompleted, params object[] content)
        {
            return new TaskList(isCompleted, content);
        }

        public static TaskList TaskList(TaskList other)
        {
            return new TaskList(other);
        }

        public static Image Image(string text, string url, string title = null)
        {
            return new Image(text, url, title);
        }

        public static Link Link(string text, string url, string title = null)
        {
            return new Link(text, url, title);
        }

        public static MElement LinkOrText(string text, string url = null, string title = null)
        {
            if (string.IsNullOrEmpty(url))
                return new MText(text);

            return new Link(text, url, title);
        }

        public static FencedCodeBlock FencedCodeBlock(string value, string info = null)
        {
            return new FencedCodeBlock(value, info);
        }

        public static IndentedCodeBlock IndentedCodeBlock(string value)
        {
            return new IndentedCodeBlock(value);
        }

        public static BlockQuote BlockQuote()
        {
            return new BlockQuote();
        }

        public static BlockQuote BlockQuote(object content)
        {
            return new BlockQuote(content);
        }

        public static BlockQuote BlockQuote(params object[] content)
        {
            return new BlockQuote(content);
        }

        public static BlockQuote BlockQuote(BlockQuote other)
        {
            return new BlockQuote(other);
        }

        public static HorizontalRule HorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            return new HorizontalRule(style, count, space);
        }

        public static CharReference CharReference(int number)
        {
            return new CharReference(number);
        }

        public static EntityReference EntityReference(string name)
        {
            return new EntityReference(name);
        }

        public static Table Table()
        {
            return new Table();
        }

        public static Table Table(object content)
        {
            return new Table(content);
        }

        public static Table Table(params object[] content)
        {
            return new Table(content);
        }

        public static Table Table(Table other)
        {
            return new Table(other);
        }

        public static TableColumn TableColumn(Alignment alignment)
        {
            return new TableColumn(alignment);
        }

        public static TableColumn TableColumn(Alignment alignment, object content)
        {
            return new TableColumn(alignment, content);
        }

        public static TableColumn TableColumn(Alignment alignment, params object[] content)
        {
            return new TableColumn(alignment, content);
        }

        public static TableColumn TableColumn(TableColumn other)
        {
            return new TableColumn(other);
        }

        public static TableRow TableRow()
        {
            return new TableRow();
        }

        public static TableRow TableRow(object content)
        {
            return new TableRow(content);
        }

        public static TableRow TableRow(params object[] content)
        {
            return new TableRow(content);
        }

        public static TableRow TableRow(TableRow other)
        {
            return new TableRow(other);
        }

        internal static string StrikethroughDelimiter => "~~";

        internal static string CodeDelimiter => "`";

        internal static char CodeDelimiterChar => '`';

        internal static string TableDelimiter => "|";

        internal static string BlockQuoteStart => "> ";

        internal static string BoldDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "**";

            if (style == EmphasisStyle.Underscore)
                return "__";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        internal static string ItalicDelimiter(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return "*";

            if (style == EmphasisStyle.Underscore)
                return "_";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        internal static char HeadingStartChar(HeadingStyle style)
        {
            switch (style)
            {
                case HeadingStyle.NumberSign:
                    return '#';
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
            }
        }

        internal static string BulletItemStart(BulletListStyle style)
        {
            if (style == BulletListStyle.Asterisk)
                return "* ";

            if (style == BulletListStyle.Plus)
                return "+ ";

            if (style == BulletListStyle.Minus)
                return "- ";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        internal static string OrderedItemStart(OrderedListStyle style)
        {
            switch (style)
            {
                case OrderedListStyle.Dot:
                    return ". ";
                case OrderedListStyle.Parenthesis:
                    return ") ";
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
            }
        }

        internal static string TaskItemStart(bool isCompleted = false)
        {
            if (isCompleted)
            {
                return "- [x] ";
            }
            else
            {
                return "- [ ] ";
            }
        }

        internal static char HorizontalRuleChar(HorizontalRuleStyle style)
        {
            switch (style)
            {
                case HorizontalRuleStyle.Hyphen:
                    return '-';
                case HorizontalRuleStyle.Asterisk:
                    return '*';
                case HorizontalRuleStyle.Underscore:
                    return '_';
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
            }
        }
    }
}
