// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class MarkdownFactoryTests
    {
        [TestMethod]
        public void TextTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, Text(x).Text);
            Assert.AreEqual(EmphasisOptions.None, Text(x).Options);
            Assert.AreEqual(true, Text(x).Escape);

            Assert.AreEqual(x, Text(x, EmphasisOptions.Bold).Text);
            Assert.AreEqual(EmphasisOptions.Bold, Text(x, EmphasisOptions.Bold).Options);
            Assert.AreEqual(true, Text(x, EmphasisOptions.Bold).Escape);

            Assert.AreEqual(x, Text(x, EmphasisOptions.BoldItalic).Text);
            Assert.AreEqual(EmphasisOptions.BoldItalic, Text(x, EmphasisOptions.BoldItalic).Options);
            Assert.AreEqual(true, Text(x, EmphasisOptions.BoldItalic).Escape);
        }

        [TestMethod]
        public void RawTextTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, RawText(x).Text);
            Assert.AreEqual(EmphasisOptions.None, RawText(x).Options);
            Assert.AreEqual(false, RawText(x).Escape);

            Assert.AreEqual(x, RawText(x, EmphasisOptions.Bold).Text);
            Assert.AreEqual(EmphasisOptions.Bold, RawText(x, EmphasisOptions.Bold).Options);
            Assert.AreEqual(false, RawText(x, EmphasisOptions.Bold).Escape);

            Assert.AreEqual(x, RawText(x, EmphasisOptions.BoldItalic).Text);
            Assert.AreEqual(EmphasisOptions.BoldItalic, RawText(x, EmphasisOptions.BoldItalic).Options);
            Assert.AreEqual(false, RawText(x, EmphasisOptions.BoldItalic).Escape);
        }

        [TestMethod]
        public void BoldTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, Bold(x).Text);
            Assert.AreEqual(EmphasisOptions.Bold, Bold(x).Options);
            Assert.AreEqual(true, Bold(x).Escape);
        }

        [DataTestMethod]
        [DataRow("**", EmphasisStyle.Asterisk)]
        [DataRow("__", EmphasisStyle.Underscore)]
        public void BoldDelimiterTest1(string syntax, EmphasisStyle style)
        {
            Assert.AreEqual(syntax, BoldDelimiter(style));
        }

        [TestMethod]
        public void ItalicTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, Italic(x).Text);
            Assert.AreEqual(EmphasisOptions.Italic, Italic(x).Options);
            Assert.AreEqual(true, Italic(x).Escape);
        }

        [DataTestMethod]
        [DataRow("*", EmphasisStyle.Asterisk)]
        [DataRow("_", EmphasisStyle.Underscore)]
        public void ItalicDelimiterTest1(string syntax, EmphasisStyle style)
        {
            Assert.AreEqual(syntax, ItalicDelimiter(style));
        }

        [TestMethod]
        public void StrikethroughTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, Strikethrough(x).Text);
            Assert.AreEqual(EmphasisOptions.Strikethrough, Strikethrough(x).Options);
            Assert.AreEqual(true, Strikethrough(x).Escape);
        }

        [TestMethod]
        public void CodeTest1()
        {
            const string x = "x";

            Assert.AreEqual(x, Code(x).Text);
            Assert.AreEqual(EmphasisOptions.Code, Code(x).Options);
            Assert.AreEqual(true, Code(x).Escape);
        }

        [TestMethod]
        public void JoinTest1()
        {
            const string separator = "separator";
            IEnumerable<string> values = Array.Empty<string>();

            Assert.AreEqual(separator, Join(separator, values).Separator);
            Assert.AreSame(values, Join(separator, values).Values);
            Assert.AreEqual(true, Join(separator, values).Escape);
            Assert.AreEqual(true, Join(separator, values, true).Escape);
            Assert.AreEqual(false, Join(separator, values, false).Escape);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        public void HeadingTest1(int level)
        {
            const string x = "x";

            Assert.AreEqual(x, Heading(x, level).Text);
            Assert.AreEqual(level, Heading(x, level).Level);

            Assert.AreEqual(new string('#', level) + " ", HeadingStart(level));
            Assert.AreEqual(" " + new string('#', level), HeadingEnd(level));
        }

        [TestMethod]
        public void HeadingTest2()
        {
            const string x = "x";

            Assert.AreEqual(x, Heading1(x).Text);
            Assert.AreEqual(1, Heading1(x).Level);
            Assert.AreEqual(x, Heading2(x).Text);
            Assert.AreEqual(2, Heading2(x).Level);
            Assert.AreEqual(x, Heading3(x).Text);
            Assert.AreEqual(3, Heading3(x).Level);
            Assert.AreEqual(x, Heading4(x).Text);
            Assert.AreEqual(4, Heading4(x).Level);
            Assert.AreEqual(x, Heading5(x).Text);
            Assert.AreEqual(5, Heading5(x).Level);
            Assert.AreEqual(x, Heading6(x).Text);
            Assert.AreEqual(6, Heading6(x).Level);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public void HeadingTest4(int level)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Heading("x", level));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => HeadingStart(level));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => HeadingEnd(level));
        }

        [TestMethod]
        public void ListItemTest1()
        {
            const string x = "x";
            Assert.AreEqual(x, MarkdownFactory.ListItem(x).Text);
        }

        [DataTestMethod]
        [DataRow("* ", ListItemStyle.Asterisk)]
        [DataRow("- ", ListItemStyle.Minus)]
        [DataRow("+ ", ListItemStyle.Plus)]
        public void ListItemStartTest1(string syntax, ListItemStyle style)
        {
            Assert.AreEqual(syntax, ListItemStart(style));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        public void OrderedListItemTest1(int number)
        {
            const string x = "x";
            Assert.AreEqual(x, OrderedListItem(number, x).Text);
            Assert.AreEqual(number, OrderedListItem(number).Number);
            Assert.AreEqual(number + ". ", OrderedListItemStart(number));
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TaskListItemTest1(bool isCompleted)
        {
            const string x = "x";
            Assert.AreEqual(x, MarkdownFactory.TaskListItem(x, isCompleted).Text);
            Assert.AreEqual(isCompleted, MarkdownFactory.TaskListItem(x, isCompleted).IsCompleted);

            Assert.AreEqual($"- [{((isCompleted) ? "x" : " ")}] ", TaskListItemStart(isCompleted));
        }

        [TestMethod]
        public void TaskListItemTest2()
        {
            const string x = "x";

            Assert.AreEqual(x, MarkdownFactory.TaskListItem(x).Text);
            Assert.AreEqual(false, MarkdownFactory.TaskListItem(x).IsCompleted);

            Assert.AreEqual(x, CompletedTaskListItem(x).Text);
            Assert.AreEqual(true, CompletedTaskListItem(x).IsCompleted);

            Assert.AreEqual("- [ ] ", TaskListItemStart());
        }

        [TestMethod]
        public void ImageTest1()
        {
            const string text = "Text";
            const string url = "Url";
            const string title = "Title";

            Image image = MarkdownFactory.Image(text, url);

            Assert.AreEqual(text, image.Text);
            Assert.AreEqual(url, image.Url);
            Assert.AreEqual(null, image.Title);

            image = MarkdownFactory.Image(text, url, title);

            Assert.AreEqual(text, image.Text);
            Assert.AreEqual(url, image.Url);
            Assert.AreEqual(title, image.Title);
        }

        [TestMethod]
        public void LinkTest1()
        {
            const string text = "Text";
            const string url = "Url";
            const string title = "Title";

            Link link = MarkdownFactory.Link(text, url);

            Assert.AreEqual(text, link.Text);
            Assert.AreEqual(url, link.Url);
            Assert.AreEqual(null, link.Title);

            link = MarkdownFactory.Link(text, url, title);

            Assert.AreEqual(text, link.Text);
            Assert.AreEqual(url, link.Url);
            Assert.AreEqual(title, link.Title);
        }

        [TestMethod]
        public void LinkOrImageTest()
        {
            const string text = "Text";
            const string url = "Url";

            Assert.IsInstanceOfType(LinkOrText(text, url), typeof(Link));

            Assert.IsInstanceOfType(LinkOrText(text), typeof(MarkdownText));
            Assert.IsInstanceOfType(LinkOrText(text, url: null), typeof(MarkdownText));
        }

        [TestMethod]
        public void CodeBlockTest1()
        {
            const string text = "x";
            const string language = LanguageIdentifiers.CSharp;

            CodeBlock cb = CodeBlock(text);

            Assert.AreEqual(text, cb.Text);
            Assert.AreEqual(null, cb.Language);

            cb = CodeBlock(text, language);

            Assert.AreEqual(text, cb.Text);
            Assert.AreEqual(language, cb.Language);
        }

        [TestMethod]
        public void QuoteBlockTest1()
        {
            const string text = "x";

            QuoteBlock cb = QuoteBlock(text);

            Assert.AreEqual(text, cb.Text);
        }

        [DataTestMethod]
        [DataRow('*', HorizontalRuleStyle.Asterisk)]
        [DataRow('-', HorizontalRuleStyle.Hyphen)]
        [DataRow('_', HorizontalRuleStyle.Underscore)]
        public void HorizontalRuleTest1(char syntax, HorizontalRuleStyle style)
        {
            for (int i = 3; i <= 5; i++)
            {
                Assert.AreEqual(style, HorizontalRule(style, count: i).Style);
                Assert.AreEqual(i, HorizontalRule(style, count: i).Count);
                Assert.AreEqual(true, HorizontalRule(style, count: i).AddSpaces);
                Assert.AreEqual(true, HorizontalRule(style, count: i, addSpaces: true).AddSpaces);
                Assert.AreEqual(false, HorizontalRule(style, count: i, addSpaces: false).AddSpaces);
            }

            Assert.AreEqual(syntax, HorizontalRuleChar(style));
        }

        [TestMethod]
        public void TableTest1()
        {
            TableColumn column1 = TableColumn("1");
            TableColumn column2 = TableColumn("2");
            TableColumn column3 = TableColumn("3");
            TableColumn column4 = TableColumn("4");
            TableColumn column5 = TableColumn("5");
            TableColumn column6 = TableColumn("6");

            Table t = Table();
            Assert.AreEqual(0, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);

            t = Table(Array.Empty<TableColumn>());
            Assert.AreEqual(0, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);

            t = MarkdownFactory.Table(column1);
            Assert.AreEqual(1, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);

            t = MarkdownFactory.Table(column1, column2);
            Assert.AreEqual(2, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);
            Assert.AreEqual(column2.Name, t.Columns[1].Name);

            t = MarkdownFactory.Table(column1, column2, column3);
            Assert.AreEqual(3, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);
            Assert.AreEqual(column2.Name, t.Columns[1].Name);
            Assert.AreEqual(column3.Name, t.Columns[2].Name);

            t = MarkdownFactory.Table(column1, column2, column3, column4);
            Assert.AreEqual(4, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);
            Assert.AreEqual(column2.Name, t.Columns[1].Name);
            Assert.AreEqual(column3.Name, t.Columns[2].Name);
            Assert.AreEqual(column4.Name, t.Columns[3].Name);

            t = MarkdownFactory.Table(column1, column2, column3, column4, column5);
            Assert.AreEqual(5, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);
            Assert.AreEqual(column2.Name, t.Columns[1].Name);
            Assert.AreEqual(column3.Name, t.Columns[2].Name);
            Assert.AreEqual(column4.Name, t.Columns[3].Name);
            Assert.AreEqual(column5.Name, t.Columns[4].Name);

            t = MarkdownFactory.Table(column1, column2, column3, column4, column5, column6);
            Assert.AreEqual(6, t.Columns.Count);
            Assert.AreEqual(0, t.Rows.Count);
            Assert.AreEqual(column1.Name, t.Columns[0].Name);
            Assert.AreEqual(column2.Name, t.Columns[1].Name);
            Assert.AreEqual(column3.Name, t.Columns[2].Name);
            Assert.AreEqual(column4.Name, t.Columns[3].Name);
            Assert.AreEqual(column5.Name, t.Columns[4].Name);
            Assert.AreEqual(column6.Name, t.Columns[5].Name);
        }

        [TestMethod]
        public void TableHeaderTest1()
        {
            TableColumn column1 = TableColumn("1");
            TableColumn column2 = TableColumn("2");
            TableColumn column3 = TableColumn("3");
            TableColumn column4 = TableColumn("4");
            TableColumn column5 = TableColumn("5");
            TableColumn column6 = TableColumn("6");

            TableColumnCollection th = TableHeader();
            Assert.AreEqual(0, th.Count);

            th = TableHeader(Array.Empty<TableColumn>());
            Assert.AreEqual(0, th.Count);

            th = TableHeader(column1);
            Assert.AreEqual(1, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);

            th = TableHeader(column1, column2);
            Assert.AreEqual(2, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);
            Assert.AreEqual(column2.Name, th[1].Name);

            th = TableHeader(column1, column2, column3);
            Assert.AreEqual(3, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);
            Assert.AreEqual(column2.Name, th[1].Name);
            Assert.AreEqual(column3.Name, th[2].Name);

            th = TableHeader(column1, column2, column3, column4);
            Assert.AreEqual(4, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);
            Assert.AreEqual(column2.Name, th[1].Name);
            Assert.AreEqual(column3.Name, th[2].Name);
            Assert.AreEqual(column4.Name, th[3].Name);

            th = TableHeader(column1, column2, column3, column4, column5);
            Assert.AreEqual(5, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);
            Assert.AreEqual(column2.Name, th[1].Name);
            Assert.AreEqual(column3.Name, th[2].Name);
            Assert.AreEqual(column4.Name, th[3].Name);
            Assert.AreEqual(column5.Name, th[4].Name);

            th = TableHeader(column1, column2, column3, column4, column5, column6);
            Assert.AreEqual(6, th.Count);
            Assert.AreEqual(column1.Name, th[0].Name);
            Assert.AreEqual(column2.Name, th[1].Name);
            Assert.AreEqual(column3.Name, th[2].Name);
            Assert.AreEqual(column4.Name, th[3].Name);
            Assert.AreEqual(column5.Name, th[4].Name);
            Assert.AreEqual(column6.Name, th[5].Name);
        }

        [DataTestMethod]
        [DataRow(Alignment.Left)]
        [DataRow(Alignment.Center)]
        [DataRow(Alignment.Right)]
        public void TableColumnTest1(Alignment alignment)
        {
            string name = alignment.ToString();
            Assert.AreEqual(name, MarkdownFactory.TableColumn(name, alignment).Name);
            Assert.AreEqual(alignment, MarkdownFactory.TableColumn(name, alignment).Alignment);
        }

        [TestMethod]
        public void TableRowTest1()
        {
            const string row1 = "1";
            const string row2 = "2";
            const string row3 = "3";
            const string row4 = "4";
            const string row5 = "5";

            List<object> row = TableRow(row1);
            Assert.AreEqual(1, row.Count);
            Assert.AreEqual(row1, row[0]);

            row = TableRow(row1, row2);
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(row1, row[0]);
            Assert.AreEqual(row2, row[1]);

            row = TableRow(row1, row2, row3);
            Assert.AreEqual(3, row.Count);
            Assert.AreEqual(row1, row[0]);
            Assert.AreEqual(row2, row[1]);
            Assert.AreEqual(row3, row[2]);

            row = TableRow(row1, row2, row3, row4);
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(row1, row[0]);
            Assert.AreEqual(row2, row[1]);
            Assert.AreEqual(row3, row[2]);
            Assert.AreEqual(row4, row[3]);

            row = TableRow(row1, row2, row3, row4, row5);
            Assert.AreEqual(5, row.Count);
            Assert.AreEqual(row1, row[0]);
            Assert.AreEqual(row2, row[1]);
            Assert.AreEqual(row3, row[2]);
            Assert.AreEqual(row4, row[3]);
            Assert.AreEqual(row5, row[4]);
        }
    }
}
