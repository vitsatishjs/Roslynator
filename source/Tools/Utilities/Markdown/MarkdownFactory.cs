// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator.Utilities.Markdown
{
    public static class MarkdownFactory
    {
        public static MarkdownText Text(string text)
        {
            return new MarkdownText(text, escape: true);
        }

        public static MarkdownText RawText(string text)
        {
            return new MarkdownText(text, escape: false);
        }

        public static BoldText Bold(string text)
        {
            return new BoldText(text);
        }

        public static ItalicText Italic(string text)
        {
            return new ItalicText(text);
        }

        public static StrikethroughText Strikethrough(string text)
        {
            return new StrikethroughText(text);
        }

        public static MarkdownJoin Join(string separator, IEnumerable<object> values, bool escape = true)
        {
            return new MarkdownJoin(separator, values, escape);
        }

        public static Header Header(string text, int level)
        {
            return new Header(text, level);
        }

        public static Header Header1(string text = null)
        {
            return new Header(text, 1);
        }

        public static Header Header2(string text = null)
        {
            return new Header(text, 2);
        }

        public static Header Header3(string text = null)
        {
            return new Header(text, 3);
        }

        public static Header Header4(string text = null)
        {
            return new Header(text, 4);
        }

        public static Header Header5(string text = null)
        {
            return new Header(text, 5);
        }

        public static Header Header6(string text = null)
        {
            return new Header(text, 6);
        }

        public static TableHeader TableHeader(string name, Alignment alignment = Alignment.Left)
        {
            return new TableHeader(name, alignment);
        }

        public static ListItem ListItem(string value = null)
        {
            return new ListItem(value);
        }

        public static OrderedListItem OrderedListItem(int number, string value = null)
        {
            return new OrderedListItem(number, value);
        }

        public static TaskListItem TaskListItem(string value = null)
        {
            return new TaskListItem(value);
        }

        public static TaskListItem CompletedTaskListItem(string value = null)
        {
            return new TaskListItem(value, isCompleted: true);
        }

        public static Link Link(string text, string url)
        {
            return new Link(text, url);
        }

        public static Image Image(string text, string url)
        {
            return new Image(text, url);
        }

        public static InlineCode InlineCode(string text)
        {
            return new InlineCode(text);
        }

        public static CodeBlock CodeBlock(string text, string language = null)
        {
            return new CodeBlock(text, language);
        }

        public static BlockQuote BlockQuote(string text)
        {
            return new BlockQuote(text);
        }

        public static MarkdownText HorizontalRule()
        {
            return RawText(MarkdownSettings.Default.HorizontalRule);
        }
    }
}
