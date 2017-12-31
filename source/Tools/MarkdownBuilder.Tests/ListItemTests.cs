// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class ListItemTests
    {
        [TestMethod]
        public void ListItemTest1()
        {
            ListItem x = MarkdownFactory.ListItem("ListItemText");

            string text = x.Text;

            string text2 =  text.Modify();

            Assert.AreNotEqual(text, text2);

            TestEquality(x, x.WithText(text2));

            Assert.AreEqual(text2, x.WithText(text2).Text);

            Assert.AreEqual(x, x.WithText(text));

            Assert.AreNotEqual(x, x.WithText(text2));
        }

        private static void TestEquality(ListItem x, ListItem y)
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
