// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class HorizontalRuleTests
    {
        [TestMethod]
        public void HorizontalRuleTest1()
        {
            HorizontalRule x = HorizontalRule.Default;

            HorizontalRuleStyle style = x.Style;
            int count = x.Count;
            bool addSpaces = x.AddSpaces;

            HorizontalRuleStyle style2 = style.Modify();
            int count2 = count.Modify();
            bool addSpaces2 = addSpaces.Modify();

            Assert.AreNotEqual(style, style2);
            Assert.AreNotEqual(count, count2);
            Assert.AreNotEqual(addSpaces, addSpaces2);

            TestEquality(x, x.WithStyle(style2));
            TestEquality(x, x.WithCount(count2));
            TestEquality(x, x.WithAddSpaces(addSpaces2));

            Assert.AreEqual(style2, x.WithStyle(style2).Style);
            Assert.AreEqual(count2, x.WithCount(count2).Count);
            Assert.AreEqual(addSpaces2, x.WithAddSpaces(addSpaces2).AddSpaces);

            Assert.AreEqual(x, x.WithStyle(style));
            Assert.AreEqual(x, x.WithCount(count));
            Assert.AreEqual(x, x.WithAddSpaces(addSpaces));

            Assert.AreNotEqual(x, x.WithStyle(style2));
            Assert.AreNotEqual(x, x.WithCount(count2));
            Assert.AreNotEqual(x, x.WithAddSpaces(addSpaces2));
        }

        private static void TestEquality(HorizontalRule x, HorizontalRule y)
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
