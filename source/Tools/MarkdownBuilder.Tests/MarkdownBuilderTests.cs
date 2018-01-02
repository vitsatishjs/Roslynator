// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class MarkdownBuilderTests
    {
        private const string Value = Chars;
        private const string ValueEscaped = CharsEscaped;

        [Fact]
        public void AppendBoldTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.Equal("**" + y + "**", mb.AppendBold(x).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.AppendBold((object)x).ToStringAndClear());
            Assert.Equal("**" + y + y + "**", mb.AppendBold(x, x).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x)).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithBoldStyle(EmphasisStyle.Asterisk));
            Assert.Equal("**" + y + "**", mb.AppendBold(x).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.AppendBold((object)x).ToStringAndClear());
            Assert.Equal("**" + y + y + "**", mb.AppendBold(x, x).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x)).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.Equal("**" + y + "**", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithBoldStyle(EmphasisStyle.Underscore));
            Assert.Equal("__" + y + "__", mb.AppendBold(x).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.AppendBold((object)x).ToStringAndClear());
            Assert.Equal("__" + y + y + "__", mb.AppendBold(x, x).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append(Bold(x)).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.Equal("__" + y + "__", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());
        }

        [Fact]
        public void AppendItalicTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.Equal("*" + y + "*", mb.AppendItalic(x).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.Equal("*" + y + y + "*", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x)).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithItalicStyle(EmphasisStyle.Asterisk));
            Assert.Equal("*" + y + "*", mb.AppendItalic(x).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.Equal("*" + y + y + "*", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x)).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.Equal("*" + y + "*", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithItalicStyle(EmphasisStyle.Underscore));
            Assert.Equal("_" + y + "_", mb.AppendItalic(x).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.Equal("_" + y + y + "_", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append(Italic(x)).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.Equal("_" + y + "_", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());
        }

        [Fact]
        public void AppendStrikethroughTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.Equal("~~" + y + "~~", mb.AppendStrikethrough(x).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.AppendStrikethrough((object)x).ToStringAndClear());
            Assert.Equal("~~" + y + y + "~~", mb.AppendStrikethrough(x, x).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append(Strikethrough(x)).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append(Strikethrough(x), escape: true).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append((object)Strikethrough(x)).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append(Strikethrough(x), escape: true).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append(x, EmphasisOptions.Strikethrough).ToStringAndClear());
            Assert.Equal("~~" + y + "~~", mb.Append(x, EmphasisOptions.Strikethrough, escape: true).ToStringAndClear());
        }

        [Fact]
        public void AppendCodeTest1()
        {
            const string x = CharsEnclosedWithBacktick;
            const string y = CharsEnclosedWithBacktickDoubled;

            MarkdownBuilder mb = CreateBuilder();

            Assert.Equal("` " + y + " `", mb.AppendCode(x).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.AppendCode((object)x).ToStringAndClear());
            Assert.Equal("` " + y + y + " `", mb.AppendCode(x, x).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append(Code(x)).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append(Code(x), escape: true).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append((object)Code(x)).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append(Code(x), escape: true).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append(x, EmphasisOptions.Code).ToStringAndClear());
            Assert.Equal("` " + y + " `", mb.Append(x, EmphasisOptions.Code, escape: true).ToStringAndClear());
        }

        [Fact]
        public void AppendCode_String()
        {
            MarkdownBuilder mb = CreateBuilder();
            Assert.Equal("` `` `", mb.AppendCode("`").ToStringAndClear());
        }

        [Fact]
        public void AppendCode_Char()
        {
            MarkdownBuilder mb = CreateBuilder();
            Assert.Equal("` `` `", mb.AppendCode('`').ToStringAndClear());
        }

        [Fact]
        public void AppendCode_Params()
        {
            MarkdownBuilder mb = CreateBuilder();
            Assert.Equal("` ```` `", mb.AppendCode('`', '`').ToStringAndClear());
        }

        [Fact]
        public void AppendHeadingTest2()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.Equal($"# {ValueEscaped}{NewLine}", mb.AppendHeading1(Value).ToStringAndClear());
            Assert.Equal($"## {ValueEscaped}{NewLine}", mb.AppendHeading2(Value).ToStringAndClear());
            Assert.Equal($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.Equal($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.Equal($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.Equal($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void AppendHeadingTest2_(int value)
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.Equal($"{new string('#', value)} {ValueEscaped}{NewLine}", mb.AppendHeading(value, Value).ToStringAndClear());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        public void HeadingLevelOutOfRangeTest1(int level)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => mb.AppendHeading(level, Value));
        }

        [Fact]
        public void AppendHeadingWithoutTextTest1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.Equal("# ", mb.AppendHeading1().ToStringAndClear());
            Assert.Equal("## ", mb.AppendHeading2().ToStringAndClear());
            Assert.Equal("### ", mb.AppendHeading3().ToStringAndClear());
            Assert.Equal("#### ", mb.AppendHeading4().ToStringAndClear());
            Assert.Equal("##### ", mb.AppendHeading5().ToStringAndClear());
            Assert.Equal("###### ", mb.AppendHeading6().ToStringAndClear());
        }

        [Fact]
        public void AppendHeadingUnderlineH1Test1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.UnderlineH1));

            Assert.Equal(ValueEscaped + NewLine + new string('=', ValueEscaped.Length) + NewLine, mb.AppendHeading1(Value).ToStringAndClear());
            Assert.Equal($"## {ValueEscaped}{NewLine}", mb.AppendHeading2(Value).ToStringAndClear());
            Assert.Equal($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.Equal($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.Equal($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.Equal($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [Fact]
        public void AppendHeadingUnderlineH2Test1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.UnderlineH2));

            Assert.Equal($"# {ValueEscaped}{NewLine}", mb.AppendHeading1(Value).ToStringAndClear());
            Assert.Equal(ValueEscaped + NewLine + new string('-', ValueEscaped.Length) + NewLine, mb.AppendHeading2(Value).ToStringAndClear());
            Assert.Equal($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.Equal($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.Equal($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.Equal($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [Fact]
        public void AppendHeadingUnderlineTest1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.Underline));

            Assert.Equal(ValueEscaped + NewLine + new string('=', ValueEscaped.Length) + NewLine, mb.AppendHeading1(Value).ToStringAndClear());
            Assert.Equal(ValueEscaped + NewLine + new string('-', ValueEscaped.Length) + NewLine, mb.AppendHeading2(Value).ToStringAndClear());
            Assert.Equal($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.Equal($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.Equal($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.Equal($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [Fact]
        public void AppendHeadingEmptyLineBeforeAfterTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string x = "x";
            const string s = "# " + CharsEscaped + NewLine;
            Heading h = Heading(Chars, 1);

            mb.WithSettings(mb.Settings.WithHeadingOptions(headingOptions: HeadingOptions.None));

            Assert.Equal(x + NewLine + s + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBefore));
            Assert.Equal(x + NewLine2 + s + NewLine + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineAfter));
            Assert.Equal(x + NewLine + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBeforeAndAfter));
            Assert.Equal(x + NewLine2 + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());
        }

        [Fact]
        public void AppendHorizontalRuleTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            HorizontalRule hr = HorizontalRule.Default;
            Assert.Equal("- - -" + NewLine, mb.Append(hr).ToStringAndClear());

            Assert.Equal("***" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.Equal("* * *" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.Equal("*****" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.Equal("* * * * *" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.Equal("***" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: false).ToStringAndClear());
            Assert.Equal("* * *" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: true).ToStringAndClear());
            Assert.Equal("*****" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: false).ToStringAndClear());
            Assert.Equal("* * * * *" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: true).ToStringAndClear());

            Assert.Equal("---" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.Equal("- - -" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.Equal("-----" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.Equal("- - - - -" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.Equal("---" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: false).ToStringAndClear());
            Assert.Equal("- - -" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: true).ToStringAndClear());
            Assert.Equal("-----" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: false).ToStringAndClear());
            Assert.Equal("- - - - -" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: true).ToStringAndClear());

            Assert.Equal("___" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.Equal("_ _ _" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.Equal("_____" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.Equal("_ _ _ _ _" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.Equal("___" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: false).ToStringAndClear());
            Assert.Equal("_ _ _" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: true).ToStringAndClear());
            Assert.Equal("_____" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: false).ToStringAndClear());
            Assert.Equal("_ _ _ _ _" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: true).ToStringAndClear());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AppendHorizontalRuleTest2(int count)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: count, addSpaces: false)));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: count, addSpaces: false)));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: count, addSpaces: false)));

            Assert.Throws<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: count, addSpaces: false));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: count, addSpaces: false));
            Assert.Throws<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: count, addSpaces: false));
        }

        [Fact]
        public void AppendImageTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "ImageText";
            const string url = "ImageUrl";
            const string title = "ImageTitle";

            Image i = Image(text + Chars, url + CharsWithoutSpaces);

            string y = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.Equal(y, mb.AppendImage(i.Text, i.Url).ToStringAndClear());
            Assert.Equal(y, mb.Append(i).ToStringAndClear());
            Assert.Equal(y, mb.Append((object)i).ToStringAndClear());
            Assert.Equal(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.Equal(y + y, mb.AppendRange(i, i).ToStringAndClear());

            i = i.WithTitle(title + Chars);

            y = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.Equal(y, mb.AppendImage(i.Text, i.Url, i.Title).ToStringAndClear());
            Assert.Equal(y, mb.Append(i).ToStringAndClear());
            Assert.Equal(y, mb.Append((object)i).ToStringAndClear());
            Assert.Equal(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.Equal(y + y, mb.AppendRange(i, i).ToStringAndClear());

            Assert.Throws<ArgumentNullException>(() => mb.AppendImage(null, "Url"));
            Assert.Throws<ArgumentNullException>(() => mb.AppendImage("Text", null));
        }

        [Fact]
        public void AppendLinkTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "LinkText";
            const string url = "LinkUrl";
            const string title = "LinkTitle";

            Link i = Link(text + Chars, url + CharsWithoutSpaces);

            string y = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.Equal(y, mb.AppendLink(i.Text, i.Url).ToStringAndClear());
            Assert.Equal(y, mb.Append(i).ToStringAndClear());
            Assert.Equal(y, mb.Append((object)i).ToStringAndClear());
            Assert.Equal(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.Equal(y + y, mb.AppendRange(i, i).ToStringAndClear());

            i = i.WithTitle(title + Chars);

            y = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.Equal(y, mb.AppendLink(i.Text, i.Url, i.Title).ToStringAndClear());
            Assert.Equal(y, mb.Append(i).ToStringAndClear());
            Assert.Equal(y, mb.Append((object)i).ToStringAndClear());
            Assert.Equal(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.Equal(y + y, mb.AppendRange(i, i).ToStringAndClear());

            Assert.Throws<ArgumentNullException>(() => mb.AppendLink(null, "Url"));
            Assert.Throws<ArgumentNullException>(() => mb.AppendLink("Text", null));
        }

        [Theory]
        [InlineData("*", ListItemStyle.Asterisk)]
        [InlineData("-", ListItemStyle.Minus)]
        [InlineData("+", ListItemStyle.Plus)]
        public void AppendListItemTest1(string syntax, ListItemStyle style)
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(listItemStyle: style));

            const string text = "ListItemText";

            Test1(syntax + $" {text + CharsEscaped}" + NewLine, ListItem(text + Chars));

            Test1(syntax + " " + NewLine, ListItem());

            void Test1(string e, ListItem li)
            {
                Assert.Equal(e, mb.AppendListItem(li.Text).ToStringAndClear());
                Assert.Equal(e, mb.AppendListItem((object)li.Text).ToStringAndClear());
                Assert.Equal(e, mb.AppendListItem(li.Text, null).ToStringAndClear());
                Assert.Equal(e, mb.Append(li).ToStringAndClear());
                Assert.Equal(e, mb.Append((object)li).ToStringAndClear());
                Assert.Equal(e, mb.Append(li, escape: true).ToStringAndClear());
                Assert.Equal(e + e, mb.AppendRange(li, li).ToStringAndClear());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void AppendOrderedListItemTest1(int number)
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "OrderedListItemText";

            Test1(number + $". {text + CharsEscaped}" + NewLine, OrderedListItem(number, text + Chars));

            Test1(number + ". " + NewLine, OrderedListItem(number));

            void Test1(string e, OrderedListItem oli)
            {
                Assert.Equal(e, mb.AppendOrderedListItem(number, oli.Text).ToStringAndClear());
                Assert.Equal(e, mb.AppendOrderedListItem(number, (object)oli.Text).ToStringAndClear());
                Assert.Equal(e, mb.AppendOrderedListItem(number, oli.Text, null).ToStringAndClear());
                Assert.Equal(e, mb.Append(oli).ToStringAndClear());
                Assert.Equal(e, mb.Append((object)oli).ToStringAndClear());
                Assert.Equal(e, mb.Append(oli, escape: true).ToStringAndClear());
                Assert.Equal(e + e, mb.AppendRange(oli, oli).ToStringAndClear());
            }
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(-1)]
        public void AppendOrderedListItemTest2(int number)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => mb.AppendOrderedListItem(number));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AppendTaskListItemTest1(bool isCompleted)
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "TaskListItemText";

            string start = $"- [{((isCompleted) ? "x" : " ")}] ";

            Test1(start + text + CharsEscaped + NewLine, TaskListItem(text + Chars, isCompleted));

            Test1(start + NewLine, TaskListItem(null, isCompleted));

            if (isCompleted)
            {
                Test1(start + text + CharsEscaped + NewLine, CompletedTaskListItem(text + Chars));

                Test1(start + NewLine, CompletedTaskListItem(null));
            }

            void Test1(string e, TaskListItem tli)
            {
                if (isCompleted)
                {
                    Assert.Equal(e, mb.AppendCompletedTaskListItem(tli.Text).ToStringAndClear());
                    Assert.Equal(e, mb.AppendCompletedTaskListItem((object)tli.Text).ToStringAndClear());
                    Assert.Equal(e, mb.AppendCompletedTaskListItem(tli.Text, null).ToStringAndClear());
                }
                else
                {
                    Assert.Equal(e, mb.AppendTaskListItem(tli.Text).ToStringAndClear());
                    Assert.Equal(e, mb.AppendTaskListItem((object)tli.Text).ToStringAndClear());
                    Assert.Equal(e, mb.AppendTaskListItem(tli.Text, null).ToStringAndClear());
                }

                Assert.Equal(e, mb.Append(tli).ToStringAndClear());
                Assert.Equal(e, mb.Append((object)tli).ToStringAndClear());
                Assert.Equal(e, mb.Append(tli, escape: true).ToStringAndClear());
                Assert.Equal(e + e, mb.AppendRange(tli, tli).ToStringAndClear());
            }
        }

        [Fact]
        public void AppendQuoteBlockTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "QuoteBlockText";

            const string start = "> ";

            const string md = start + text + CharsEscaped + NewLine;

            Test1(md, QuoteBlock(text + Chars));

            Test1(md + md, QuoteBlock(text + Chars + NewLine + text + Chars));

            void Test1(string e, QuoteBlock x)
            {
                Assert.Equal(e, mb.AppendQuoteBlock(x.Text).ToStringAndClear());
                Assert.Equal(e, mb.Append(x).ToStringAndClear());
                Assert.Equal(e, mb.Append((object)x).ToStringAndClear());
                Assert.Equal(e, mb.Append(x, escape: true).ToStringAndClear());
                Assert.Equal(e + e, mb.AppendRange(x, x).ToStringAndClear());
            }

            Assert.Throws<ArgumentNullException>(() => mb.AppendQuoteBlock(null));
        }
    }
}
