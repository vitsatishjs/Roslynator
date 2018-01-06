// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Pihrtsoft.Markdown.Tests.TestHelpers;
using Xunit;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class ListItemTests
    {
        [Fact]
        public void ListItem_Equals()
        {
            ListItem item = CreateListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void ListItem_NotEquals()
        {
            ListItem item = CreateListItem();
            ListItem item2 = item.Modify();

            Assert.False(item.Equals((object)item2));
        }

        [Fact]
        public void ListItem_IEquatableEquals()
        {
            ListItem item = CreateListItem();
            ListItem item2 = item;
            IEquatable<ListItem> equatable = item;

            Assert.True(equatable.Equals(item2));
        }

        [Fact]
        public void ListItem_IEquatableNotEquals()
        {
            ListItem item = CreateListItem();
            ListItem item2 = CreateListItem().Modify();
            IEquatable<ListItem> equatable = item;

            Assert.False(item.Equals(item2));
        }

        [Fact]
        public void ListItem_GetHashCode_Equal()
        {
            ListItem item = CreateListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void ListItem_GetHashCode_NotEqual()
        {
            ListItem item = CreateListItem();
            ListItem item2 = item.Modify();

            Assert.NotEqual(item.GetHashCode(), item2.GetHashCode());
        }

        [Fact]
        public void ListItem_OperatorEquals()
        {
            ListItem item = CreateListItem();
            ListItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void ListItem_OperatorNotEquals()
        {
            ListItem item = CreateListItem();
            ListItem item2 = item.Modify();

            Assert.True(item != item2);
        }

        [Fact]
        public void ListItem_Constructor_AssignText()
        {
            string text = ListItemText();
            var item = new ListItem(text: text);

            Assert.Equal(text, item.Text);
        }

        [Fact]
        public void ListItem_WithText()
        {
            string text = ListItemText();

            Assert.Equal(text, CreateListItem().WithText(text).Text);
        }
    }
}
