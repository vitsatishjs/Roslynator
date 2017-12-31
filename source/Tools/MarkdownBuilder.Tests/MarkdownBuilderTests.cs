// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.TestHelper;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class MarkdownBuilderTests
    {
        private const string Value = SpecialChars;
        private const string ValueEscaped = SpecialCharsEscaped;

        [TestMethod]
        public void BoldTest1()
        {
            const string x = SpecialChars;
            const string y = SpecialCharsEscaped;

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
        public void ItalicTest1()
        {
            const string x = SpecialChars;
            const string y = SpecialCharsEscaped;

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
        public void StrikethroughTest1()
        {
            const string x = SpecialChars;
            const string y = SpecialCharsEscaped;

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
        public void CodeTest1()
        {
            const string x = SpecialCharsEnclosedWithBacktick;
            const string y = SpecialCharsEnclosedWithBacktickDoubled;

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
        public void HeadingTest2()
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
        public void HeadingTest2(int value)
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
        public void HeadingWithoutTextTest1()
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
        public void HeadingUnderlineH1Test1()
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
        public void HeadingUnderlineH2Test1()
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
        public void HeadingUnderlineTest1()
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
        public void HeadingEmptyLineBeforeAfterTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string x = "x";
            string s = "# " + SpecialCharsEscaped + NewLine;
            Heading h = Heading(SpecialChars, 1);

            mb.WithSettings(mb.Settings.WithHeadingOptions(headingOptions: HeadingOptions.None));

            Assert.AreEqual(x + NewLine + s + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBefore));
            Assert.AreEqual(x + NewLine2 + s + NewLine + s + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineAfter));
            Assert.AreEqual(x + NewLine + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithHeadingOptions(HeadingOptions.EmptyLineBeforeAndAfter));
            Assert.AreEqual(x + NewLine2 + s + NewLine + s + NewLine + x, mb.Append(x).Append(h).Append(h).Append(x).ToStringAndClear());
        }
    }
}
