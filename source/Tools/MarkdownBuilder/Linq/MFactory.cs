﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Markdown.Linq
{
    public static class MFactory
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

        public static MRaw RawText(string text)
        {
            return new MRaw(text);
        }

        public static MRaw RawText(MRaw other)
        {
            return new MRaw(other);
        }

        public static MBold Bold()
        {
            return new MBold();
        }

        public static MBold Bold(object content)
        {
            return new MBold(content);
        }

        public static MBold Bold(params object[] content)
        {
            return new MBold(content);
        }

        public static MBold Bold(MBold other)
        {
            return new MBold(other);
        }

        public static MItalic Italic()
        {
            return new MItalic();
        }

        public static MItalic Italic(object content)
        {
            return new MItalic(content);
        }

        public static MItalic Italic(params object[] content)
        {
            return new MItalic(content);
        }

        public static MItalic Italic(MItalic other)
        {
            return new MItalic(other);
        }

        public static MStrikethrough Strikethrough()
        {
            return new MStrikethrough();
        }

        public static MStrikethrough Strikethrough(object content)
        {
            return new MStrikethrough(content);
        }

        public static MStrikethrough Strikethrough(params object[] content)
        {
            return new MStrikethrough(content);
        }

        public static MStrikethrough Strikethrough(MStrikethrough other)
        {
            return new MStrikethrough(other);
        }

        public static MInlineContainer BoldItalic(object content)
        {
            return Bold(Italic(content));
        }

        public static MInlineContainer BoldItalic(params object[] content)
        {
            return Bold(Italic(content));
        }

        public static MInlineCode InlineCode(string text)
        {
            return new MInlineCode(text);
        }

        public static MInlineCode InlineCode(MInlineCode other)
        {
            return new MInlineCode(other);
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

        public static MHeading Heading(int level)
        {
            return new MHeading(level);
        }

        public static MHeading Heading(int level, object content)
        {
            return new MHeading(level, content);
        }

        public static MHeading Heading(int level, params object[] content)
        {
            return new MHeading(level, content);
        }

        public static MHeading Heading(MHeading other)
        {
            return new MHeading(other);
        }

        public static MHeading Heading1()
        {
            return Heading(1);
        }

        public static MHeading Heading1(object content)
        {
            return Heading(1, content);
        }

        public static MHeading Heading1(params object[] content)
        {
            return Heading(1, content);
        }

        public static MHeading Heading2()
        {
            return Heading(2);
        }

        public static MHeading Heading2(object content)
        {
            return Heading(2, content);
        }

        public static MHeading Heading2(params object[] content)
        {
            return Heading(2, content);
        }

        public static MHeading Heading3()
        {
            return Heading(3);
        }

        public static MHeading Heading3(object content)
        {
            return Heading(3, content);
        }

        public static MHeading Heading3(params object[] content)
        {
            return Heading(3, content);
        }

        public static MHeading Heading4()
        {
            return Heading(4);
        }

        public static MHeading Heading4(object content)
        {
            return Heading(4, content);
        }

        public static MHeading Heading4(params object[] content)
        {
            return Heading(4, content);
        }

        public static MHeading Heading5()
        {
            return Heading(5);
        }

        public static MHeading Heading5(object content)
        {
            return Heading(5, content);
        }

        public static MHeading Heading5(params object[] content)
        {
            return Heading(5, content);
        }

        public static MHeading Heading6()
        {
            return Heading(6);
        }

        public static MHeading Heading6(object content)
        {
            return Heading(6, content);
        }

        public static MHeading Heading6(params object[] content)
        {
            return Heading(6, content);
        }

        public static MBulletItem BulletItem()
        {
            return new MBulletItem();
        }

        public static MBulletItem BulletItem(object content)
        {
            return new MBulletItem(content);
        }

        public static MBulletItem BulletItem(params object[] content)
        {
            return new MBulletItem(content);
        }

        public static MBulletItem BulletItem(MBulletItem other)
        {
            return new MBulletItem(other);
        }

        public static MOrderedItem OrderedItem(int number)
        {
            return new MOrderedItem(number);
        }

        public static MOrderedItem OrderedItem(int number, object content)
        {
            return new MOrderedItem(number, content);
        }

        public static MOrderedItem OrderedItem(int number, params object[] content)
        {
            return new MOrderedItem(number, content);
        }

        public static MOrderedItem OrderedItem(MOrderedItem other)
        {
            return new MOrderedItem(other);
        }

        public static MTaskItem TaskItem(bool isCompleted)
        {
            return new MTaskItem(isCompleted);
        }

        public static MTaskItem TaskItem(bool isCompleted, object content)
        {
            return new MTaskItem(isCompleted, content);
        }

        public static MTaskItem TaskItem(bool isCompleted, params object[] content)
        {
            return new MTaskItem(isCompleted, content);
        }

        public static MTaskItem TaskItem(MTaskItem other)
        {
            return new MTaskItem(other);
        }

        public static MTaskItem CompletedTaskItem()
        {
            return TaskItem(isCompleted: true);
        }

        public static MTaskItem CompletedTaskItem(object content)
        {
            return TaskItem(isCompleted: true, content: content);
        }

        public static MTaskItem CompletedTaskItem(params object[] content)
        {
            return TaskItem(isCompleted: true, content: content);
        }

        public static MBulletList List()
        {
            return new MBulletList();
        }

        public static MBulletList List(object content)
        {
            return new MBulletList(content);
        }

        public static MBulletList List(params object[] content)
        {
            return new MBulletList(content);
        }

        public static MBulletList List(MBulletList other)
        {
            return new MBulletList(other);
        }

        public static MOrderedList OrderedList(int number)
        {
            return new MOrderedList(number);
        }

        public static MOrderedList OrderedList(int number, object content)
        {
            return new MOrderedList(number, content);
        }

        public static MOrderedList OrderedList(int number, params object[] content)
        {
            return new MOrderedList(number, content);
        }

        public static MOrderedList OrderedList(MOrderedList other)
        {
            return new MOrderedList(other);
        }

        public static MTaskList TaskList(bool isCompleted)
        {
            return new MTaskList(isCompleted);
        }

        public static MTaskList TaskList(bool isCompleted, object content)
        {
            return new MTaskList(isCompleted, content);
        }

        public static MTaskList TaskList(bool isCompleted, params object[] content)
        {
            return new MTaskList(isCompleted, content);
        }

        public static MTaskList TaskList(MTaskList other)
        {
            return new MTaskList(other);
        }

        public static MImage Image(string text, string url, string title = null)
        {
            return new MImage(text, url, title);
        }

        public static MLink Link(string text, string url, string title = null)
        {
            return new MLink(text, url, title);
        }

        public static MElement LinkOrText(string text, string url = null, string title = null)
        {
            if (!string.IsNullOrEmpty(url))
                return new MLink(text, url, title);

            return new MText(text);
        }

        public static MFencedCodeBlock FencedCodeBlock(string value, string info = null)
        {
            return new MFencedCodeBlock(value, info);
        }

        public static MIndentedCodeBlock IndentedCodeBlock(string value)
        {
            return new MIndentedCodeBlock(value);
        }

        public static MBlockQuote BlockQuote()
        {
            return new MBlockQuote();
        }

        public static MBlockQuote BlockQuote(object content)
        {
            return new MBlockQuote(content);
        }

        public static MBlockQuote BlockQuote(params object[] content)
        {
            return new MBlockQuote(content);
        }

        public static MBlockQuote BlockQuote(MBlockQuote other)
        {
            return new MBlockQuote(other);
        }

        public static MHorizontalRule HorizontalRule(string value = "-", int count = 3, string separator = " ")
        {
            return new MHorizontalRule(value, count, separator);
        }

        public static MCharReference CharReference(char value)
        {
            return new MCharReference(value);
        }

        public static MEntityReference EntityReference(string name)
        {
            return new MEntityReference(name);
        }

        public static MTable Table()
        {
            return new MTable();
        }

        public static MTable Table(object content)
        {
            return new MTable(content);
        }

        public static MTable Table(params object[] content)
        {
            return new MTable(content);
        }

        public static MTable Table(MTable other)
        {
            return new MTable(other);
        }

        public static MTableColumn TableColumn(Alignment alignment)
        {
            return new MTableColumn(alignment);
        }

        public static MTableColumn TableColumn(Alignment alignment, object content)
        {
            return new MTableColumn(alignment, content);
        }

        public static MTableColumn TableColumn(Alignment alignment, params object[] content)
        {
            return new MTableColumn(alignment, content);
        }

        public static MTableColumn TableColumn(MTableColumn other)
        {
            return new MTableColumn(other);
        }

        public static MTableRow TableRow()
        {
            return new MTableRow();
        }

        public static MTableRow TableRow(object content)
        {
            return new MTableRow(content);
        }

        public static MTableRow TableRow(params object[] content)
        {
            return new MTableRow(content);
        }

        public static MTableRow TableRow(MTableRow other)
        {
            return new MTableRow(other);
        }
    }
}
