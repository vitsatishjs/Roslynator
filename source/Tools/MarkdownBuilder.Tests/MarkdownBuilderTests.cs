﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;
using static Pihrtsoft.Markdown.Linq.MFactory;
using static Pihrtsoft.Markdown.Tests.TestHelpers;
using static Pihrtsoft.Markdown.Tests.MarkdownSamples;
using System.Globalization;
using Pihrtsoft.Markdown.Linq;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class MarkdownBuilderTests
    {
        private const string Value = Chars;
        private const string ValueEscaped = CharsEscaped;

        [Theory]
        [InlineData("**", null)]
        [InlineData("**", EmphasisStyle.Asterisk)]
        [InlineData("__", EmphasisStyle.Underscore)]
        public void MarkdownBuilder_AppendBold(string syntax, EmphasisStyle? boldStyle)
        {
            const string x = Chars;
            const string y = CharsEscaped;
            MarkdownWriter mw = CreateBuilderWithBoldStyle(boldStyle);

            Assert.Equal(syntax + y + syntax, mw.WriteBold(x).ToStringAndClear());
        }

        [Theory]
        [InlineData("**", null)]
        [InlineData("**", EmphasisStyle.Asterisk)]
        [InlineData("__", EmphasisStyle.Underscore)]
        public void MarkdownBuilder_Append_Bold(string syntax, EmphasisStyle? boldStyle)
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownWriter mw = CreateBuilderWithBoldStyle(boldStyle);
            mw.Write(Bold(x));

            Assert.Equal(syntax + y + syntax, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData("*", null)]
        [InlineData("*", EmphasisStyle.Asterisk)]
        [InlineData("_", EmphasisStyle.Underscore)]
        public void MarkdownBuilder_AppendItalic(string syntax, EmphasisStyle? ItalicStyle)
        {
            const string x = Chars;
            const string y = CharsEscaped;
            MarkdownWriter mw = CreateBuilderWithItalicStyle(ItalicStyle);

            Assert.Equal(syntax + y + syntax, mw.WriteItalic(x).ToStringAndClear());
        }

        [Theory]
        [InlineData("*", null)]
        [InlineData("*", EmphasisStyle.Asterisk)]
        [InlineData("_", EmphasisStyle.Underscore)]
        public void MarkdownBuilder_Append_Italic(string syntax, EmphasisStyle? italicStyle)
        {
            const string x = Chars;
            const string y = CharsEscaped;
            MarkdownWriter mw = CreateBuilderWithItalicStyle(italicStyle);
            mw.Write(Italic(x));

            Assert.Equal(syntax + y + syntax, mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendStrikethrough()
        {
            const string x = Chars;
            const string y = CharsEscaped;
            MarkdownWriter mw = CreateWriter();

            Assert.Equal("~~" + y + "~~", mw.WriteStrikethrough(x).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_Strikethrough()
        {
            const string x = Chars;
            const string y = CharsEscaped;
            MarkdownWriter mw = CreateWriter();
            mw.Write(Strikethrough(x));

            Assert.Equal("~~" + y + "~~", mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendCode()
        {
            const string x = CharsEnclosedWithBacktick;
            const string y = CharsEnclosedWithBacktickDoubled;
            MarkdownWriter mw = CreateWriter();

            Assert.Equal("` " + y + " `", mw.WriteInlineCode(x).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendCode_String()
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Equal("` `` `", mw.WriteInlineCode("`").ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_InlineCode()
        {
            const string x = CharsEnclosedWithBacktick;
            const string y = CharsEnclosedWithBacktickDoubled;
            MarkdownWriter mw = CreateWriter();
            mw.Write(InlineCode(x));

            Assert.Equal("` " + y + " `", mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        public void MarkdownBuilder_AppendHeading1(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"# {ValueEscaped}{NewLine}", mw.WriteHeading1(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        public void MarkdownBuilder_AppendHeading2(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"## {ValueEscaped}{NewLine}", mw.WriteHeading2(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading3(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"### {ValueEscaped}{NewLine}", mw.WriteHeading3(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading4(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"#### {ValueEscaped}{NewLine}", mw.WriteHeading4(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading5(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"##### {ValueEscaped}{NewLine}", mw.WriteHeading5(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(HeadingOptions.None)]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading6(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal($"###### {ValueEscaped}{NewLine}", mw.WriteHeading6(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void MarkdownBuilder_AppendHeading(int level)
        {
            MarkdownWriter mw = CreateWriter(new MarkdownFormat(headingOptions: HeadingOptions.None));

            Assert.Equal($"{new string('#', level)} {ValueEscaped}{NewLine}", mw.WriteHeading(level, Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        public void MarkdownBuilder_AppendHeading_Throws(int level)
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Throws<ArgumentOutOfRangeException>(() => mw.WriteHeading(level, Value));
        }

        [Fact]
        public void MarkdownBuilder_AppendHeading_Params()
        {
            MarkdownWriter mw = CreateWriter(new MarkdownFormat(headingOptions: HeadingOptions.None));
        }

        [Theory]
        [InlineData(HeadingOptions.UnderlineHeading1)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading_UnderlineH1(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal(ValueEscaped + NewLine + new string('=', ValueEscaped.Length) + NewLine, mw.WriteHeading1(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(HeadingOptions.UnderlineHeading2)]
        [InlineData(HeadingOptions.Underline)]
        public void MarkdownBuilder_AppendHeading_UnderlineH2(HeadingOptions? options)
        {
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(options);

            Assert.Equal(ValueEscaped + NewLine + new string('-', ValueEscaped.Length) + NewLine, mw.WriteHeading2(Value).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendHeading_EmptyLineBefore()
        {
            string text = HeadingText();
            const string s = "# " + CharsEscaped + NewLine;
            MHeading heading = Heading(1, Chars);
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(HeadingOptions.EmptyLineBefore);
            mw.WriteString(text);
            mw.Write(heading);
            mw.Write(heading);
            mw.Write(text);

            Assert.Equal(
                text + NewLine2 + s + NewLine + s + text,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendHeading_EmptyLineAfter()
        {
            string text = HeadingText();
            const string s = "# " + CharsEscaped + NewLine;
            MHeading heading = Heading(1, Chars);
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(HeadingOptions.EmptyLineAfter);
            mw.WriteString(text);
            mw.Write(heading);
            mw.Write(heading);
            mw.Write(text);

            Assert.Equal(
                text + NewLine + s + NewLine + s + NewLine + text,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendHeading_EmptyLineBeforeAfter()
        {
            string text = HeadingText();
            const string s = "# " + CharsEscaped + NewLine;
            MHeading heading = Heading(1, Chars);
            MarkdownWriter mw = CreateBuilderWithHeadingOptions(HeadingOptions.EmptyLineBeforeAndAfter);
            mw.Write(text);
            mw.Write(heading);
            mw.Write(heading);
            mw.Write(text);

            Assert.Equal(
                text + NewLine2 + s + NewLine + s + NewLine + text,
                mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(0)]
        [InlineData(2)]
        public void MarkdownBuilder_AppendHorizontalRule_Throws(int count)
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Throws<ArgumentOutOfRangeException>(() => mw.WriteHorizontalRule(text: "*", count: count, separator: ""));
            Assert.Throws<ArgumentOutOfRangeException>(() => mw.WriteHorizontalRule(text: "-", count: count, separator: ""));
            Assert.Throws<ArgumentOutOfRangeException>(() => mw.WriteHorizontalRule(text: "_", count: count, separator: ""));
        }

        [Fact]
        public void MarkdownBuilder_AppendImage()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "ImageText";
            const string url = "ImageUrl";
            const string title = "ImageTitle";

            MImage image = Image(text + Chars, url + CharsWithoutSpaces, title + Chars);

            string expected = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.Equal(expected, mw.WriteImage(image.Text, image.Url, image.Title).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendImage_NoTitle()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "ImageText";
            const string url = "ImageUrl";

            MImage image = Image(text + Chars, url + CharsWithoutSpaces);

            string expected = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.Equal(expected, mw.WriteImage(image.Text, image.Url).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_Image()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "ImageText";
            const string url = "ImageUrl";
            const string title = "ImageTitle";

            MImage image = Image(text + Chars, url + CharsWithoutSpaces, title + Chars);

            string expected = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            mw.Write(image);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_Image_NoTitle()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "ImageText";
            const string url = "ImageUrl";

            MImage image = Image(text + Chars, url + CharsWithoutSpaces);

            string expected = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            mw.Write(image);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendImage_Throws()
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Throws<ArgumentNullException>(() => mw.WriteImage(text: null, url: "Url"));
            Assert.Throws<ArgumentNullException>(() => mw.WriteImage(text: "Text", url: null));
        }

        [Fact]
        public void MarkdownBuilder_AppendLink()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "LinkText";
            const string url = "LinkUrl";
            const string title = "LinkTitle";

            MLink image = Link(text + Chars, url + CharsWithoutSpaces, title + Chars);

            string expected = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.Equal(expected, mw.WriteLink(image.Text, image.Url, image.Title).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendLink_NoTitle()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "LinkText";
            const string url = "LinkUrl";

            MLink image = Link(text + Chars, url + CharsWithoutSpaces);

            string expected = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.Equal(expected, mw.WriteLink(image.Text, image.Url).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_Link()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "LinkText";
            const string url = "LinkUrl";
            const string title = "LinkTitle";

            MLink image = Link(text + Chars, url + CharsWithoutSpaces, title + Chars);

            string expected = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.Equal(expected, mw.Write2(image).ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_Link_NoTitle()
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "LinkText";
            const string url = "LinkUrl";

            MLink image = Link(text + Chars, url + CharsWithoutSpaces);

            string expected = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            mw.Write(image);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData("*", null)]
        [InlineData("*", BulletListStyle.Asterisk)]
        [InlineData("-", BulletListStyle.Minus)]
        [InlineData("+", BulletListStyle.Plus)]
        public void MarkdownBuilder_AppendListItem(string syntax, BulletListStyle? style)
        {
            MarkdownWriter mw = CreateBuilderWithListItemStyle(style);
            const string text = "ListItemText";
            string expected = syntax + $" {text + CharsEscaped}" + NewLine;
            MBulletItem item = BulletItem(text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData("*", null)]
        [InlineData("*", BulletListStyle.Asterisk)]
        [InlineData("-", BulletListStyle.Minus)]
        [InlineData("+", BulletListStyle.Plus)]
        public void MarkdownBuilder_Append_ListItem(string syntax, BulletListStyle? style)
        {
            MarkdownWriter mw = CreateBuilderWithListItemStyle(style);
            const string text = "ListItemText";
            string expected = syntax + $" {text + CharsEscaped}" + NewLine;
            MBulletItem item = BulletItem(text + Chars);

            mw.Write(item);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void MarkdownBuilder_AppendOrderedListItem(int number)
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "OrderedListItemText";

            string expected = number + $". {text + CharsEscaped}" + NewLine;
            MOrderedItem item = OrderedItem(number, text + Chars);

            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void MarkdownBuilder_Append_OrderedListItem(int number)
        {
            MarkdownWriter mw = CreateWriter();

            const string text = "OrderedListItemText";

            string expected = number + $". {text + CharsEscaped}" + NewLine;
            MOrderedItem item = OrderedItem(number, text + Chars);
            mw.Write(item);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(-2)]
        [InlineData(-1)]
        public void MarkdownBuilder_AppendOrderedListItem_Throws(int number)
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Throws<ArgumentOutOfRangeException>(() => mw.WriteOrderedItem(number, StringValue()));
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_AppendTaskListItem(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [ ] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = TaskItem(false, text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_Append_TaskListItem(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [ ] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = TaskItem(false, text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_Append_TaskListItem_NotCompleted(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [ ] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = TaskItem(false, text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_Append_TaskListItem_Completed(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [x] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = TaskItem(true, text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_AppendCompletedTaskListItem(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [x] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = CompletedTaskItem(text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(Chars, CharsEscaped)]
        public void MarkdownBuilder_Append_CompletedTaskListItem(string text, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            const string start = "- [x] ";
            string expected = start + text2 + CharsEscaped + NewLine;
            MTaskItem item = CompletedTaskItem(text + Chars);
            item.WriteTo(mw);

            Assert.Equal(expected, mw.ToStringAndClear());
        }

        [Theory]
        [InlineData("```", null)]
        [InlineData("```", CodeFenceStyle.Backtick)]
        [InlineData("~~~", CodeFenceStyle.Tilde)]
        public void MarkdownBuilder_Append_CodeBlock_CodeFenceStyle(string syntax, CodeFenceStyle? style)
        {
            MarkdownWriter mw = CreateBuilderWithCodeFenceOptions(style);

            MFencedCodeBlock block = FencedCodeBlock(Chars, DefaultText);
            block.WriteTo(mw);

            Assert.Equal(
                syntax + DefaultText + NewLine + Chars + NewLine + syntax + NewLine,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_CodeBlock_CodeBlockOptionsNone()
        {
            MarkdownWriter mw = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.None);

            MFencedCodeBlock block = FencedCodeBlock(Chars, DefaultText);

            mw.Write(DefaultText);
            mw.Write(block);
            mw.Write(block);
            mw.Write(DefaultText);

            Assert.Equal(
                DefaultText + NewLine + CodeBlockMarkdown + CodeBlockMarkdown + DefaultText,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_CodeBlock_CodeBlockOptionsEmptyLineBefore()
        {
            MarkdownWriter mw = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineBefore);

            MFencedCodeBlock block = FencedCodeBlock(Chars, DefaultText);

            mw.Write(DefaultText);
            mw.Write(block);
            mw.Write(block);
            mw.Write(DefaultText);

            Assert.Equal(
                DefaultText + NewLine2 + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + DefaultText,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_CodeBlock_CodeBlockOptionsEmptyLineAfter()
        {
            MarkdownWriter mw = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineAfter);

            MFencedCodeBlock block = FencedCodeBlock(Chars, DefaultText);

            mw.Write(DefaultText);
            mw.Write(block);
            mw.Write(block);
            mw.Write(DefaultText);

            Assert.Equal(
                DefaultText + NewLine + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + NewLine + DefaultText,
                mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_Append_CodeBlock_CodeBlockOptionsEmptyLineBeforeAndAfter()
        {
            MarkdownWriter mw = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineBeforeAndAfter);

            MFencedCodeBlock block = FencedCodeBlock(Chars, DefaultText);

            mw.Write(DefaultText);
            mw.Write(block);
            mw.Write(block);
            mw.Write(DefaultText);

            Assert.Equal(
                DefaultText + NewLine2 + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + NewLine + DefaultText,
                mw.ToStringAndClear());
        }

        [Theory]
        [InlineData(Chars, "> " + CharsEscaped + NewLine)]
        [InlineData(Chars + NewLine + Chars, "> " + CharsEscaped + NewLine + "> " + CharsEscaped + NewLine)]
        public void MarkdownBuilder_AppendQuoteBlock(string text1, string text2)
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Equal(text2, mw.WriteBlockQuote(text1).ToStringAndClear());
        }

        [Theory]
        [InlineData(Chars, "> " + CharsEscaped + NewLine)]
        [InlineData(Chars + NewLine + Chars, "> " + CharsEscaped + NewLine + "> " + CharsEscaped + NewLine)]
        public void MarkdownBuilder_Append_QuoteBlock(string text1, string text2)
        {
            MarkdownWriter mw = CreateWriter();
            MBlockQuote quoteBlock = BlockQuote(text1);

            mw.Write(quoteBlock);

            Assert.Equal(text2, mw.ToStringAndClear());
        }

        [Fact]
        public void MarkdownBuilder_AppendQuoteBlock_Throws()
        {
            MarkdownWriter mw = CreateWriter();

            Assert.Throws<ArgumentNullException>(() => mw.WriteBlockQuote(null));
        }

        [Theory]
        [InlineData("&#x", "x", null)]
        [InlineData("&#x", "x", CharEntityFormat.Hexadecimal)]
        [InlineData("&#", null, CharEntityFormat.Decimal)]
        public void MarkdownBuilder_AppendHtmlEntity(string syntax, string format, CharEntityFormat? htmlEntityFormat)
        {
            MarkdownWriter mw = CreateBuilderWithHtmlEntityFormat(htmlEntityFormat);

            char ch = CharEntityChar();

            MCharEntity entity = CharEntity(ch);

            Assert.Equal(syntax + ((int)ch).ToString(format, CultureInfo.InvariantCulture) + ";", mw.WriteCharEntity(ch).ToStringAndClear());
        }

        [Theory]
        [InlineData("&#x", "x", null)]
        [InlineData("&#x", "x", CharEntityFormat.Hexadecimal)]
        [InlineData("&#", null, CharEntityFormat.Decimal)]
        public void MarkdownBuilder_Append_HtmlEntity(string syntax, string format, CharEntityFormat? htmlEntityFormat)
        {
            MarkdownWriter mw = CreateBuilderWithHtmlEntityFormat(htmlEntityFormat);

            char ch = CharEntityChar();

            MCharEntity charEntity = CharEntity(ch);

            mw.Write(charEntity);

            Assert.Equal(syntax + ((int)ch).ToString(format, CultureInfo.InvariantCulture) + ";", mw.ToStringAndClear());
        }
    }
}
