// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class TaskListItemTests
    {
        [Fact]
        public void TaskListItem_Equals()
        {
            TaskListItem item = CreateTaskListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void TaskListItem_NotEquals()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = item.Modify();

            Assert.False(item.Equals((object)item2));
        }

        [Fact]
        public void TaskListItem_IEquatableEquals()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = item;
            IEquatable<TaskListItem> equatable = item;

            Assert.True(equatable.Equals(item2));
        }

        [Fact]
        public void TaskListItem_IEquatableNotEquals()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = CreateTaskListItem().Modify();
            IEquatable<TaskListItem> equatable = item;

            Assert.False(item.Equals(item2));
        }

        [Fact]
        public void TaskListItem_GetHashCode_Equal()
        {
            TaskListItem item = CreateTaskListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void TaskListItem_GetHashCode_NotEqual()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = item.Modify();

            Assert.NotEqual(item.GetHashCode(), item2.GetHashCode());
        }

        [Fact]
        public void TaskListItem_OperatorEquals()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void TaskListItem_OperatorNotEquals()
        {
            TaskListItem item = CreateTaskListItem();
            TaskListItem item2 = item.Modify();

            Assert.True(item != item2);
        }

        [Fact]
        public void TaskListItem_Constructor_AssignText()
        {
            string text = ListItemText();
            var item = new TaskListItem(text: text, isCompleted: TaskListItemIsCompleted());

            Assert.Equal(text, item.Text);
        }

        [Fact]
        public void TaskListItem_WithText()
        {
            string text = ListItemText();

            Assert.Equal(text, CreateTaskListItem().WithText(text).Text);
        }

        [Fact]
        public void TaskListItem_Constructor_AssignIsCompleted()
        {
            bool isCompleted = TaskListItemIsCompleted();
            var item = new TaskListItem(text: ListItemText(), isCompleted: isCompleted);

            Assert.Equal(isCompleted, item.IsCompleted);
        }

        [Fact]
        public void TaskListItem_WithNumber()
        {
            bool isCompleted = TaskListItemIsCompleted();

            Assert.Equal(isCompleted, CreateTaskListItem().WithIsCompleted(isCompleted).IsCompleted);
        }
    }
}
