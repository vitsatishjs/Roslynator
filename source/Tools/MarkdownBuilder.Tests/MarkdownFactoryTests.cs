// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class MarkdownFactoryTests
    {
        [Fact]
        public void MarkdownFactory_Text_DefaultValues()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Text(text).Text);
            Assert.Equal(EmphasisOptions.None, MarkdownFactory.Text(text).Options);
        }

        [Theory]
        [InlineData(EmphasisOptions.None)]
        [InlineData(EmphasisOptions.Bold)]
        [InlineData(EmphasisOptions.BoldItalic)]
        public void MarkdownFactory_Text(EmphasisOptions options)
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Text(text, options).Text);
            Assert.Equal(options, MarkdownFactory.Text(text, options).Options);
        }

        [Fact]
        public void MarkdownFactory_RawText()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.RawText(text).Text);
        }

        [Fact]
        public void MarkdownFactory_Bold()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Bold(text).Text);
            Assert.Equal(EmphasisOptions.Bold, MarkdownFactory.Bold(text).Options);
        }

        [Theory]
        [InlineData("**", EmphasisStyle.Asterisk)]
        [InlineData("__", EmphasisStyle.Underscore)]
        public void MarkdownFactory_BoldDelimiter(string syntax, EmphasisStyle style)
        {
            Assert.Equal(syntax, MarkdownFactory.BoldDelimiter(style));
        }

        [Fact]
        public void MarkdownFactory_Italic()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Italic(text).Text);
            Assert.Equal(EmphasisOptions.Italic, MarkdownFactory.Italic(text).Options);
        }

        [Theory]
        [InlineData("*", EmphasisStyle.Asterisk)]
        [InlineData("_", EmphasisStyle.Underscore)]
        public void MarkdownFactory_ItalicDelimiter(string syntax, EmphasisStyle style)
        {
            Assert.Equal(syntax, MarkdownFactory.ItalicDelimiter(style));
        }

        [Fact]
        public void MarkdownFactory_Strikethrough()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Strikethrough(text).Text);
            Assert.Equal(EmphasisOptions.Strikethrough, MarkdownFactory.Strikethrough(text).Options);
        }

        [Fact]
        public void MarkdownFactory_Code()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Code(text).Text);
            Assert.Equal(EmphasisOptions.Code, MarkdownFactory.Code(text).Options);
        }

        [Fact]
        public void MarkdownFactory_Join_DefaultValues()
        {
            const string separator = "separator";
            IEnumerable<string> values = Array.Empty<string>();

            Assert.Equal(separator, MarkdownFactory.Join(separator, values).Separator);
            Assert.Same(values, MarkdownFactory.Join(separator, values).Values);
            Assert.True(MarkdownFactory.Join(separator, values).EscapeSeparator);
        }

        [Fact]
        public void MarkdownFactory_Join()
        {
            const string separator = "separator";
            IEnumerable<string> values = Array.Empty<string>();

            Assert.Equal(separator, MarkdownFactory.Join(separator, values, false).Separator);
            Assert.Same(values, MarkdownFactory.Join(separator, values, false).Values);
            Assert.True(MarkdownFactory.Join(separator, values, true).EscapeSeparator);
            Assert.False(MarkdownFactory.Join(separator, values, false).EscapeSeparator);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void MarkdownFactory_Heading(int level)
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading(text, level).Text);
            Assert.Equal(level, MarkdownFactory.Heading(text, level).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading1()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading1(text).Text);
            Assert.Equal(1, MarkdownFactory.Heading1(text).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading2()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading2(text).Text);
            Assert.Equal(2, MarkdownFactory.Heading2(text).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading3()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading3(text).Text);
            Assert.Equal(3, MarkdownFactory.Heading3(text).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading4()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading4(text).Text);
            Assert.Equal(4, MarkdownFactory.Heading4(text).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading5()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading5(text).Text);
            Assert.Equal(5, MarkdownFactory.Heading5(text).Level);
        }

        [Fact]
        public void MarkdownFactory_Heading6()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.Heading6(text).Text);
            Assert.Equal(6, MarkdownFactory.Heading6(text).Level);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        public void MarkdownFactory_Heading_Throws(int level)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MarkdownFactory.Heading("x", level));
        }

        [Fact]
        public void MarkdownFactory_ListItem()
        {
            string text = MarkdownTextText();
            Assert.Equal(text, MarkdownFactory.ListItem(text).Text);
        }

        [Theory]
        [InlineData("* ", ListItemStyle.Asterisk)]
        [InlineData("- ", ListItemStyle.Minus)]
        [InlineData("+ ", ListItemStyle.Plus)]
        public void MarkdownFactory_ListItemStart(string syntax, ListItemStyle style)
        {
            Assert.Equal(syntax, MarkdownFactory.ListItemStart(style));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void MarkdownFactory_OrderedListItem(int number)
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.OrderedListItem(number, text).Text);
            Assert.Equal(number, MarkdownFactory.OrderedListItem(number, null).Number);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void MarkdownFactory_OrderedListItemStart(int number)
        {
            string text = MarkdownTextText();

            Assert.Equal(number + ". ", MarkdownFactory.OrderedListItemStart(number));
        }

        [Fact]
        public void MarkdownFactory_TaskListItem_DefaultValues()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.TaskListItem(text).Text);
            Assert.False(MarkdownFactory.TaskListItem(text).IsCompleted);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MarkdownFactory_TaskListItem(bool isCompleted)
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.TaskListItem(text, isCompleted).Text);
            Assert.Equal(isCompleted, MarkdownFactory.TaskListItem(text, isCompleted).IsCompleted);
        }

        [Fact]
        public void MarkdownFactory_TaskListItemStart_DefaultValues()
        {
            string text = MarkdownTextText();

            Assert.Equal("- [ ] ", MarkdownFactory.TaskListItemStart());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MarkdownFactory_TaskListItemStart(bool isCompleted)
        {
            string text = MarkdownTextText();

            Assert.Equal($"- [{((isCompleted) ? "x" : " ")}] ", MarkdownFactory.TaskListItemStart(isCompleted));
        }

        [Fact]
        public void MarkdownFactory_CompletedTaskListItem()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, MarkdownFactory.CompletedTaskListItem(text).Text);
            Assert.True(MarkdownFactory.CompletedTaskListItem(text).IsCompleted);
        }

        [Fact]
        public void MarkdownFactory_Image_DefaultValues()
        {
            string text = LinkText();
            string url = LinkUrl();
            string title = LinkTitle();

            Image image = MarkdownFactory.Image(text, url);

            Assert.Equal(text, image.Text);
            Assert.Equal(url, image.Url);
            Assert.Null(image.Title);
        }

        [Fact]
        public void MarkdownFactory_Image()
        {
            string text = LinkText();
            string url = LinkUrl();
            string title = LinkTitle();

            Image image = MarkdownFactory.Image(text, url);

            image = MarkdownFactory.Image(text, url, title);

            Assert.Equal(text, image.Text);
            Assert.Equal(url, image.Url);
            Assert.Equal(title, image.Title);
        }

        [Fact]
        public void MarkdownFactory_Link_DefaultValues()
        {
            string text = LinkText();
            string url = LinkUrl();
            string title = LinkTitle();

            Link link = MarkdownFactory.Link(text, url);

            Assert.Equal(text, link.Text);
            Assert.Equal(url, link.Url);
            Assert.Null(link.Title);
        }

        [Fact]
        public void MarkdownFactory_Link()
        {
            string text = LinkText();
            string url = LinkUrl();
            string title = LinkTitle();

            Link link = MarkdownFactory.Link(text, url, title);

            Assert.Equal(text, link.Text);
            Assert.Equal(url, link.Url);
            Assert.Equal(title, link.Title);
        }

        [Fact]
        public void MarkdownFactory_LinkOrText_Link()
        {
            string text = LinkText();
            string url = LinkUrl();

            Assert.IsType<Link>(MarkdownFactory.LinkOrText(text, url));
        }

        [Fact]
        public void MarkdownFactory_LinkOrText_Text()
        {
            string text = LinkText();
            string url = LinkUrl();

            Assert.IsType<MarkdownText>(MarkdownFactory.LinkOrText(text));
            Assert.IsType<MarkdownText>(MarkdownFactory.LinkOrText(text, url: ""));
            Assert.IsType<MarkdownText>(MarkdownFactory.LinkOrText(text, url: null));
        }

        [Fact]
        public void MarkdownFactory_CodeBlock_DefaultValues()
        {
            string text = CodeBlockText();
            string info = CodeBlockInfo();

            FencedCodeBlock block = MarkdownFactory.FencedCodeBlock(text);

            Assert.Equal(text, block.Text);
            Assert.Null(block.Info);
        }

        [Fact]
        public void MarkdownFactory_CodeBlock()
        {
            string text = CodeBlockText();
            string info = CodeBlockInfo();

            FencedCodeBlock block = MarkdownFactory.FencedCodeBlock(text, info);

            Assert.Equal(text, block.Text);
            Assert.Equal(info, block.Info);
        }

        [Fact]
        public void MarkdownFactory_IndentedCodeBlock()
        {
            string text = IndentedCodeBlockText();

            IndentedCodeBlock block = MarkdownFactory.IndentedCodeBlock(text);

            Assert.Equal(text, block.Text);
        }

        [Fact]
        public void MarkdownFactory_QuoteBlock()
        {
            string text = QuoteBlockText();

            QuoteBlock block = MarkdownFactory.QuoteBlock(text);

            Assert.Equal(text, block.Text);
        }

        [Theory]
        [InlineData(HorizontalRuleStyle.Asterisk)]
        [InlineData(HorizontalRuleStyle.Hyphen)]
        [InlineData(HorizontalRuleStyle.Underscore)]
        public void MarkdownFactory_HorizontalRule_DefaultValues(HorizontalRuleStyle style)
        {
            for (int i = 3; i <= 5; i++)
            {
                Assert.Equal(style, MarkdownFactory.HorizontalRule(style, count: i).Style);
                Assert.Equal(i, MarkdownFactory.HorizontalRule(style, count: i).Count);
                Assert.Equal(" ", MarkdownFactory.HorizontalRule(style, count: i).Space);
            }
        }

        [Theory]
        [InlineData(HorizontalRuleStyle.Asterisk)]
        [InlineData(HorizontalRuleStyle.Hyphen)]
        [InlineData(HorizontalRuleStyle.Underscore)]
        public void MarkdownFactory_HorizontalRule(HorizontalRuleStyle style)
        {
            for (int i = 3; i <= 5; i++)
            {
                Assert.Equal(style, MarkdownFactory.HorizontalRule(style, count: i, space: " ").Style);
                Assert.Equal(i, MarkdownFactory.HorizontalRule(style, count: i, space: " ").Count);
                Assert.Equal("", MarkdownFactory.HorizontalRule(style, count: i, space: null).Space);
                Assert.Equal("", MarkdownFactory.HorizontalRule(style, count: i, space: "").Space);
                Assert.Equal("  ", MarkdownFactory.HorizontalRule(style, count: i, space: "  ").Space);
            }
        }

        [Theory]
        [InlineData('*', HorizontalRuleStyle.Asterisk)]
        [InlineData('-', HorizontalRuleStyle.Hyphen)]
        [InlineData('_', HorizontalRuleStyle.Underscore)]
        public void MarkdownFactory_HorizontalRuleChar(char syntax, HorizontalRuleStyle style)
        {
            Assert.Equal(syntax, MarkdownFactory.HorizontalRuleChar(style));
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(0)]
        [InlineData(2)]
        public void MarkdownFactory_HorizontalRule_Throws(int count)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(MarkdownFactory.HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: count, space: " ")));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(MarkdownFactory.HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: count, space: " ")));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(MarkdownFactory.HorizontalRule(style: HorizontalRuleStyle.Underscore, count: count, space: " ")));
        }

        [Fact]
        public void MarkdownFactory_Table_Params()
        {
            Table table = MarkdownFactory.Table(Array.Empty<TableColumn>());
            Assert.Empty(table.Columns);
            Assert.Empty(table.Rows);
        }

        [Fact]
        public void MarkdownFactory_Table_0()
        {
            Table table = MarkdownFactory.Table();
            Assert.Empty(table.Columns);
            Assert.Empty(table.Rows);

            table = MarkdownFactory.Table(Array.Empty<TableColumn>());
            Assert.Empty(table.Columns);
            Assert.Empty(table.Rows);
        }

        [Fact]
        public void MarkdownFactory_Table_1()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            Table table = MarkdownFactory.Table(column1);

            Assert.Single(table.Columns);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
        }

        [Fact]
        public void MarkdownFactory_Table_2()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            Table table = MarkdownFactory.Table(column1, column2);

            Assert.Equal(2, table.Columns.Count);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
            Assert.Equal(column2.Name, table.Columns[1].Name);
        }

        [Fact]
        public void MarkdownFactory_Table_3()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            Table table = MarkdownFactory.Table(column1, column2, column3);

            Assert.Equal(3, table.Columns.Count);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
            Assert.Equal(column2.Name, table.Columns[1].Name);
            Assert.Equal(column3.Name, table.Columns[2].Name);
        }

        [Fact]
        public void MarkdownFactory_Table_4()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            Table table = MarkdownFactory.Table(column1, column2, column3, column4);

            Assert.Equal(4, table.Columns.Count);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
            Assert.Equal(column2.Name, table.Columns[1].Name);
            Assert.Equal(column3.Name, table.Columns[2].Name);
            Assert.Equal(column4.Name, table.Columns[3].Name);
        }

        [Fact]
        public void MarkdownFactory_Table_5()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            TableColumn column5 = MarkdownFactory.TableColumn("5");
            Table table = MarkdownFactory.Table(column1, column2, column3, column4, column5);

            Assert.Equal(5, table.Columns.Count);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
            Assert.Equal(column2.Name, table.Columns[1].Name);
            Assert.Equal(column3.Name, table.Columns[2].Name);
            Assert.Equal(column4.Name, table.Columns[3].Name);
            Assert.Equal(column5.Name, table.Columns[4].Name);
        }

        [Fact]
        public void MarkdownFactory_Table_6()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            TableColumn column5 = MarkdownFactory.TableColumn("5");
            TableColumn column6 = MarkdownFactory.TableColumn("6");
            Table table = MarkdownFactory.Table(column1, column2, column3, column4, column5, column6);

            Assert.Equal(6, table.Columns.Count);
            Assert.Empty(table.Rows);
            Assert.Equal(column1.Name, table.Columns[0].Name);
            Assert.Equal(column2.Name, table.Columns[1].Name);
            Assert.Equal(column3.Name, table.Columns[2].Name);
            Assert.Equal(column4.Name, table.Columns[3].Name);
            Assert.Equal(column5.Name, table.Columns[4].Name);
            Assert.Equal(column6.Name, table.Columns[5].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_Params()
        {
            TableColumnCollection th = MarkdownFactory.TableHeader(Array.Empty<TableColumn>());
            Assert.Empty(th);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_0()
        {
            TableColumnCollection th = MarkdownFactory.TableHeader();
            Assert.Empty(th);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_1()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1);

            Assert.Single(th);
            Assert.Equal(column1.Name, th[0].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_2()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1, column2);

            Assert.Equal(2, th.Count);
            Assert.Equal(column1.Name, th[0].Name);
            Assert.Equal(column2.Name, th[1].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_3()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1, column2, column3);

            Assert.Equal(3, th.Count);
            Assert.Equal(column1.Name, th[0].Name);
            Assert.Equal(column2.Name, th[1].Name);
            Assert.Equal(column3.Name, th[2].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_4()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1, column2, column3, column4);

            Assert.Equal(4, th.Count);
            Assert.Equal(column1.Name, th[0].Name);
            Assert.Equal(column2.Name, th[1].Name);
            Assert.Equal(column3.Name, th[2].Name);
            Assert.Equal(column4.Name, th[3].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_5()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            TableColumn column5 = MarkdownFactory.TableColumn("5");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1, column2, column3, column4, column5);

            Assert.Equal(5, th.Count);
            Assert.Equal(column1.Name, th[0].Name);
            Assert.Equal(column2.Name, th[1].Name);
            Assert.Equal(column3.Name, th[2].Name);
            Assert.Equal(column4.Name, th[3].Name);
            Assert.Equal(column5.Name, th[4].Name);
        }

        [Fact]
        public void MarkdownFactory_TableHeader_6()
        {
            TableColumn column1 = MarkdownFactory.TableColumn("1");
            TableColumn column2 = MarkdownFactory.TableColumn("2");
            TableColumn column3 = MarkdownFactory.TableColumn("3");
            TableColumn column4 = MarkdownFactory.TableColumn("4");
            TableColumn column5 = MarkdownFactory.TableColumn("5");
            TableColumn column6 = MarkdownFactory.TableColumn("6");
            TableColumnCollection th = MarkdownFactory.TableHeader(column1, column2, column3, column4, column5, column6);

            Assert.Equal(6, th.Count);
            Assert.Equal(column1.Name, th[0].Name);
            Assert.Equal(column2.Name, th[1].Name);
            Assert.Equal(column3.Name, th[2].Name);
            Assert.Equal(column4.Name, th[3].Name);
            Assert.Equal(column5.Name, th[4].Name);
            Assert.Equal(column6.Name, th[5].Name);
        }

        [Fact]
        public void MarkdownFactory_TableColumn_DefaultValues()
        {
            string name = TableColumnName();
            Assert.Equal(name, MarkdownFactory.TableColumn(name).Name);
            Assert.Equal(Alignment.Left, MarkdownFactory.TableColumn(name).Alignment);
        }

        [Theory]
        [InlineData(Alignment.Left)]
        [InlineData(Alignment.Center)]
        [InlineData(Alignment.Right)]
        public void MarkdownFactory_TableColumnTest1(Alignment alignment)
        {
            string name = TableColumnName();

            Assert.Equal(name, MarkdownFactory.TableColumn(name, alignment).Name);
            Assert.Equal(alignment, MarkdownFactory.TableColumn(name, alignment).Alignment);
        }

        [Fact]
        public void MarkdownFactory_TableRow_1()
        {
            const string row1 = "1";

            List<object> row = MarkdownFactory.TableRow(row1);
            Assert.Single(row);
            Assert.Equal(row1, row[0]);
        }

        [Fact]
        public void MarkdownFactory_TableRow_2()
        {
            const string row1 = "1";
            const string row2 = "2";

            List<object> row = MarkdownFactory.TableRow(row1, row2);
            Assert.Equal(2, row.Count);
            Assert.Equal(row1, row[0]);
            Assert.Equal(row2, row[1]);
        }

        [Fact]
        public void MarkdownFactory_TableRow_3()
        {
            const string row1 = "1";
            const string row2 = "2";
            const string row3 = "3";

            List<object> row = MarkdownFactory.TableRow(row1, row2, row3);
            Assert.Equal(3, row.Count);
            Assert.Equal(row1, row[0]);
            Assert.Equal(row2, row[1]);
            Assert.Equal(row3, row[2]);
        }

        [Fact]
        public void MarkdownFactory_TableRow_4()
        {
            const string row1 = "1";
            const string row2 = "2";
            const string row3 = "3";
            const string row4 = "4";

            List<object> row = MarkdownFactory.TableRow(row1, row2, row3, row4);
            Assert.Equal(4, row.Count);
            Assert.Equal(row1, row[0]);
            Assert.Equal(row2, row[1]);
            Assert.Equal(row3, row[2]);
            Assert.Equal(row4, row[3]);
        }

        [Fact]
        public void MarkdownFactory_TableRow_5()
        {
            const string row1 = "1";
            const string row2 = "2";
            const string row3 = "3";
            const string row4 = "4";
            const string row5 = "5";

            List<object> row = MarkdownFactory.TableRow(row1, row2, row3, row4, row5);
            Assert.Equal(5, row.Count);
            Assert.Equal(row1, row[0]);
            Assert.Equal(row2, row[1]);
            Assert.Equal(row3, row[2]);
            Assert.Equal(row4, row[3]);
            Assert.Equal(row5, row[4]);
        }

        [Fact]
        public void MarkdownFactory_HtmlEntity()
        {
            int number = IntValue(1, 0xFFFF);

            var entity = MarkdownFactory.HtmlEntity(number);

            Assert.Equal(number, entity.Number);
        }
    }
}
