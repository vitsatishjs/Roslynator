// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class TableColumnTests
    {
        [TestMethod]
        public void TableColumnTest1()
        {
            TableColumn x = MarkdownFactory.TableColumn("TableColumnName", Alignment.Center);

            string name = x.Name;
            Alignment alignment = x.Alignment;

            string name2 =  name.Modify();
            Alignment alignment2 = alignment.Modify();

            Assert.AreNotEqual(name, name2);
            Assert.AreNotEqual(alignment, alignment2);

            TestEquality(x, x.WithName(name2));
            TestEquality(x, x.WithAlignment(alignment2));

            Assert.AreEqual(name2, x.WithName(name2).Name);
            Assert.AreEqual(alignment2, x.WithAlignment(alignment2).Alignment);

            Assert.AreEqual(x, x.WithName(name));
            Assert.AreEqual(x, x.WithAlignment(alignment));

            Assert.AreNotEqual(x, x.WithName(name2));
            Assert.AreNotEqual(x, x.WithAlignment(alignment2));
        }

        private static void TestEquality(TableColumn x, TableColumn y)
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
