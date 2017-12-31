// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class TaskListItemTests
    {
        [TestMethod]
        public void TaskListItemTest1()
        {
            TaskListItem x = MarkdownFactory.TaskListItem("TaskListItemText", isCompleted: true);

            string text = x.Text;
            bool isCompleted = x.IsCompleted;

            string text2 =  text.Modify();
            bool isCompleted2 = isCompleted.Modify();

            Assert.AreNotEqual(text, text2);
            Assert.AreNotEqual(isCompleted, isCompleted2);

            TestEquality(x, x.WithText(text2));
            TestEquality(x, x.WithIsCompleted(isCompleted2));

            Assert.AreEqual(text2, x.WithText(text2).Text);
            Assert.AreEqual(isCompleted2, x.WithIsCompleted(isCompleted2).IsCompleted);

            Assert.AreEqual(x, x.WithText(text));
            Assert.AreEqual(x, x.WithIsCompleted(isCompleted));

            Assert.AreNotEqual(x, x.WithText(text2));
            Assert.AreNotEqual(x, x.WithIsCompleted(isCompleted2));
        }

        private static void TestEquality(TaskListItem x, TaskListItem y)
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
