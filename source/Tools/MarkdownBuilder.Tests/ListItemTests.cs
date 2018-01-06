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
            BulletListItem item = CreateListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void ListItem_NotEquals()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = item.Modify();

            Assert.False(item.Equals((object)item2));
        }

        [Fact]
        public void ListItem_IEquatableEquals()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = item;
            IEquatable<BulletListItem> equatable = item;

            Assert.True(equatable.Equals(item2));
        }

        [Fact]
        public void ListItem_IEquatableNotEquals()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = CreateListItem().Modify();
            IEquatable<BulletListItem> equatable = item;

            Assert.False(item.Equals(item2));
        }

        [Fact]
        public void ListItem_GetHashCode_Equal()
        {
            BulletListItem item = CreateListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void ListItem_GetHashCode_NotEqual()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = item.Modify();

            Assert.NotEqual(item.GetHashCode(), item2.GetHashCode());
        }

        [Fact]
        public void ListItem_OperatorEquals()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void ListItem_OperatorNotEquals()
        {
            BulletListItem item = CreateListItem();
            BulletListItem item2 = item.Modify();

            Assert.True(item != item2);
        }

        [Fact]
        public void ListItem_Constructor_AssignText()
        {
            string text = ListItemText();
            var item = new BulletListItem(text: text);

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
