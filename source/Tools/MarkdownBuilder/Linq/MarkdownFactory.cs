// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown.Linq
{
    public static class MarkdownFactory
    {
        public static RawText RawText(string text)
        {
            return new RawText(text);
        }

        public static Bold Bold(object content)
        {
            return new Bold(content);
        }

        public static Bold Bold(params object[] content)
        {
            return new Bold(content);
        }

        public static Italic Italic(object content)
        {
            return new Italic(content);
        }

        public static Italic Italic(params object[] content)
        {
            return new Italic(content);
        }

        public static MInlineContainer BoldItalic(object content)
        {
            return Bold(Italic(content));
        }

        public static MInlineContainer BoldItalic(params object[] content)
        {
            return Bold(Italic(content));
        }

        public static Strikethrough Strikethrough(object content)
        {
            return new Strikethrough(content);
        }

        public static Strikethrough Strikethrough(params object[] content)
        {
            return new Strikethrough(content);
        }

        public static InlineCode Code(string text)
        {
            return new InlineCode(text);
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

        public static Heading Heading(int level, object content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading(int level, params object[] content)
        {
            return new Heading(level, content);
        }

        public static Heading Heading1(object content)
        {
            return Heading(1, content);
        }

        public static Heading Heading1(params object[] content)
        {
            return Heading(1, content);
        }

        public static Heading Heading2(object content)
        {
            return Heading(2, content);
        }

        public static Heading Heading2(params object[] content)
        {
            return Heading(2, content);
        }

        public static Heading Heading3(object content)
        {
            return Heading(3, content);
        }

        public static Heading Heading3(params object[] content)
        {
            return Heading(3, content);
        }

        public static Heading Heading4(object content)
        {
            return Heading(4, content);
        }

        public static Heading Heading4(params object[] content)
        {
            return Heading(4, content);
        }

        public static Heading Heading5(object content)
        {
            return Heading(5, content);
        }

        public static Heading Heading5(params object[] content)
        {
            return Heading(5, content);
        }

        public static Heading Heading6(object content)
        {
            return Heading(6, content);
        }

        public static Heading Heading6(params object[] content)
        {
            return Heading(6, content);
        }

        public static ListItem ListItem(object content)
        {
            return new ListItem(content);
        }

        public static ListItem ListItem(params object[] content)
        {
            return new ListItem(content);
        }

        public static OrderedListItem OrderedListItem(int number, object content)
        {
            return new OrderedListItem(number, content);
        }

        public static OrderedListItem OrderedListItem(int number, params object[] content)
        {
            return new OrderedListItem(number, content);
        }

        public static TaskListItem TaskListItem(bool isCompleted, object content)
        {
            return new TaskListItem(isCompleted, content);
        }

        public static TaskListItem TaskListItem(bool isCompleted, params object[] content)
        {
            return new TaskListItem(isCompleted, content);
        }

        public static TaskListItem CompletedTaskListItem(object content)
        {
            return TaskListItem(isCompleted: true, content: content);
        }

        public static TaskListItem CompletedTaskListItem(params object[] content)
        {
            return TaskListItem(isCompleted: true, content: content);
        }

        public static List List(object content)
        {
            return new List(content);
        }

        public static List List(params object[] content)
        {
            return new List(content);
        }

        public static OrderedList OrderedList(int number, object content)
        {
            return new OrderedList(number, content);
        }

        public static OrderedList OrderedList(int number, params object[] content)
        {
            return new OrderedList(number, content);
        }

        public static TaskList TaskList(bool isCompleted, object content)
        {
            return new TaskList(isCompleted, content);
        }

        public static TaskList TaskList(bool isCompleted, params object[] content)
        {
            return new TaskList(isCompleted, content);
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

        public static BlockQuote BlockQuote(object content)
        {
            return new BlockQuote(content);
        }

        public static BlockQuote BlockQuote(params object[] content)
        {
            return new BlockQuote(content);
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

        public static Table Table(object content)
        {
            return new Table(content);
        }

        public static Table Table(params object[] content)
        {
            return new Table(content);
        }

        public static TableColumn TableColumn(Alignment alignment, object content)
        {
            return new TableColumn(alignment, content);
        }

        public static TableColumn TableColumn(Alignment alignment, params object[] content)
        {
            return new TableColumn(alignment, content);
        }

        public static TableRow TableRow(object content)
        {
            return new TableRow(content);
        }

        public static TableRow TableRow(params object[] content)
        {
            return new TableRow(content);
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

        internal static string ListItemStart(ListStyle style)
        {
            if (style == ListStyle.Asterisk)
                return "* ";

            if (style == ListStyle.Plus)
                return "+ ";

            if (style == ListStyle.Minus)
                return "- ";

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        internal static string OrderedListItemStart(OrderedListStyle style)
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

        internal static string TaskListItemStart(bool isCompleted = false)
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
