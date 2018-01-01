// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class MarkdownBuilderTests
    {
        private const string Value = Chars;
        private const string ValueEscaped = CharsEscaped;

        [TestMethod]
        public void AppendBoldTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.AreEqual("**" + y + "**", mb.AppendBold(x).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.AppendBold((object)x).ToStringAndClear());
            Assert.AreEqual("**" + y + y + "**", mb.AppendBold(x, x).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x)).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithBoldStyle(EmphasisStyle.Asterisk));
            Assert.AreEqual("**" + y + "**", mb.AppendBold(x).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.AppendBold((object)x).ToStringAndClear());
            Assert.AreEqual("**" + y + y + "**", mb.AppendBold(x, x).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x)).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.AreEqual("**" + y + "**", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithBoldStyle(EmphasisStyle.Underscore));
            Assert.AreEqual("__" + y + "__", mb.AppendBold(x).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.AppendBold((object)x).ToStringAndClear());
            Assert.AreEqual("__" + y + y + "__", mb.AppendBold(x, x).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append(Bold(x)).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append((object)Bold(x)).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append(Bold(x), escape: true).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append(x, EmphasisOptions.Bold).ToStringAndClear());
            Assert.AreEqual("__" + y + "__", mb.Append(x, EmphasisOptions.Bold, escape: true).ToStringAndClear());
        }

        [TestMethod]
        public void AppendItalicTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.AreEqual("*" + y + "*", mb.AppendItalic(x).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.AreEqual("*" + y + y + "*", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x)).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithItalicStyle(EmphasisStyle.Asterisk));
            Assert.AreEqual("*" + y + "*", mb.AppendItalic(x).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.AreEqual("*" + y + y + "*", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x)).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.AreEqual("*" + y + "*", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithItalicStyle(EmphasisStyle.Underscore));
            Assert.AreEqual("_" + y + "_", mb.AppendItalic(x).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.AppendItalic((object)x).ToStringAndClear());
            Assert.AreEqual("_" + y + y + "_", mb.AppendItalic(x, x).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append(Italic(x)).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append((object)Italic(x)).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append(Italic(x), escape: true).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append(x, EmphasisOptions.Italic).ToStringAndClear());
            Assert.AreEqual("_" + y + "_", mb.Append(x, EmphasisOptions.Italic, escape: true).ToStringAndClear());
        }

        [TestMethod]
        public void AppendStrikethroughTest1()
        {
            const string x = Chars;
            const string y = CharsEscaped;

            MarkdownBuilder mb = CreateBuilder();

            Assert.AreEqual("~~" + y + "~~", mb.AppendStrikethrough(x).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.AppendStrikethrough((object)x).ToStringAndClear());
            Assert.AreEqual("~~" + y + y + "~~", mb.AppendStrikethrough(x, x).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append(Strikethrough(x)).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append(Strikethrough(x), escape: true).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append((object)Strikethrough(x)).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append(Strikethrough(x), escape: true).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append(x, EmphasisOptions.Strikethrough).ToStringAndClear());
            Assert.AreEqual("~~" + y + "~~", mb.Append(x, EmphasisOptions.Strikethrough, escape: true).ToStringAndClear());
        }

        [TestMethod]
        public void AppendCodeTest1()
        {
            const string x = CharsEnclosedWithBacktick;
            const string y = CharsEnclosedWithBacktickDoubled;

            MarkdownBuilder mb = CreateBuilder();

            Assert.AreEqual("` " + y + " `", mb.AppendCode(x).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.AppendCode((object)x).ToStringAndClear());
            Assert.AreEqual("` " + y + y + " `", mb.AppendCode(x, x).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append(Code(x)).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append(Code(x), escape: true).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append((object)Code(x)).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append(Code(x), escape: true).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append(x, EmphasisOptions.Code).ToStringAndClear());
            Assert.AreEqual("` " + y + " `", mb.Append(x, EmphasisOptions.Code, escape: true).ToStringAndClear());
        }

        [TestMethod]
        public void AppendHeadingTest2()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.AreEqual($"# {ValueEscaped}{NewLine}", mb.AppendHeading1(Value).ToStringAndClear());
            Assert.AreEqual($"## {ValueEscaped}{NewLine}", mb.AppendHeading2(Value).ToStringAndClear());
            Assert.AreEqual($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.AreEqual($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.AreEqual($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.AreEqual($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        public void AppendHeadingTest2(int value)
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.AreEqual($"{new string('#', value)} {ValueEscaped}{NewLine}", mb.AppendHeading(value, Value).ToStringAndClear());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public void HeadingLevelOutOfRangeTest1(int level)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.AppendHeading(level, Value));
        }

        [TestMethod]
        public void AppendHeadingWithoutTextTest1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.None));

            Assert.AreEqual("# ", mb.AppendHeading1().ToStringAndClear());
            Assert.AreEqual("## ", mb.AppendHeading2().ToStringAndClear());
            Assert.AreEqual("### ", mb.AppendHeading3().ToStringAndClear());
            Assert.AreEqual("#### ", mb.AppendHeading4().ToStringAndClear());
            Assert.AreEqual("##### ", mb.AppendHeading5().ToStringAndClear());
            Assert.AreEqual("###### ", mb.AppendHeading6().ToStringAndClear());
        }

        [TestMethod]
        public void AppendHeadingUnderlineH1Test1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.UnderlineH1));

            Assert.AreEqual(ValueEscaped + NewLine + new string('=', ValueEscaped.Length) + NewLine, mb.AppendHeading1(Value).ToStringAndClear());
            Assert.AreEqual($"## {ValueEscaped}{NewLine}", mb.AppendHeading2(Value).ToStringAndClear());
            Assert.AreEqual($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.AreEqual($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.AreEqual($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.AreEqual($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [TestMethod]
        public void AppendHeadingUnderlineH2Test1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.UnderlineH2));

            Assert.AreEqual($"# {ValueEscaped}{NewLine}", mb.AppendHeading1(Value).ToStringAndClear());
            Assert.AreEqual(ValueEscaped + NewLine + new string('-', ValueEscaped.Length) + NewLine, mb.AppendHeading2(Value).ToStringAndClear());
            Assert.AreEqual($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.AreEqual($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.AreEqual($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.AreEqual($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [TestMethod]
        public void AppendHeadingUnderlineTest1()
        {
            MarkdownBuilder mb = CreateBuilder(new MarkdownSettings(headingOptions: HeadingOptions.Underline));

            Assert.AreEqual(ValueEscaped + NewLine + new string('=', ValueEscaped.Length) + NewLine, mb.AppendHeading1(Value).ToStringAndClear());
            Assert.AreEqual(ValueEscaped + NewLine + new string('-', ValueEscaped.Length) + NewLine, mb.AppendHeading2(Value).ToStringAndClear());
            Assert.AreEqual($"### {ValueEscaped}{NewLine}", mb.AppendHeading3(Value).ToStringAndClear());
            Assert.AreEqual($"#### {ValueEscaped}{NewLine}", mb.AppendHeading4(Value).ToStringAndClear());
            Assert.AreEqual($"##### {ValueEscaped}{NewLine}", mb.AppendHeading5(Value).ToStringAndClear());
            Assert.AreEqual($"###### {ValueEscaped}{NewLine}", mb.AppendHeading6(Value).ToStringAndClear());
        }

        [TestMethod]
        public void AppendHeadingEmptyLineBeforeAfterTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string x = "x";
            string s = "# " + CharsEscaped + NewLine;
            Heading h = Heading(Chars, 1);

            mb.WithSettings(mb.Settings.WithHeadingOptions(headingOptions: HeadingOptions.None));

            Assert.AreEqual(x + NewLine + s + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBefore));
            Assert.AreEqual(x + NewLine2 + s + NewLine + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineAfter));
            Assert.AreEqual(x + NewLine + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBeforeAndAfter));
            Assert.AreEqual(x + NewLine2 + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());
        }

        [TestMethod]
        public void AppendHorizontalRuleTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            HorizontalRule hr = HorizontalRule.Default;
            Assert.AreEqual("- - -" + NewLine, mb.Append(hr).ToStringAndClear());

            Assert.AreEqual("***" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("* * *" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.AreEqual("*****" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("* * * * *" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.AreEqual("***" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("* * *" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 3, addSpaces: true).ToStringAndClear());
            Assert.AreEqual("*****" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("* * * * *" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: 5, addSpaces: true).ToStringAndClear());

            Assert.AreEqual("---" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("- - -" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.AreEqual("-----" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("- - - - -" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.AreEqual("---" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("- - -" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 3, addSpaces: true).ToStringAndClear());
            Assert.AreEqual("-----" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("- - - - -" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: 5, addSpaces: true).ToStringAndClear());

            Assert.AreEqual("___" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("_ _ _" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: true)).ToStringAndClear());
            Assert.AreEqual("_____" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: false)).ToStringAndClear());
            Assert.AreEqual("_ _ _ _ _" + NewLine, mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: true)).ToStringAndClear());

            Assert.AreEqual("___" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("_ _ _" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 3, addSpaces: true).ToStringAndClear());
            Assert.AreEqual("_____" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: false).ToStringAndClear());
            Assert.AreEqual("_ _ _ _ _" + NewLine, mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: 5, addSpaces: true).ToStringAndClear());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void AppendHorizontalRuleTest2(int count)
        {
            MarkdownBuilder mb = CreateBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Asterisk, count: count, addSpaces: false)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Hyphen, count: count, addSpaces: false)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.Append(HorizontalRule(style: HorizontalRuleStyle.Underscore, count: count, addSpaces: false)));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Asterisk, count: count, addSpaces: false));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Hyphen, count: count, addSpaces: false));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => mb.AppendHorizontalRule(style: HorizontalRuleStyle.Underscore, count: count, addSpaces: false));
        }

        [TestMethod]
        public void AppendImageTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "ImageText";
            const string url = "ImageUrl";
            const string title = "ImageTitle";

            Image i = Image(text + Chars, url + CharsWithoutSpaces);

            string y = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.AreEqual(y, mb.AppendImage(i.Text, i.Url).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append((object)i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.AreEqual(y + y, mb.AppendRange(i, i).ToStringAndClear());

            i = i.WithTitle(title + Chars);

            y = $"![{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.AreEqual(y, mb.AppendImage(i.Text, i.Url, i.Title).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append((object)i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.AreEqual(y + y, mb.AppendRange(i, i).ToStringAndClear());

            Assert.ThrowsException<ArgumentNullException>(() => mb.AppendImage(null, "Url"));
            Assert.ThrowsException<ArgumentNullException>(() => mb.AppendImage("Text", null));
        }

        [TestMethod]
        public void AppendLinkTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string text = "LinkText";
            const string url = "LinkUrl";
            const string title = "LinkTitle";

            Link i = Link(text + Chars, url + CharsWithoutSpaces);

            string y = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped})";

            Assert.AreEqual(y, mb.AppendLink(i.Text, i.Url).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append((object)i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.AreEqual(y + y, mb.AppendRange(i, i).ToStringAndClear());

            i = i.WithTitle(title + Chars);

            y = $"[{text + CharsSquareBracketsBacktickEscaped}]({url + CharsWithoutSpacesParenthesesEscaped} \"{title + CharsDoubleQuoteEscaped}\")";

            Assert.AreEqual(y, mb.AppendLink(i.Text, i.Url, i.Title).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append((object)i).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(i, escape: true).ToStringAndClear());
            Assert.AreEqual(y + y, mb.AppendRange(i, i).ToStringAndClear());

            Assert.ThrowsException<ArgumentNullException>(() => mb.AppendLink(null, "Url"));
            Assert.ThrowsException<ArgumentNullException>(() => mb.AppendLink("Text", null));
        }
    }
}
