// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class OrderedListItemTests
    {
        [TestMethod]
        public void OrderedListItemTest1()
        {
            OrderedListItem x = MarkdownFactory.OrderedListItem(1, "OrderedListItemText");

            int number = x.Number;
            string text = x.Text;

            int number2 = number.Modify();
            string text2 =  text.Modify();

            Assert.AreNotEqual(number, number2);
            Assert.AreNotEqual(text, text2);

            TestEquality(x, x.WithNumber(number2));
            TestEquality(x, x.WithText(text2));

            Assert.AreEqual(number2, x.WithNumber(number2).Number);
            Assert.AreEqual(text2, x.WithText(text2).Text);

            Assert.AreEqual(x, x.WithNumber(number));
            Assert.AreEqual(x, x.WithText(text));

            Assert.AreNotEqual(x, x.WithNumber(number2));
            Assert.AreNotEqual(x, x.WithText(text2));
        }

        private static void TestEquality(OrderedListItem x, OrderedListItem y)
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
