// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.Tests.TestHelper;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class HeadingTests
    {
        private const string Value = SpecialChars;
        private const string ValueEscaped = SpecialCharsEscaped;

        [TestMethod]
        public void HeadingTest1()
        {
            Heading x = MarkdownFactory.Heading("HeadingText", 1);

            string text = x.Text;
            int level = x.Level;

            string text2 = text.Modify();
            int level2 = level.Modify();

            Assert.AreNotEqual(text, text2);
            Assert.AreNotEqual(level, level2);

            TestEquality(x, x.WithText(text2));
            TestEquality(x, x.WithLevel(level2));

            Assert.AreEqual(text2, x.WithText(text2).Text);
            Assert.AreEqual(level2, x.WithLevel(level2).Level);

            Assert.AreEqual(x, x.WithText(text));
            Assert.AreEqual(x, x.WithLevel(level));

            Assert.AreNotEqual(x, x.WithText(text2));
            Assert.AreNotEqual(x, x.WithLevel(level2));
        }

        private static void TestEquality(Heading x, Heading y)
        {
            Assert.AreEqual(x, x);
            Assert.IsTrue(x == x);
            Assert.IsFalse(x != x);

            Assert.AreNotEqual(x, y);
            Assert.IsFalse(x == y);
            Assert.IsTrue(x != y);
            Assert.IsFalse(x.GetHashCode() == y.GetHashCode());
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
    }
}
