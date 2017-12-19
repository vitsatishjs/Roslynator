// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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

        public static BoldItalicText BoldItalic(string text)
        {
            return new BoldItalicText(text);
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

        public static string HeaderStart(int level)
        {
            switch (level)
            {
                case 1:
                    return "# ";
                case 2:
                    return "## ";
                case 3:
                    return "### ";
                case 4:
                    return "#### ";
                case 5:
                    return "##### ";
                case 6:
                    return "###### ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Header level cannot be less than 1 or greater than 6");
            }
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

        public static string OrderedListItemStart(int number)
        {
            switch (number)
            {
                case 1:
                    return "1. ";
                case 2:
                    return "2. ";
                case 3:
                    return "3. ";
                case 4:
                    return "4. ";
                case 5:
                    return "5. ";
                case 6:
                    return "6. ";
                case 7:
                    return "7. ";
                case 8:
                    return "8. ";
                case 9:
                    return "9. ";
                default:
                    return number.ToString() + ". ";
            }
        }

        public static TaskListItem TaskListItem(string value = null)
        {
            return new TaskListItem(value);
        }

        public static TaskListItem CompletedTaskListItem(string value = null)
        {
            return new TaskListItem(value, isCompleted: true);
        }

        public static string TaskListItemStart(bool isCompleted = false)
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

        public static Link Link(string text, string url)
        {
            return new Link(text, url);
        }

        public static Image Image(string text, string url)
        {
            return new Image(text, url);
        }

        public static CodeText Code(string text)
        {
            return new CodeText(text);
        }

        public static CodeBlock CodeBlock(string text, string language = null)
        {
            return new CodeBlock(text, language);
        }

        public static QuoteBlock QuoteBlock(string text)
        {
            return new QuoteBlock(text);
        }

        public static MarkdownText HorizontalRule()
        {
            return RawText(MarkdownSettings.Default.HorizontalRule);
        }
    }
}
