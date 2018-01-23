// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
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
            MTaskItem item = CreateTaskListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void TaskListItem_GetHashCode_Equal()
        {
            MTaskItem item = CreateTaskListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void TaskListItem_OperatorEquals()
        {
            MTaskItem item = CreateTaskListItem();
            MTaskItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void TaskListItem_Constructor_AssignIsCompleted()
        {
            bool isCompleted = TaskListItemIsCompleted();
            var item = new MTaskItem(isCompleted: isCompleted, content: StringValue());

            Assert.Equal(isCompleted, item.IsCompleted);
        }
    }
}
