// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class HeadingTests
    {
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
    }
}
